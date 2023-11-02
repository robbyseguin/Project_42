using Entities;
using Entities.Parts;
using Factories;
using Levels.Sections;
using UnityEngine;

namespace SectionComponents
{
    public class DummyEntitySpawner : SectionComponent
    {
        [SerializeField] private PartIdentifier _partIdentifier;
        private EntitiesFactory _entitiesFactory => EntitiesFactory.Instance;

        private Entity _entity;
        
        public override void OnSectionInitialisation()
        {
            base.OnSectionInitialisation();

            if (_partIdentifier == 0)
                _partIdentifier = PartIdentifier.POOL_DUMMY;
            
            _entity = _entitiesFactory.GetDummyEntity(transform, _partIdentifier);
        }

        public override void OnSectionStart()
        {
            base.OnSectionStart();
        }

        public override void OnSectionReset()
        {
            base.OnSectionReset();
            _entity = GetComponentInChildren<Entity>();
            
            if(!_entity)
                return;
            
            _entity.PartsHandler.Disassemble();
            Destroy(_entity.gameObject);
        }

        public override void OnSectionBake()
        {
            base.OnSectionBake();
        }
    }
}