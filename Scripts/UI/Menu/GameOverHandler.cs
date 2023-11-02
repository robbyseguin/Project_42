using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers.Events;
using Entities;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class GameOverHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text _gameOverScoreValue;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private TMP_Text _usernameText;

        public void ClickSound() => AudioManager.Instance.PlayUI(0);
        public void HoverSound() => AudioManager.Instance.PlayUI(1);

        private void Awake()
        {           
            _restartButton.onClick.AddListener(RestartGame);
            _mainMenuButton.onClick.AddListener(BackToMainMenu);
            EventsManager.Subscribe<Entity>(GameOver, EntityEvent.PLAYER_DEATH);
            
            gameObject.SetActive(false);
        }
        
        private void RestartGame()
        {
            SaveManager.SaveGame(_usernameText.text);
            WorldManager.ResetWorld();
            Time.timeScale = 1;
            Cursor.visible = false;
            gameObject.SetActive(false);

        }
        
        private void BackToMainMenu()
        {
            Time.timeScale = 1;
            SaveManager.SaveGame(_usernameText.text);
            SceneManager.LoadScene(0);
        }
        
        public void GameOver(EventsDictionary<Entity>.CallbackContext ctx)
        {
            Cursor.visible = true;
            _gameOverScoreValue.text = GameManager.Instance.Score.ToString();
            gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
