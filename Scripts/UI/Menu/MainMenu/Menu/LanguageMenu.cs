using Managers;

namespace UI.Menu.MainMenu
{
    public class LanguageMenu : Menu
    {
        public void ChangeLocale(int localeID)
        {
            LocalizationManager.Instance.ChangeLocale(localeID);
        }
    }
}