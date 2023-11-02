using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.MainMenu
{
    public class GraphicsMenu : Menu
    {
        [SerializeField] private GameObject _resolutionConfirmationPopUp;
        [SerializeField] private TMP_Dropdown _resolutionDropDown;
        [SerializeField] private Toggle _fullscreenToggle;

        private Coroutine _resolutionConfirmation;
        private Resolution _currentResolution;
        private List<Resolution> _resolutionsArray = new List<Resolution>();
        private bool _fullScreen;
        private int _lastIndex;

        protected void Awake()
        {
            _resolutionsArray = Screen.resolutions.ToList();
            _currentResolution = Screen.currentResolution;
            _fullScreen = Screen.fullScreen;

            GenerateResolutionsList();
            
            _resolutionDropDown.onValueChanged.AddListener(SetResolution);
            _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        private void SetFullscreen(bool value)
        {
            Screen.fullScreen = _fullScreen = value;
        }

        private void GenerateResolutionsList()
        {
            _resolutionDropDown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutionsArray.Count; i++)
            {
                options.Add(_resolutionsArray[i].ToString());

                if (_resolutionsArray[i].Equals(_currentResolution))
                    currentResolutionIndex = i;
            }

            _resolutionDropDown.AddOptions(options);
            _resolutionDropDown.value = currentResolutionIndex;
            _lastIndex = currentResolutionIndex;
            _resolutionDropDown.RefreshShownValue();
        }

        private void SetResolution(int selectedResolution)
        {
            if(Screen.currentResolution.Equals(_resolutionsArray[selectedResolution]))
                return;

            _lastIndex = _resolutionsArray.IndexOf(_currentResolution);
            _currentResolution = Screen.currentResolution;
            Screen.SetResolution(_resolutionsArray[selectedResolution].width, _resolutionsArray[selectedResolution].height,
                                    _fullScreen, _resolutionsArray[selectedResolution].refreshRate);
            
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
            _resolutionDropDown.SetValueWithoutNotify(_lastIndex);
            _resolutionDropDown.RefreshShownValue();
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
    }
}