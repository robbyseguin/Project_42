using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Parts.Movements
{
    public class SprintMovementPart : MovementPart
    {
        public override PartIdentifier PartID => PartIdentifier.MOVEMENT_PART_SPRINT;
        [SerializeField] private float _speedMultiplier = 1.5f;
        
        public override string Name => GetLocalizedString("Tracks turbo", "Sprint movement part");
        public override string Description => GetLocalizedString("Maintenez Espace pour accélérer", "Hold Space go even faster");
        
        public override string[] Info => new[]
        {
            base.Info[0] + GetLocalizedString("Multiplicateur de vitesse","Speed modifier") + "\n",
            base.Info[1] + _speedMultiplier.ToString("F1") + "\n"
        };
        
        protected override void StartActiveAbility()
        {
            _speed *= _speedMultiplier;
        }

        protected override void StopActiveAbility()
        {
            _speed /= _speedMultiplier;
        }
    }
}
