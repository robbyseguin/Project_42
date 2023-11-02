using System.Collections;
using Managers;
using UnityEngine;

namespace Entities.Parts
{
    public abstract class WeaponPart : Part
    {
        [Header("Weapon Stats")]
        [SerializeField] protected float _damage;
        [SerializeField] protected float _fireRate;
        [SerializeField] protected float _range;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _spreadAngle;
        [SerializeField] protected Transform _shootPoint;

        protected Coroutine _shooting;
        protected float _timeOfLastShot;
        
        public override string[] Info => new []
        {
            "\n" +GetLocalizedString("Délai","Delay") + "\n" +
            GetLocalizedString("Dommage","Damage") + "\n " +
            GetLocalizedString("Fréquence de tir","FireRate") + "\n " +
            GetLocalizedString("Distance","Range") + "\n " +
            GetLocalizedString("Vitesse du projectile","BulletSpeed") + "\n" +
            GetLocalizedString("Précision","Spread Angle") + "\n",
            "\n" +
            (Delay == -1 ? 0 : Delay.ToString("F1")) + "\n" +
            _damage.ToString("F1") + "\n" +
            _fireRate.ToString("F1") +  "\n" +
            _range.ToString("F1") +  "\n" +
            _speed.ToString("F1") + "\n" +
            _spreadAngle.ToString("F1") + "\n"
        };

        public virtual float Damage
        {
            get
            {
                if (IsPlayer)
                    return _damage * _currentMultiplier;
                
                return _damage * _currentMultiplier * DifficultyManager.Instance.EnemyMultiplier;
            }
            private set => _damage = value;
        }
        protected float FireRate
        {
            get
            {
                if (_entity == null || IsPlayer)
                    return _fireRate * _currentMultiplier;
                
                return _fireRate * _currentMultiplier * DifficultyManager.Instance.EnemyMultiplier;
            }
            private set => _fireRate = value;
        }
        
        protected float FireDelay => 1f / FireRate;
        protected bool DelayPassed() => _timeOfLastShot + FireDelay < Time.time;
        private float Dps => _damage * _fireRate * _currentMultiplier;

        #region Weapon

        public void AimAt(Vector3 target)
        {
            _transform.forward = target - transform.position;
        }

        public override void OnEquip(Entity entity, Transform mount)
        {
            foreach (var t in _powerUpThreshold)
            {
                if (Dps >= t)
                {
                    _currentLevel++;
                }
            }

            base.OnEquip(entity, mount);
        }
        
        protected virtual IEnumerator Shooting()
        {
            while(enabled)
            {
                yield return new WaitUntil(DelayPassed);
                
                Shoot();
                _timeOfLastShot = Time.time;
                
                if(Delay > 0.2f && _entity.IsPlayer) 
                    PublishActivation(PartsEvents.ACTIVATED);
            }
        }

        protected abstract void Shoot();

        #endregion

        #region Part
        
        protected override void StartActiveAbility()
        {
            _shooting ??= StartCoroutine(Shooting());
        }

        protected override void StopActiveAbility()
        {
            if (_shooting == null) 
                return;
            
            StopCoroutine(_shooting);
            _shooting = null;
        }
        
        #endregion
    }
}