using System.Collections.Generic;
using Entities;
using Entities.Parts;
using Factories;
using Unity.VisualScripting;
using UnityEngine;

namespace SectionComponents
{
    public class AiEntitySpawner : MonoBehaviour
    {
        [SerializeField] private PartIdentifier _partIdentifier;

        private List<Entity> _entitiesList = new List<Entity>();
        private EntitiesFactory _entitiesFactory => EntitiesFactory.Instance;
        
        public void SpawnEntity(PartIdentifier partIdentifier = PartIdentifier.POOL_ENEMY)
        {
            if (_partIdentifier != 0)
                partIdentifier = _partIdentifier;
            
            _entitiesList.Add(_entitiesFactory.GetAiEntity(transform, partIdentifier));
        }

        public void KillRemaining()
        {
            foreach (Entity go in _entitiesList)
                if(!go.IsDestroyed())
                {
                    go.PartsHandler.Disassemble();
                    Destroy(go.gameObject);
                }
        }
    }
}