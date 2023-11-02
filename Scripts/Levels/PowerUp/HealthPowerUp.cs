using System;
using Datas;
using Entities;
using Managers;
using UnityEngine;

namespace Levels.Interactable
{
    public class HealthPowerUp : PowerUp
    {
        [SerializeField] private int _healAmount = 15;
        [SerializeField] private SoundDefinition _healSounds;
        [SerializeField] private Color _mainColor = Color.green;
        
        private int _calculatedHeal;
        public override string[] Info => new[] { GetLocalizedString("Répare","Repairs"), _calculatedHeal.ToString() };
        public override string Name => GetLocalizedString("Kit de réparation", "Repair kit");
        public override string Description => GetLocalizedString("", "");
        public override Color MainColor => _mainColor;

        private void OnEnable()
        {
            _calculatedHeal = (int)(_healAmount * DifficultyManager.Instance.HealthMultiplier);
        }

        public override bool OnPickUp(GameObject target)
        {            
            if (!target.TryGetComponent(out Entity entity))                        
                return false;         

            entity.Heal((int)_calculatedHeal);
            _healSounds.PlayOneSFX(AudioManager.Instance._sfxSource, 0);           
            return true;
        }

        public override void OnDrop() { }
    }
}