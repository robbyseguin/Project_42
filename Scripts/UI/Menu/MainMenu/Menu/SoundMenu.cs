using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu.MainMenu
{
    public class SoundMenu : Menu
    {
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _UISlider;
        [SerializeField] private Slider _fxSlider;

        protected void Awake()
        {
            _masterSlider.onValueChanged.AddListener(OnMasterVolumeChange);
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
            _UISlider.onValueChanged.AddListener(OnUIVolumeChange);
            _fxSlider.onValueChanged.AddListener(OnSfxVolumeChange);
        }
        
        private void OnMasterVolumeChange(float volume) => AudioManager.Instance.SetLevelMaster(volume);
        private void OnMusicVolumeChange(float volume) => AudioManager.Instance.SetLevelMusic(volume);
        private void OnUIVolumeChange(float volume) => AudioManager.Instance.SetLevelUI(volume);
        private void OnSfxVolumeChange(float volume) => AudioManager.Instance.SetLevelSfx(volume);
        
        public void InitSoundSliders()
        {
            _masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            _UISlider.value = PlayerPrefs.GetFloat("UIVolume");
            _fxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }
}
