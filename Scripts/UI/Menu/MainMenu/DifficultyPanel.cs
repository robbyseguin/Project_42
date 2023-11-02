using Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Menu.MainMenu
{
    public class DifficultyPanel : MonoBehaviour
    {
        [SerializeField] private Image _imagePreview;
        [SerializeField] private TextMeshProUGUI _text;
        
        private Toggle _toggle;
        private Difficulty _currentDifficulty;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(SetLevelID);
        }

        public void SetUp(Difficulty chosenDifficulty, ToggleGroup group)
        {
            _currentDifficulty = chosenDifficulty;
            _imagePreview.sprite = chosenDifficulty.Icon;

            LocalizationManager.Instance.AddLocalizedString(_text, _currentDifficulty.NameFr, _currentDifficulty.NameEn);
            
            _toggle.group = group;
            group.RegisterToggle(_toggle);
            if (chosenDifficulty.name == "Normal")
            {
                _toggle.isOn = true;
            }
        }

        private void SetLevelID(bool value)
        {
            if (_toggle.isOn)
            {
                DifficultyManager.CurrentDifficulty = _currentDifficulty;
                _toggle.interactable = false;
            }
            else
            {
                _toggle.interactable = true;
            }
        }
    }
}