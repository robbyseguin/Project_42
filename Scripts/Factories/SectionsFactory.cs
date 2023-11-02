using System.Collections.Generic;
using Datas;
using Levels.Sections;
using Managers;
using UnityEngine;
using Utilities;
using Utility;
using Random = System.Random;

namespace Factories
{
    public class SectionsFactory
    {
        private const int PoolSize = 5;
        
        private static SectionsFactory s_instance;
        public static SectionsFactory Instance => s_instance ??= new SectionsFactory();
        
        private WorldManager WM => WorldManager.Instance;
        private SectionsData WMS => WM.SectionsData;
        private int AtRoom => WM.AtRoom;

        private GenericPool<Section>[] _startingSectionsPoolArray;
        private GenericPool<Section>[] _normalSectionsPoolArray;
        private GenericPool<Section>[] _specialSectionsPoolArray;
        
        private Transform _rootFolder;
        private Transform _poolsFolder;
        private Random _randomEngine;
        private List<SectionSlot> _slot = new List<SectionSlot>();
        
        private SectionsFactory()
        {
            _randomEngine = new Random(WorldManager.Seed);
            _rootFolder = new GameObject("Sections").transform;

            _startingSectionsPoolArray = BuildPool(WMS.StartingSection);
            _normalSectionsPoolArray = BuildPool(WMS.NormalSections);
            _specialSectionsPoolArray = BuildPool(WMS.SpecialSections);
        }

        public void ResetFactory()
        {
            _slot = new List<SectionSlot>();
            _randomEngine = new Random(WorldManager.Seed);
        }
        
        private GenericPool<T>[] BuildPool<T>(T[] prefabs) where T : Section
        {
            GenericPool<T>[] pools = new GenericPool<T>[prefabs.Length];
            
            for (int i = 0; i < prefabs.Length; i++)
                pools[i] = new GenericPool<T>(prefabs[i], PoolSize);

            return pools;
        }

        public Section GetSection()
        {
            GenericPool<Section> pool;
            
            if (AtRoom == 0)
                pool = _startingSectionsPoolArray.RandomIndex(_randomEngine);
            else if (AtRoom % WMS.SpecialSectionInterval == 0)
                pool = _specialSectionsPoolArray.RandomIndex(_randomEngine);
            else
                pool = _normalSectionsPoolArray.RandomIndex(_randomEngine);
            
            Section section = pool.GetObject();
            section.transform.SetParent(_rootFolder);
            
            SectionSlot slot = SelectSlot();
            
            slot._section = section;
            section.transform.position = slot._transform;
            section.InitializeSection();

            return section;
        }

        private SectionSlot SelectSlot()
        {
            SectionSlot slot = _slot.Find(slot => slot._isAvailable);

            if (slot == default)
            {
                slot = new SectionSlot();
                
                Vector3 nextSlotPosition = Vector3.forward * 
                                           (WMS.SectionsMaximumSize.y * (_slot.Count / WMS.SpecialSectionInterval));
                nextSlotPosition += Vector3.right * 
                                    (WMS.SectionsMaximumSize.x * (_slot.Count % WMS.SpecialSectionInterval));

                slot._transform = nextSlotPosition;
                _slot.Add(slot);
            }

            return slot;
        }

        private class SectionSlot
        {
            public bool _isAvailable => !_section.gameObject.activeInHierarchy;
            public Vector3 _transform;
            public Section _section;
        }
    }
}