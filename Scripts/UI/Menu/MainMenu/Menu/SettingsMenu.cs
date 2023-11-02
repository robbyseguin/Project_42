using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu.MainMenu
{
    public class SettingsMenu : Menu
    {
        [SerializeField] private SoundMenu _soundMenu;
        [SerializeField] private LanguageMenu _languageMenu;
        [SerializeField] private GraphicsMenu _graphicsMenu;

        protected void Awake()
        {
            _soundMenu.Show();
        }
        
        public void OpenGraphicsMenu()
        {
            _soundMenu.Hide();
            _languageMenu.Hide();
            _graphicsMenu.Show();
        }

        public void OpenGameplayMenu()
        {
            _soundMenu.Hide();
            _graphicsMenu.Hide();
            _languageMenu.Show();
        }

        public void OpenSoundsMenu()
        {
            _languageMenu.Hide();
            _graphicsMenu.Hide();
            _soundMenu.Show();
        }
    }
}
