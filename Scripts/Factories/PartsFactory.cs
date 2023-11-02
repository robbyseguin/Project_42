using System;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Entities.Parts;
using Entities.Parts.Cockpits;
using Entities.Parts.Heads;
using Entities.Parts.HeavyWeapons;
using Entities.Parts.LightWeapons;
using Entities.Parts.Movements;
using Managers;
using Utilities;
using Utility;
using Random = System.Random;

namespace Factories
{
    public class PartsFactory
    {
        private const int PoolSize = 20;
        
        private static PartsFactory s_instance;
        public static PartsFactory Instance => s_instance ??= new PartsFactory();
        
        private Random _randomEngine;
        private GenericPool<MovementPart>[] _movementPartPoolArray;
        private GenericPool<CockpitPart>[] _cockpitPartPoolArray;
        private GenericPool<HeadPart>[] _headPartPoolArray;
        private GenericPool<LightWeaponPart>[] _lightWeaponPartPoolArray;
        private GenericPool<HeavyWeaponPart>[] _heavyWeaponPartPoolArray;

        private Dictionary<PartIdentifier, PartInformation> _partLookUp = new Dictionary<PartIdentifier, PartInformation>();

        private PartData _partData => WorldManager.Instance.PartData;
        
        private PartsFactory()
        {
            _randomEngine = new Random(WorldManager.Seed);

            _movementPartPoolArray = BuildPool(_partData.MovementParts);
            _cockpitPartPoolArray = BuildPool(_partData.CockpitParts);
            _headPartPoolArray = BuildPool(_partData.HeadParts);
            _lightWeaponPartPoolArray = BuildPool(_partData.LightWeaponParts);
            _heavyWeaponPartPoolArray = BuildPool(_partData.HeavyWeaponParts);
        }

        private GenericPool<T>[] BuildPool<T>(T[] prefabs) where T : Part
        {
            GenericPool<T>[] pools = new GenericPool<T>[prefabs.Length];
            
            for (int i = 0; i < prefabs.Length; i++)
            {
                pools[i] = new GenericPool<T>(prefabs[i], PoolSize);
                _partLookUp.Add(prefabs[i].PartID,new PartInformation(pools[i],prefabs[i]));
            }

            return pools;
        }

        public Part GetPart(PartIdentifier partIdentifier)
        {
            List<GenericPool> pools = new List<GenericPool>();

            foreach (KeyValuePair<PartIdentifier,PartInformation> pair in _partLookUp)
                if ((pair.Key & partIdentifier) != 0)
                    pools.Add(pair.Value._pool);
            
            return pools.RandomIndex(_randomEngine).GetObject<Part>();
        }

        public Part[] GetPartInformation(PartIdentifier partIdentifier)
        {
            List<Part> parts = new List<Part>();

            foreach (KeyValuePair<PartIdentifier,PartInformation> pair in _partLookUp)
                if ((pair.Key & partIdentifier) != 0)
                    parts.Add(pair.Value._prefab);

            return parts.ToArray();
        }
        
        private struct PartInformation
        {
            public GenericPool _pool;
            public Part _prefab;

            public PartInformation(GenericPool pool, Part prefab)
            {
                _pool = pool;
                _prefab = prefab;
            }
        }

        public static PartIdentifier GetPart()
        {
            return Instance._partLookUp.Keys.ToArray().RandomIndex();;
        }
    }
}