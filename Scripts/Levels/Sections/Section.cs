using Entities;
using Entities.Parts;
using Managers;
using UnityEngine;
using Utilities;
using Utility;
using Factories;
using Random = System.Random;

namespace Levels.Sections
{
    public class Section : MonoBehaviour, IPoolable
    {
        [SerializeField, HideInInspector] private Portal[] _entries;
        [SerializeField, HideInInspector] private Portal[] _exits;
        [SerializeField, HideInInspector] private SectionComponent[] _sectionComponents;
        [SerializeField, Range(0, 24)] private float _timeOfDay;
        [SerializeField] private bool _canRotate = true;
        [SerializeField] public Restriction _restriction;

        private Portal _selectedEntryPortal;
        private Random _randomEngine;

        public int Score => (int)(100 * DifficultyManager.Instance.EnemyMultiplier * (_restriction.IllegalPartAmount()+1) / _restriction.maxHealth);
        public Portal[] Entries => _entries;
        public Portal[] Exits => _exits;
        public SectionComponent[] SectionComponents => _sectionComponents;
        public Portal EntryPortal => _selectedEntryPortal;

        public void InitializeSection()
        {
            _randomEngine = new System.Random(WorldManager.Seed + WorldManager.Instance.AtRoom);
            
            if(_canRotate)
                transform.rotation = Quaternion.Euler(0,_randomEngine.Next(4) * 90,0);

            foreach (Portal portal in _entries)
                portal.gameObject.SetActive(false);
            
            _selectedEntryPortal = _entries.RandomIndex();
            _selectedEntryPortal.gameObject.SetActive(true);
            
            foreach (SectionComponent sectionComponent in _sectionComponents)
                sectionComponent.OnSectionInitialisation();
        }

        public void StartSection()
        {
            WorldManager.Instance.TimeOfDay = _timeOfDay;
            
            foreach (SectionComponent sectionComponent in _sectionComponents)
                sectionComponent.OnSectionStart();
        }

        public void ResetSection()
        {
            foreach (SectionComponent sectionComponent in _sectionComponents)
                sectionComponent.OnSectionReset();
        }
        
        public void BakeSection()
        {
            Transform entries = transform.Find("Entries");
            Transform exits = transform.Find("Exits");
            Transform environment = transform.Find("Environment");

            _entries = entries.GetComponentsInChildren<Portal>();
            _exits = exits.GetComponentsInChildren<Portal>();
            _sectionComponents = environment.GetComponentsInChildren<SectionComponent>();

            foreach (SectionComponent component in _sectionComponents)
                component.OnSectionBake();
        }
        
        [System.Serializable]
        public class Restriction
        {
            [Range(0,1)]public float maxHealth = 1.0f;
            [SerializeField] private PartIdentifier illegalParts = 0;
            public PartIdentifier IllegalParts => illegalParts == 0 ? 
                (PartsFactory.GetPart() | PartsFactory.GetPart()) & ~PartIdentifier.GROUP_DEFAULT : 
                illegalParts;

            public int IllegalPartAmount()
            {
                int part = (int) illegalParts;
                int count = 0;
                
                while (part > 0)
                {
                    if (part % 2 != 0)
                        count++;
                    
                    part /= 2;
                }

                return count;
            }
            
            public bool TestEntity(Entity e, PartIdentifier pe = 0)
            {
                PartsHandler partsHandler = e.PartsHandler;
                
                partsHandler.ResetPartToDefault(pe);
                
                return true;
            }
        }
    }
}