using Entities.Parts;
using Levels.Interactable;
using Managers;
using UnityEngine;
using Utilities;
using Utility;
using Random = System.Random;

namespace Factories
{
    public class LootsFactory
    {
        private const int PoolSize = 20;
        
        private static LootsFactory s_instance;
        public static LootsFactory Instance => s_instance ??= new LootsFactory();
        private Random _randomEngine;
        
        private PowerUp[] LootTable => WorldManager.Instance.LootData.LootTable;
        private GenericPool<PowerUp>[] _monoBehaviourPoolArray;
        
        private PartsFactory _partsFactory => PartsFactory.Instance;

        private LootsFactory()
        {
            _randomEngine = new Random(WorldManager.Seed);
            
            _monoBehaviourPoolArray = new GenericPool<PowerUp>[LootTable.Length];
            for (int i = 0; i < LootTable.Length; i++)
                _monoBehaviourPoolArray[i] = new GenericPool<PowerUp>(LootTable[i], PoolSize);
        }
        
        public GameObject GetLoot()
        {
            GameObject go = _randomEngine.Next(4) == 0? 
                _monoBehaviourPoolArray.RandomIndex(_randomEngine).GetObject().gameObject:
                _partsFactory.GetPart(PartIdentifier.POOL_LOOT).gameObject;
            
            return go;
        }
    }
}