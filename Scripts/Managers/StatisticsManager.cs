using Entities;
using Managers.Events;

namespace Managers
{
    public class StatisticsManager : Singleton<StatisticsManager>
    {
        public enum GameStatistic
        {
            PLAYER_DEATH = 0,
            AI_KILLED,
            ROOM_TRAVERSED,
            NUMBER_OF_GAME,
            HIGH_SCORE,
            MAXIMUM
        }

        public int[] _sessionStatistics { get; set; }

        protected override void Awake()
        {
            base.Awake();

            if (!IsSingleton) 
                return;

            _sessionStatistics = new int[(int)GameStatistic.MAXIMUM];
            
            // Entities statistics
            EventsManager.Subscribe<Entity>(IncrementEntitiesStatistics);
            
            // Level Statistics
            EventsManager.Subscribe<WorldManager>(IncrementLevelStatistics);
            
            EventsManager.Subscribe<GameManager>(UpdateGameManagerStatistic);
        }

        private void UpdateGameManagerStatistic(EventsDictionary<GameManager>.CallbackContext ctx)
        {
            if (_sessionStatistics[(int)GameStatistic.HIGH_SCORE] < ctx.ReadValue<int>())
                _sessionStatistics[(int)GameStatistic.HIGH_SCORE] = ctx.ReadValue<int>();
        }

        private void IncrementLevelStatistics(EventsDictionary<WorldManager>.CallbackContext ctx)
        {
            switch (ctx.EventID)
            {
                case WorldEvents.NEW_WORLD:
                    _sessionStatistics[(int)GameStatistic.NUMBER_OF_GAME]++;
                    break;
                
                case WorldEvents.NEW_SECTION:
                    _sessionStatistics[(int)GameStatistic.ROOM_TRAVERSED]++;
                    break;
            }
        }

        private void IncrementEntitiesStatistics(EventsDictionary<Entity>.CallbackContext ctx)
        {
            switch (ctx.EventID)
            {
                case EntityEvent.PLAYER_DEATH:
                    _sessionStatistics[(int)GameStatistic.PLAYER_DEATH]++;
                    break;
                case EntityEvent.AI_DEATH:
                    _sessionStatistics[(int)GameStatistic.AI_KILLED]++;
                    break;
            }
        }
    }
}