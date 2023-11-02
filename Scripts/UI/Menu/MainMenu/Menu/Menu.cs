using UnityEngine;

namespace UI.Menu.MainMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Menu : MonoBehaviour
    {
        public Menu Hide()
        {
            gameObject.SetActive(false);

            return this;
        }

        public Menu Show()
        {
            gameObject.SetActive(true);

            return this;
        }
    }
}
