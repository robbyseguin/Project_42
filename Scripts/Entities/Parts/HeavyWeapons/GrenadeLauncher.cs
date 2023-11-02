using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Entities.Parts.Animations;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Entities.Parts.HeavyWeapons
{
    public class GrenadeLauncher : HeavyWeaponPart
    {
        public override PartIdentifier PartID => PartIdentifier.HEAVY_WEAPON_PART_GRENADINE_LAUNCHER;
        public override string Name => GetLocalizedString("Lance grenades", "Grenade launcher");
        public override string Description => GetLocalizedString("Dévastateur, mais très lent", "Destructive, but slow");

        [Header("Grenade Stats")]
        [SerializeField] private GameObject _grenadePrefab;
        [SerializeField] private GameObject _explosionRadiusPrefab;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private LayerMask _damageableLayer;
        [SerializeField] private AnimationCurve _arc;

        private AHGrenadeLauncher _grenadeLauncherAnimation;
        private Transform _grenadesContainer;
        private Camera _camera;
        private GameObject _explosion;
        private Vector3 _target;
        
        private readonly List<GameObject> _grenades = new ();
        private static Transform _explosionRadiusPool;

        // SO Shoot
        private AudioSource _grenadeAudioSource;
        [SerializeField] private SoundDefinition _soundList;
        private AudioClip[] _soundsArray;
    
        private int MaxNbrOfGrenades => 10;
        private float ExplosionRadius => _explosionRadius * _currentMultiplier;

        public override string[] Info => new[]
        {
            base.Info[0] +
            GetLocalizedString("Rayon d'explosion", "Explosion Radius") + "\n",
            base.Info[1] +
            ExplosionRadius.ToString("F1") + "\n"
        };

        protected override void Awake()
        {
            _grenadeAudioSource = gameObject.GetComponent<AudioSource>();
            _soundsArray = _soundList.GetSounds();

            _camera = Camera.main;
            CreateGrenadePool();
            
            base.Awake();
        }

        private void OnDestroy()
        {
            if(_explosion != null)
                Destroy(_explosion);
        }

        private void OnDisable()
        {
            if(_explosion != null)
                _explosion.SetActive(false);
        }

        #region Grenades

        private IEnumerator LaunchGrenade(GameObject grenade, Vector3 startPos, Vector3 target, float startTime)
        {
            _soundList.PlayOneSFX(_grenadeAudioSource, 0);
            _grenadeLauncherAnimation.AnimateAbility();
            
            _arc.keys[0].value = startPos.y;
            _arc.keys[1].value = target.y;

            _explosion.SetActive(true);
            _explosion.transform.position = target;
            _explosion.transform.localScale = new Vector3(ExplosionRadius * 2, ExplosionRadius * 2, ExplosionRadius * 2);
           
            float t = 0;
            while (t < 1f)
            {
                t = (Time.time - startTime) * _speed;
                Vector3 newPos = Vector3.Lerp(startPos, target, t);
                newPos.y = _arc.Evaluate(t);

                grenade.transform.position = newPos;
                grenade.transform.localRotation = Quaternion.identity;
                
                yield return null;
            }
            
            _explosion.SetActive(false);

            Explode(grenade);
        }

        private void Explode(GameObject grenade)
        {
            var colliders = Physics.OverlapSphere(grenade.transform.position, ExplosionRadius, _damageableLayer);
            
            foreach (var collider in colliders)
            {
                if (!collider.TryGetComponent(out IDamageable damageable))
                    continue;
            
                if((damageable as Entity)?.IsPlayer == IsPlayer)
                    continue;
            
                damageable.TakeDamage(Mathf.CeilToInt(Damage));
            }
            AudioSource.PlayClipAtPoint(_soundsArray[1], grenade.transform.position);
            grenade.SetActive(false);
            grenade.transform.localPosition = Vector3.zero;
        }

        private bool FindTarget()
        {
            if(!IsPlayer)
            {
                _target = Entity.Player.transform.position;
                return Vector3.Distance(_target, transform.position) <= _range;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue) &&
                NavMesh.SamplePosition(hit.point, out NavMeshHit target, ExplosionRadius, NavMesh.AllAreas))
            {
                _target = target.position;
            }
            else
            {
                return false;
            }
            
            return Vector3.Distance(_target, transform.position) <= _range;
        }

        private void CreateGrenadePool()
        {
            _grenadesContainer ??= SceneUtility.CreateGameobjectFolder("Grenades", transform);
            _grenadesContainer.localPosition = Vector3.zero;
            _grenadesContainer.localScale = Vector3.one;
            for (int i = 0; i < MaxNbrOfGrenades; i++)
            {
                _grenades.Add(Instantiate(_grenadePrefab, _grenadesContainer));
                _grenades[i].SetActive(false);
            }

            if (_explosionRadiusPool == null)
                _explosionRadiusPool = SceneUtility.CreateGameobjectFolder("ExplosionsRadius_Container");
            
            _explosion ??= Instantiate(_explosionRadiusPrefab, _explosionRadiusPool);
            _explosion.SetActive(false);
        }

        #endregion
        
        #region Weapon

        protected override void Shoot()
        {
            if (!FindTarget()) return;

            foreach (var grenade in _grenades.Where(t => !t.activeSelf))
            {
                grenade.SetActive(true);
                        
                StartCoroutine(LaunchGrenade(grenade, _shootPoint.position, _target, Time.time));

                break;
            }
        }

        protected override IEnumerator Shooting()
        {
            Shoot();
            yield return null;
        }

        #endregion
    
        #region Part
        
        public override void OnEquip(Entity entity, Transform mount)
        {
            base.OnEquip(entity, mount);
            
            _grenadeLauncherAnimation = _levels[_currentLevel].GetComponent<AHGrenadeLauncher>();
        }
        
        #endregion
    }
}