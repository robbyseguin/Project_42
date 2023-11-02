using Entities.Controllers;
using UnityEditor;
using UnityEngine;

namespace Entities.Parts.Heads
{
    public class TransferHeadPart : HeadPart
    {
        [SerializeField] private LayerMask _entityLayer;
        
        public override PartIdentifier PartID => PartIdentifier.HEAD_PART_SWITCH;
        public override string Name => GetLocalizedString("Tête d'échange de corps", "Transfert head part");
        public override string Description => GetLocalizedString("Échange de corps avec tes ennemis avec Q", 
            "Switch body with an enemy with Q");

        protected override void StartActiveAbility()
        {
            Ray ray = new Ray(_transform.position, _transform.forward);

            if(!Physics.SphereCast(ray,2.0f,out RaycastHit hit,Range, _entityLayer))
                return;

            Entity otherEntity = hit.transform.GetComponent<Entity>();

            SwitchBrain(otherEntity);
        }

        private void SwitchBrain(Entity otherEntity)
        {
            Transform parent = _entity.transform.parent;
            
            _entity.transform.SetParent(otherEntity.transform.parent);
            otherEntity.transform.SetParent(parent);

            Controller brain = otherEntity.Brain;
            otherEntity.SetBrain(_entity.Brain);
            _entity.SetBrain(brain);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!Application.isPlaying || !enabled) return;

            Handles.color = Color.yellow;
            Handles.DrawLine(_transform.position, _transform.position + (_transform.forward * _range));
        }
#endif
    }
}
