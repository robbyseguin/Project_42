using UnityEngine;

namespace UI.Menu.MainMenu
{
    public class MainMenu : Menu
    {
        public void QuitApplication()
        {
            SaveManager.SaveGame();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
