using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI.Menu
{
    public class PauseMenuHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReference _pauseAction;

        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _optionMenu;
        [SerializeField] private GameObject _statsMenu;
        [SerializeField] private GameObject _restartConfirmation;
        [SerializeField] private GameObject _mainMenuConfirmation;
        [SerializeField] private GameObject _quitConfirmation;
        [SerializeField] private GameObject _resolutionConfirmationPopUp;  

        //Stats                      
        [SerializeField] private TMP_Text _statisticName;
        [SerializeField] private TMP_Text _statisticValue;       
        [SerializeField] private TMP_Dropdown _resolutionDropDown;
        [SerializeField] private TMP_Text _quitUsername;
        [SerializeField] private TMP_Text _mainmenuUsername;

        //Sounds
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _UISlider;
        [SerializeField] private Slider _fxSlider;

        private Coroutine _resolutionConfirmation;
        private CanvasGroup _canvasGroup;
        
        private Resolution _currentResolution;
        private Resolution[] _resolutionsArray;
        private bool _fullScreen;

        public void Pause(InputAction.CallbackContext ctx) => Pause();
        public void Resume(InputAction.CallbackContext ctx) => Resume();

        public void ClickSound() => AudioManager.Instance.PlayUI(0);
        public void HoverSound() => AudioManager.Instance.PlayUI(1);
        
        public void SetMasterVolume(float volume) => AudioManager.Instance.SetLevelMaster(volume);
        public void SetMusicVolume(float volume) => AudioManager.Instance.SetLevelMusic(volume);
        public void SetUIVolume(float volume) => AudioManager.Instance.SetLevelUI(volume);
        public void SetSfxVolume(float volume) => AudioManager.Instance.SetLevelSfx(volume);
        
        private void InitSoundSliders()
        {
            _masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            _UISlider.value = PlayerPrefs.GetFloat("UIVolume");
            _fxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        
        protected void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            
            _resolutionsArray = Screen.resolutions;
            _currentResolution = Screen.currentResolution;
            _fullScreen = Screen.fullScreen;
            
            _pauseAction.action.performed += Pause;
            _pauseAction.action.Enable();
            
            InitSoundSliders();
            GenerateResolutionsList();
            HideAll();
        }

        private void OnDisable()
        {
            _pauseAction.action.performed -= Pause;
            _pauseAction.action.performed -= Resume;
            _pauseAction.action.Disable();
        }
        
        private void HideAll()
        {
            _pauseMenu.SetActive(false);
            _optionMenu.SetActive(false);
            _statsMenu.SetActive(false);
            _restartConfirmation.SetActive(false);
            _quitConfirmation.SetActive(false);           
        }

        private void GenerateResolutionsList()
        {
            _resolutionDropDown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutionsArray.Length; i++)
            {
                options.Add(_resolutionsArray[i].ToString());

                if (_resolutionsArray[i].Equals(_currentResolution))
                    currentResolutionIndex = i;
            }

            _resolutionDropDown.AddOptions(options);
            _resolutionDropDown.value = currentResolutionIndex;
            _resolutionDropDown.RefreshShownValue();
        }

        public void SetResolution(int i)
        {
            if(Screen.currentResolution.Equals(_resolutionsArray[i]))
                return;
            
            _currentResolution = Screen.currentResolution;
            Screen.SetResolution(_resolutionsArray[i].width, _resolutionsArray[i].height, _fullScreen, _resolutionsArray[i].refreshRate);
            
            CancelRevertResolution();
            _resolutionConfirmation = StartCoroutine(RevertResolution());
        }

        private IEnumerator RevertResolution()
        {
            _resolutionConfirmationPopUp.SetActive(true);
            yield return new WaitForSecondsRealtime(15);
            RevertToOldResolution();
        }

        public void RevertToOldResolution()
        {
            Screen.SetResolution(_currentResolution.width, _currentResolution.height, _fullScreen, _currentResolution.refreshRate);
            CancelRevertResolution();
        }

        public void CancelRevertResolution()
        {
            _resolutionConfirmationPopUp.SetActive(false);
            if(_resolutionConfirmation != null)
                StopCoroutine(_resolutionConfirmation);

            _resolutionConfirmation = null;
        }

        private void ListOfStats()
        {
            string[] nameList = Enum.GetNames(typeof(StatisticsManager.GameStatistic));
            string statsName = string.Empty;
            string statsValue = string.Empty;

            for (int i = 0; i < StatisticsManager.Instance._sessionStatistics.Length; i++)
            {
                statsName += nameList[i] + "\n" + "\n";
                statsValue += StatisticsManager.Instance._sessionStatistics[i] + "\n" + "\n";
            }

            _statisticName.text = statsName;
            _statisticValue.text = statsValue;
        }
     
        private IEnumerator FadeIn()
        {          
            float l = 0;
            while (l < 1)
            {
                l += Time.deltaTime * 10f;
                _canvasGroup.alpha = Mathf.Lerp(0, 1, l);
                yield return null;
            }
            Time.timeScale = 0;
            Cursor.visible = true;
            AudioListener.pause = true;
        }
        
        private IEnumerator FadeOut()
        {        
            Cursor.visible = false;
            Time.timeScale = 1;
            float l = 1;
            while (l > 0)
            {
                l -= Time.deltaTime * 10f;
                _canvasGroup.alpha = Mathf.Lerp(0, 1, l);
                yield return null;
            }
            HideAll();
            AudioListener.pause = false;
        }

        public void Pause()
        {
            HideAll();
            _pauseMenu.SetActive(true);
            _pausePanel.SetActive(true);
            StartCoroutine(FadeIn());
            _pauseAction.action.performed -= Pause;
            _pauseAction.action.performed += Resume;
            ListOfStats();
        }
        
        public void Resume()
        {           
            StartCoroutine(FadeOut());
            
            _pauseAction.action.performed -= Resume;
            _pauseAction.action.performed += Pause;
        }
        
        
        public void Restart()
        {
            Resume();
            _pauseMenu.SetActive(true);
            _restartConfirmation.SetActive(false);
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            WorldManager.ResetWorld();
        }
      
        public void OpenMainMenuConfirmation()
        {
            _pauseMenu.SetActive(false);                                
            _mainMenuConfirmation.SetActive(true);
        }
      
        public void CancelMainMenuConfirmation()
        {
            _mainMenuConfirmation.SetActive(false);
            _pauseMenu.SetActive(true);
        }
      
        public void OptionsMenu()
        {
            _pauseMenu.SetActive(false);                                
            _optionMenu.SetActive(true);
        }
        
        public void StatsMenu()
        {
            ListOfStats();
            _pauseMenu.SetActive(false);
            _statsMenu.SetActive(true);
        }

        public void CloseStatsMenu()
        {
            _statsMenu.SetActive(false);
            _pauseMenu.SetActive(true);                     
        }

        public void QuitGame()
        {
            _pauseMenu.SetActive(false);
            _quitConfirmation.SetActive(true);
        }
        
        public void QuitDenied()
        {
            _quitConfirmation.SetActive(false);           
            _pauseMenu.SetActive(true);
        }
        
        public void RestartConfirm()
        {
            _pauseMenu.SetActive(false);
            _restartConfirmation.SetActive(true);
        }
        
        public void RestartDenied()
        {         
            _restartConfirmation.SetActive(false);
            _pauseMenu.SetActive(true);
        }
       
        public void CloseOptions()
        {
            _optionMenu.SetActive(false);
            _pauseMenu.SetActive(true);
        }
        
        public void SetFullScreen(bool isFullScreen)
        {
            _fullScreen = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }

        public void ReturnMainMenu()
        {
            Time.timeScale = 1;
            SaveManager.SaveGame(_mainmenuUsername.text);
            SceneManager.LoadScene(0);
        }
        
        public void Exit()
        {
            SaveManager.SaveGame(_quitUsername.text);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
             
        // Localization Functions
        public void ChangeLocale(int localeID)
        {
            LocalizationManager.Instance.ChangeLocale(localeID);
        }
    }
}