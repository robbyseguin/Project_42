using Datas;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
            
        [SerializeField] private AudioMixer _mixer;

        [Header("Add Child GameObjects")]
        [SerializeField] public AudioSource _musicSource;
        [SerializeField] public AudioSource _uiSource;
        [SerializeField] public AudioSource _sfxSource;

        [Header("Add Hover Sound")]
        [SerializeField] private AudioClip _hoverSound;
        [Header("Add Click Sound")]
        [SerializeField] private AudioClip _clickSound;

        [Header("Add Scriptable Object")]

        [SerializeField] private SoundDefinition _myMusic;
        [SerializeField] private SoundDefinition _myUISounds;
        [SerializeField] private SoundDefinition _mySFXSounds;

        private AudioClip[] _myMusicArray;
        private AudioClip[] _myUISoundsArray;
        private AudioClip[] _mySFXSoundsArray;
              
        protected override void Awake()
        {
            base.Awake();
            
            if(!IsSingleton)
                return;

            SetMixer();
            _myMusicArray = _myMusic.GetMusic();
            _myUISoundsArray = _myUISounds.GetSounds();
            _mySFXSoundsArray = _mySFXSounds.GetSounds();
            _uiSource.ignoreListenerPause= true;
            
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            _musicSource.volume = 0.75f;
            _uiSource.volume = 1f;
            // Play Main Menu Song at Start ( Testing Reasons )
            PlayMusic(0);
        }
        
        public void PlayMusic(int index)
        {
            _musicSource.loop = true;
            _musicSource.clip = _myMusicArray[index];
            _musicSource.Play();
        }
      
        public void StopMusic()
        {
            _musicSource.Stop();
        }
        
        public void PlayUI(int index)
        {
            _uiSource.PlayOneShot(_myUISoundsArray[index]);
        }
        public void PlaySFX(int index)
        {
            _sfxSource.PlayOneShot(_mySFXSoundsArray[index]);
        }

        //UI Functions for Pressed / Hover Etc..

        public void HoverSound()
        {
            _uiSource.PlayOneShot(_hoverSound);
        }

        public void ClickSound()
        {
            _uiSource.PlayOneShot(_clickSound);
        }

        // Volume in Settings
        public void SetLevelMaster(float volume)
        {
            PlayerPrefs.SetFloat("masterVolume", volume);
            _mixer.SetFloat("masterVolume", volume);
        }
        public void SetLevelMusic(float volume)
        {
            PlayerPrefs.SetFloat("musicVolume", volume);
            _mixer.SetFloat("musicVolume", volume);
        }
        public void SetLevelUI(float volume)
        {
            PlayerPrefs.SetFloat("UIVolume", volume);
            _mixer.SetFloat("UIVolume", volume);
        }
        public void SetLevelSfx(float volume)
        {
            PlayerPrefs.SetFloat("SFXVolume", volume);
            _mixer.SetFloat("SFXVolume", volume);
        }

        private void SetMixer()
        {
            SetLevelMaster(PlayerPrefs.GetFloat("masterVolume"));
            SetLevelMusic(PlayerPrefs.GetFloat("musicVolume"));
            SetLevelUI(PlayerPrefs.GetFloat("UIVolume"));
            SetLevelSfx(PlayerPrefs.GetFloat("SFXVolume"));
        }
    }
}


