using Managers;
using Managers.Events;

namespace Levels.Interactable
{
    public class PointDestructibleObject : DestructibleObject
    {
        private int _point => (int)(_maxHealth * DifficultyManager.Instance.HealthMultiplier);

        protected override void OnDeath()
        {
            this.Publish(_point, -1);
            
            base.OnDeath();
        }
    }
}