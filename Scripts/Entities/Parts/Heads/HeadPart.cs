using Managers;
using Managers.Events;
using UnityEngine;

namespace Entities.Parts.Heads
{
    public abstract class HeadPart : Part
    {
        [SerializeField] protected float _range;
        [SerializeField] protected float _delay;
        public override float Delay => _delay * _currentMultiplier;
        protected float Range => _range * _currentMultiplier;

        public override string[] Info => new[]
        { 
            "\n" + GetLocalizedString("Délai", "Delay") +"\n" +
            GetLocalizedString("Distance", "Range") + "\n", 
            "\n" + (Delay == -1 ? 0 : Delay.ToString("F1")) + "\n" +
            _range.ToString("F1") + "\n"
            
        };

        protected override void PublishActivation(int partEvent)
        {
            this.Publish(Delay, partEvent);
        }
    }
}