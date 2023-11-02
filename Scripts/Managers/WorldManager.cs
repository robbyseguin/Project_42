using System.Collections.Generic;
using Cinemachine;
using Datas;
using Entities;
using Entities.Parts;
using Factories;
using Levels;
using Levels.Sections;
using Managers.Events;
using Unity.Mathematics;
using UnityEngine;
using Utilities;

namespace Managers
{
    public static class WorldEvents
    {
        public const int NEW_SECTION = 0;
        public const int NEW_WORLD = 1;
    }
    
    public class WorldManager : Singleton<WorldManager>
    {
        [Header("Data")]
        [SerializeField] private SectionsData _sectionsData;
        [SerializeField] private WorldSettings _worldSettings;
        [SerializeField] private LootData _lootData;
        [SerializeField] private PartData _partData;
        [SerializeField] private EntitiesData _entitiesData;
        
        [Header("Settings")]
        [SerializeField] private string _seed;
        [SerializeField] private Light _sun;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        
        private float _startTime;
        private float _timeOfDay;
        private Color _sunColor;
        
        private Dictionary<int, Section> _nextSections = new();
        private int _sectionNumber = 0;
        private Section _currentSection = null;

        private Transform _player;
        private UI.Compass _compass;

        private SectionsFactory _sectionsFactory => SectionsFactory.Instance;
        public SectionsData SectionsData => _sectionsData;
        public LootData LootData => _lootData;
        public PartData PartData => _partData;
        public EntitiesData EntitiesData => _entitiesData;
        private float TimeScale => _worldSettings.TimeScale;
        private Color DuskColor => _worldSettings.DuskColor;
        private Color DawnColor => _worldSettings.DawnColor;
        private Vector3 SunAngle => new (_timeOfDay / 24 * 360, 0, 0);
        public static int Seed { get; private set; }
        
        public int AtRoom => Instance._sectionNumber;
        
        public float TimeOfDay
        {
            get
            {
                float time = _timeOfDay + 8;
                
                if (time >= 24)
                    time -= 24;

                return time;
            } 
            
            set
            {
                _timeOfDay = value;
                
                switch (_timeOfDay)
                {
                    case >= 24:
                        _timeOfDay -= 24;
                        break;
                    case >= 8:
                        _timeOfDay -= 8;
                        break;
                    default:
                        _timeOfDay += 16;
                        break;
                }
            } 
        }

        protected override void Awake()
        {
            base.Awake();
            
            if(!IsSingleton)
                return;

            TimeOfDay = _startTime;
            
            _sun ??= FindObjectOfType<Light>();
            _sun.transform.rotation = Quaternion.Euler(SunAngle);
            _sunColor = _sun.color;
            
            EventsManager.Subscribe<Portal>(ChangedSection);

            // QualitySettings.vSyncCount = 0;
            // Application.targetFrameRate = 1000;
        }

        private void FixedUpdate()
        {
            if (TimeScale <= 0)
                return;
            
            DayCycle();
        }

        private void Start() => NewWorld();
        public static void ResetWorld() => Instance.NewWorld();

        public void SetSeed(string seed) => _seed = seed;
        
        #region Sections
        
        private void NewWorld()
        {
            Seed = _seed == "" ? System.DateTime.Now.GetHashCode() : Instance._seed.GetHashCode();
            _sectionsFactory.ResetFactory();
            _currentSection?.ResetSection();
            _currentSection.Destroy();

            foreach (KeyValuePair<int,Section> section in _nextSections)
            {
                section.Value.ResetSection();
                section.Value.Destroy();
            }

            _nextSections.Clear();
            _sectionNumber = 0;
            
            if(_player)
            {
                _player.parent.gameObject.GetComponent<Entity>().PartsHandler.Disassemble();
                Destroy(_player.parent.gameObject);
            }

            SpawnStartingSection();
            SpawnPlayer();
            _currentSection.StartSection();
            AIManager.Instance.InitializeTarget(_player.gameObject);
        }
        
        private void SpawnPlayer()
        {
            Entity p = EntitiesFactory.Instance.GetPlayerEntity(null, PartIdentifier.POOL_PLAYER);
            //Entity p = EntitiesFactory.Instance.GetPlayerEntity(null, PartIdentifier.SET_GOD);
            p.transform.position = _currentSection.EntryPortal.transform.position;

            _player = p.Brain.transform;
            _virtualCamera.Follow = _player;
            _virtualCamera.LookAt = _player;
            
            _compass = _player.GetComponentInChildren<UI.Compass>();
            _compass.UpdateCompassArrow(Instance._currentSection.Exits);
        }
        
        private void SpawnStartingSection()
        {
            this.Publish(_sectionNumber, WorldEvents.NEW_WORLD);

            _currentSection = _sectionsFactory.GetSection();
            ++_sectionNumber;
            SpawnNextSections(_currentSection.Exits);
        }
        
        private void SpawnNextSections(Portal[] portals)
        {
            foreach (Portal portal in portals)
            {
                portal.Destination = _sectionsFactory.GetSection();
                _nextSections.Add(portal.GetInstanceID(), portal.Destination);
            }
        }

        private void ChangedSection(EventsDictionary<Portal>.CallbackContext ctx)
        {
            ++_sectionNumber;
            this.Publish(_currentSection.Score * _sectionNumber, WorldEvents.NEW_SECTION);
            

                _currentSection.ResetSection();
                _currentSection.Destroy();
            
            _currentSection = _nextSections[ctx.Caller.GetInstanceID()];
            _nextSections.Remove(ctx.Caller.GetInstanceID());
            _currentSection.StartSection();

            foreach (KeyValuePair<int,Section> section in _nextSections)
            {
                section.Value.ResetSection();
                section.Value.Destroy();
            }
            
            _nextSections.Clear();
            SpawnNextSections(_currentSection.Exits);
            
            _compass.UpdateCompassArrow(Instance._currentSection.Exits);
        }
        #endregion

        #region DayCycleMethods
        private void DayCycle()
        {
            _timeOfDay += Time.fixedDeltaTime * TimeScale;
            if (_timeOfDay >= 24)
                _timeOfDay = 0;
            
            _sun.transform.rotation = Quaternion.Euler(SunAngle);
            SetSkyColor();
        }

        private void SetSkyColor()
        {
            switch (TimeOfDay)
            {
                case < 10:
                    _sun.color = Color.Lerp(DuskColor, _sunColor, (TimeOfDay - 6) / 4);
                    break;
                case > 16:
                    _sun.color = Color.Lerp(_sunColor, DawnColor, (TimeOfDay - 16) / 4);
                    break;
                default:
                    _sun.color = _sunColor;
                    break;
            }
        }
        #endregion
    }
}