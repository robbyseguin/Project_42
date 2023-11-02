using Datas;
using Entities;
using Entities.Parts;
using Managers;
using UnityEngine;
using Utility;

namespace Levels.Interactable
{
    public class UpgradePowerUp : PowerUp
    {
        [SerializeField] private PartIdentifier[] _possiblePartGroups;
        [SerializeField] private SoundDefinition _upgradeSounds;
        
        private Color _mainColor = Color.yellow;
        public override string Name => GetLocalizedString("Kit de mise à niveau", "Upgrade kit");
        public override string Description => GetLocalizedString("Augmente la puissance d'une pièce au hazard", "Upgrades on random part's power");
        public override Color MainColor => _mainColor;
        
        public override bool OnPickUp(GameObject target)
        {            
            if (!target.TryGetComponent(out Entity entity))                        
                return false;         

            entity.PartsHandler.UpgradePart(_possiblePartGroups.RandomIndex());
            _upgradeSounds.PlayOneSFX(AudioManager.Instance._sfxSource, 0);           
            return true;
        }

        public override void OnDrop() { }
    }
}