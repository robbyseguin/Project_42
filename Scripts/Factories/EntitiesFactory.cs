using Datas;
using Entities;
using Entities.Parts;
using Managers;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace Factories
{
    public class EntitiesFactory
    {
        private const int PoolSize = 20;
        
        private static EntitiesFactory s_instance;
        public static EntitiesFactory Instance => s_instance ??= new EntitiesFactory();
        
        private Random _randomEngine;
        private EntitiesData _entitiesData => WorldManager.Instance.EntitiesData;
        
        private EntitiesFactory()
        {
            _randomEngine = new Random(WorldManager.Seed);
        }

        public Entity GetPlayerEntity(Transform parent = null, PartIdentifier partIdentifier = 0)
        {
            Entity e = GameObject.Instantiate(_entitiesData.PlayerPrefab, parent,false);
            e.Assemble(partIdentifier);

            return e;
        }

        public Entity GetAiEntity(Transform parent = null, PartIdentifier partIdentifier = 0)
        {
            Entity e = GameObject.Instantiate(_entitiesData.AiPrefab, parent,false);
            e.Assemble(partIdentifier);

            return e;
        }
        
        public Entity GetDummyEntity(Transform parent = null, PartIdentifier partIdentifier = 0)
        {
            Entity e = GameObject.Instantiate(_entitiesData.DummyPrefab, parent,false);
            e.Assemble(partIdentifier);

            return e;
        }
    }
}