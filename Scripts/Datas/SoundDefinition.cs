using Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "Sound Definition",menuName = "Project 42/New Sound Definition")]
    public class SoundDefinition : ScriptableObject
    {
        [Header("My Sounds List")]
        [SerializeField] private AudioClip[] _sounds;

        public AudioClip[] GetSounds()
        {
            return _sounds;
        }
        [Header("My Music List - Music Only")]
        [SerializeField] private AudioClip[] _music;

        public AudioClip[] GetMusic()
        {
            return _music;
        }
        private int _playIndex = 0;
        [Range(0f, 1f)]
        [SerializeField] private float _volume;
        [Range(0.1f, 3f)]
        [SerializeField] private float _pitch = 1;
        [SerializeField] private Vector2 _randomPitch;

        [Header("Loop for Music")]
        [SerializeField] private bool _loop;

        private enum AudioSeq { ORDER, RANDOM , SINGLE_CHOICE }
        [Header("Sound Sequencer")]
        [SerializeField] private AudioSeq _audioMode;
        private enum AudioEffect { RANDOM_PITCH, NO_EFFECTS }
        [Header("Audio Effects")]
        [SerializeField] private AudioEffect _audioEffects;
      
        public void PlayAsSFX(AudioSource audioSource, int index)
        {
            audioSource.pitch= _pitch;
            audioSource.volume= _volume;
            switch (_audioEffects)
            {
                case AudioEffect.RANDOM_PITCH:
                    audioSource.pitch = Random.Range(_randomPitch.x, _randomPitch.y);
                    break;
                case AudioEffect.NO_EFFECTS:                    
                    break;                
            }

            switch (_audioMode)
            {
                case AudioSeq.ORDER:
                    _playIndex = ((_playIndex + 1) % _sounds.Length);
                    audioSource.PlayOneShot(_sounds[_playIndex]);
                    break;
                case AudioSeq.RANDOM:
                    _playIndex = Random.Range(0, _sounds.Length);
                    audioSource.PlayOneShot(_sounds[_playIndex]);
                    break;
                case AudioSeq.SINGLE_CHOICE:
                    audioSource.PlayOneShot(_sounds[index]);
                    break;
            }                 
        }

        public void PlayOneSFX(AudioSource audioSource, int index , float volume, float pitch)
        {
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioSource.PlayOneShot(_sounds[index]);
        }

        public void PlayOneSFX(AudioSource audioSource, int index)
        {
            audioSource.PlayOneShot(_sounds[index]);           
        }

        public void PlayOneMusic(AudioSource audioSource, int index, float volume , float pitch)
        {
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            PlayOneMusic(audioSource, index);
        }

        public void PlayOneMusic(AudioSource audioSource, int index)
        {
            audioSource.clip = _music[index];
            audioSource.Play();
        }

        public void PlaySoundOnLoop(AudioSource audioSource, int index)
        {
            audioSource.loop = _loop;
            audioSource.clip = _sounds[index];
            audioSource.Play();
        }

        public void PlayMusicOnLoop(AudioSource audioSource, int index)
        {
            audioSource.loop = _loop;
            audioSource.clip = _music[index];
            audioSource.Play();
        }

        public void PlayAsMusic(AudioSource audioSource, int index)
        {
            audioSource.loop = _loop;           
            switch (_audioMode)
            {
                case AudioSeq.ORDER:
                    index = ((index + 1) % _sounds.Length);
                    audioSource.clip = _sounds[index];
                    audioSource.Play();
                    break;
                case AudioSeq.RANDOM:
                    index = Random.Range(0, _sounds.Length);
                    audioSource.clip = _sounds[index];
                    audioSource.Play();
                    break;
                case AudioSeq.SINGLE_CHOICE:
                    audioSource.clip = _sounds[index];
                    audioSource.Play();
                    break;
            }
        }
    }
}
