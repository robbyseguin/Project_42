using Datas;
using Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Parts.Movements
{
    public class WarpMovementPart : MovementPart
    {
        public override PartIdentifier PartID => PartIdentifier.MOVEMENT_PART_WARP;
        [SerializeField] private float _range = 3.0f;

        public override float Delay => 1;

        private float Range => _range * _currentMultiplier;
        
        private Vector3 _warpDir;
        private AudioSource _myaudioSource;
        [SerializeField] private SoundDefinition _soundList;
        public override string Name => GetLocalizedString("Jambes de téléportation", "Warp movement part");
        public override string Description => GetLocalizedString("Appuis sur Espace pour se téléporter dans la direction du mouvement", 
            "Each press of Space teleports you a step ahead in the direction you were already going");

        public override string[] Info => new[]
        {
            base.Info[0] + GetLocalizedString("Portée","Range") + "\n",
            base.Info[1] + Range.ToString("F1") + "\n"
        };
        protected override void Awake()
        {
            _myaudioSource = gameObject.GetComponent<AudioSource>();

            base.Awake();
        }
        
        protected override void StartActiveAbility()
        {
            _soundList.PlayOneSFX(_myaudioSource, 0);
            Vector3 warpPosition = _entity.transform.position + (_warpDir * Range);
            _entity.WarpTo(warpPosition, Quaternion.identity);
        }

        public override void Move(NavMeshAgent navMeshAgent)
        {
            base.Move(navMeshAgent);
            
            Vector3 direction = navMeshAgent.velocity.normalized;
            if (direction != Vector3.zero)
                _warpDir = direction;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!Application.isPlaying || !enabled || _entity == null) 
                return;

            Vector3 warpPosition = _entity.transform.position + (_transform.forward * Range);
            
            Handles.color = Color.yellow;
            Handles.DrawLine(_entity.transform.position, warpPosition);
            Handles.DrawWireDisc(warpPosition, Vector3.up, 1);
        }
#endif
    }
}
