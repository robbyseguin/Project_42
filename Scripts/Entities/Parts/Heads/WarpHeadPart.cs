using Datas;
using UnityEditor;
using UnityEngine;

namespace Entities.Parts.Heads
{
    public class WarpHeadPart : HeadPart
    {
        public override PartIdentifier PartID => PartIdentifier.HEAD_PART_WARP;
        public override string Name => GetLocalizedString("Tête de téléportation", "Warp head part");
        public override string Description => GetLocalizedString("Téléportes-toi dans la direction pointée avec Q", 
            "Warp in the direction of the pointer with Q");

        private AudioSource _myAudioSource;
        [SerializeField] private SoundDefinition _soundList;
        
        protected override void Awake()
        {
            _myAudioSource = gameObject.GetComponent<AudioSource>();
            base.Awake();
        }

        protected override void StartActiveAbility()
        {
            _soundList.PlayOneSFX(_myAudioSource, 0);
            Vector3 warpPosition = _entity.transform.position + (_transform.forward * Range);
            _entity.WarpTo(warpPosition, Quaternion.identity);
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