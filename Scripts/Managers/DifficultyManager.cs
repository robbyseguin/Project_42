using Managers.Events;
using UI.Menu.MainMenu;
using UnityEngine;

namespace Managers
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        [SerializeField] private float _difficultyIncrease = 0.05f;
        [SerializeField] private float _difficultyFrequency = 5;
        [SerializeField] private Difficulty _defaultDifficulty;
        
        private float _difficultyMultiplier;
        private int _nbrOfSections;

        private float BaseModifier => _difficultyMultiplier * Random.Range(0.8f, 1.2f);
        private static float DayNightModifier {
            get
            {
                float modifier;
                float time = WorldManager.Instance.TimeOfDay;
                if (time is < 18 and > 8)
                {
                    modifier = 0.5f;
                }
                else
                {
                    modifier = 1;
                }

                return modifier;
            }
        }

        public float EnemyMultiplier =>  0.2f + _difficultyMultiplier * DayNightModifier;

        public float PartMultiplier => 1 + BaseModifier;
        public float SpawnerMultiplier => 1 + BaseModifier * 0.5f;
        public float HealthMultiplier => 1 + BaseModifier;

        public static Difficulty CurrentDifficulty;
        
        protected override void Awake()
        {
            base.Awake();
            
            if (!IsSingleton) 
                return;

            EventsManager.Subscribe<WorldManager>(AdjustDifficulty);

            if (CurrentDifficulty == null)
                CurrentDifficulty = _defaultDifficulty;
            
            _difficultyIncrease *= CurrentDifficulty.Multiplier;
        }

        private void AdjustDifficulty(EventsDictionary<WorldManager>.CallbackContext ctx)
        {
            switch (ctx.EventID)
            {
                case WorldEvents.NEW_WORLD:
                    OnNewWorld();
                    break;
                
                case WorldEvents.NEW_SECTION:
                    OnNewSection();
                    break;
            }
        }

        private void OnNewWorld()
        {
            _nbrOfSections = 0;
            _difficultyMultiplier = 0;
        }

        private void OnNewSection()
        {
            ++_nbrOfSections;
            _difficultyMultiplier = (_nbrOfSections / _difficultyFrequency) * _difficultyIncrease;
        }
    }
}