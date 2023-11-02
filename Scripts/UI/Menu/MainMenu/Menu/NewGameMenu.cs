using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace UI.Menu.MainMenu
{
    public class NewGameMenu : Menu
    {
        [SerializeField] private GameObject _prefabLevel;
        [SerializeField] private Transform _levelContent;
        [SerializeField] private SceneHandler _loadingScreen;
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private Difficulty[] _difficulties;

        private readonly string _defaultSeed = "Testing";
        
        protected void Awake()
        {
            foreach (var difficulty in _difficulties)
            {
                DifficultyPanel levelPanel = Instantiate(_prefabLevel, _levelContent).GetComponent<DifficultyPanel>();
                
                levelPanel.SetUp(difficulty, _toggleGroup);
            }
        }

        public void OnSeedChange(string seed)
        {
            WorldManager.Instance.SetSeed(seed == "" ? _defaultSeed : seed);
        }

        public void OnNewGameButtonPressed()
        {
            _loadingScreen.LoadMyScene(1);
            Hide();
        }
    }
}
