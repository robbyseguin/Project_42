using Managers;
using Managers.Events;

namespace Entities.Parts.HeavyWeapons
{
    public abstract class HeavyWeaponPart : WeaponPart
    {
        public override float Delay => FireDelay;
        
        protected override void PublishActivation(int partEvent)
        {
            this.Publish(Delay, partEvent);
        }
    }
}