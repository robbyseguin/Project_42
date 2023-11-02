using Entities;
using Levels.Interactable;
using Managers.Events;
using UnityEngine;
using Utilities;

public enum GameState 
{
    STARTING = 0,
    PLAYING,
    PAUSED,
    GAMEOVER
}

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Texture _particleTextTexture;
        public delegate void OnStateChangeHandler(GameState state);
        public event OnStateChangeHandler _onGameStateChanged;

        public GameState State { get; private set; }
        public int Score { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (!IsSingleton) 
                return;

            TextParticleHelper._texture = _particleTextTexture;
            
            EventsManager.Subscribe<Entity>(UpdateScoreOnAiDeath, EntityEvent.AI_DEATH);
            EventsManager.Subscribe<WorldManager>(UpdateScoreOnRoomChange, WorldEvents.NEW_SECTION);
            EventsManager.Subscribe<WorldManager>(UpdateScoreOnNewLevel, WorldEvents.NEW_WORLD);
            EventsManager.Subscribe<PointDestructibleObject>(UpdateScoreDestroyedObject);
        }
     
        private void UpdateScoreDestroyedObject(EventsDictionary<PointDestructibleObject>.CallbackContext ctx)
        {
            Score += ctx.ReadValue<int>();
            this.Publish(Score,0);
        }

        private void UpdateScoreOnNewLevel(EventsDictionary<WorldManager>.CallbackContext ctx)
        {
            Score = 0;
            this.Publish(Score,0);
        }

        private void UpdateScoreOnRoomChange(EventsDictionary<WorldManager>.CallbackContext ctx)
        {
            Score += ctx.ReadValue<int>();
            this.Publish(Score,0);
        }

        private void UpdateScoreOnAiDeath(EventsDictionary<Entity>.CallbackContext ctx)
        {
            Score += ctx.ReadValue<int>();
            this.Publish(Score, 0);
        }

        public void SetGameState(GameState state)
        {
            State = state;
            _onGameStateChanged(state);
        }
    }
}