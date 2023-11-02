using Managers.Events;
using UnityEngine;

namespace Entities.Parts.Cockpits
{
    public abstract class CockpitPart : Part
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private Transform _headMount;
        [SerializeField] private Transform _lightWeaponMount;
        [SerializeField] private Transform _heavyWeaponMount;

        public override string[] Info => new []
        {
            "\n" + GetLocalizedString("Vie", "Health") + "\n",
            "\n" + MaxHealth + "\n"
        };

        public int MaxHealth => (int)(_maxHealth * _currentMultiplier);
        public int Score => MaxHealth;

        public Transform HeadMount => _headMount;
        public Transform LightWeaponMount => _lightWeaponMount;
        public Transform HeavyWeaponMount => _heavyWeaponMount;

        public virtual int OnDamage(int hitPoint)
        {
            return hitPoint;
        }

        protected override void StartActiveAbility() { }
        
        public void AimAt(Vector3 target)
        {
            Vector3 position = _transform.position;
            
            target.y = position.y;
            
            if(target - position != Vector3.zero)
                _transform.forward = target - position;
        }

        protected override void PublishActivation(int partEvent)
        {
            base.PublishActivation(partEvent);
            
            this.Publish(partEvent);
        }
    }
}
