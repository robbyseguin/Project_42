using Managers;
using Managers.Events;

namespace Entities.Parts.LightWeapons
{
    public abstract class LightWeaponPart : WeaponPart
    {
        public override float Delay => -1;

        protected override void PublishActivation(int partEvent)
        {
            this.Publish(Delay, partEvent);
        }
    }
}