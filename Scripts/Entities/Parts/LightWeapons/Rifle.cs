using Datas;
using UnityEngine;
using Managers;

namespace Entities.Parts.LightWeapons
{
    public abstract class Rifle : LightWeaponPart
    {
        
         private ParticleSystem _particleSystem;
         [SerializeField] private GameObject _muzzleFlash;
        
        [SerializeField] private SoundDefinition _laserGunOS;
        private AudioSource _myAudioSource;
         
        protected override void Awake()
        {
            base.Awake();
             
            _particleSystem = _shootPoint.GetComponent<ParticleSystem>();
            
             _myAudioSource = GetComponent<AudioSource>();
            
            if(_particleSystem != null)
            {
                ParticleSystem.MainModule main = _particleSystem.main;
                ParticleSystem.ShapeModule shape = _particleSystem.shape;

                main.startSpeed = _speed * _currentMultiplier;
                main.startLifetime = _range / _speed;
                shape.angle = _spreadAngle / _currentMultiplier;
            }
        }

        #region Weapon

        protected override void Shoot()
        {
                _particleSystem.Emit(1);
                _laserGunOS.PlayAsSFX(_myAudioSource,0);
                _muzzleFlash.SetActive(true);

                Invoke(nameof(MuzzleFlashEffect),0.02f);
        }

        private void MuzzleFlashEffect()
        {
            _muzzleFlash.SetActive(false);
        }
        
        #endregion

        #region Part

        protected override void StopActiveAbility()
        {
            _particleSystem.Stop();
            base.StopActiveAbility();
        }

        #endregion
    }
}