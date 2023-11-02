using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Parts.Cockpits
{
    public class AbsorptionCockpit : CockpitPart
    {
        [SerializeField, Range(0,1)] private float _absortionChance = 0.3f;
        
        public override PartIdentifier PartID => PartIdentifier.COCKPIT_PART_ABSORBTION;
        public override string Name => GetLocalizedString("Cockpit d'absorbtion", "Absorption cockpit");
        public override string Description => GetLocalizedString("Chance aléatoire de ne pas recevoir de dommage", 
            "Random chance to negate incoming damage");

        public override string[] Info => new[]
        {
            base.Info[0] + GetLocalizedString("Chance d'absorbtion", "Absorption Chance") + "\n",
            base.Info[1] + _absortionChance.ToString("F1") + "\n"
        };

        public override int OnDamage(int hitPoint)
        {
            if (Random.value > _absortionChance)
                return hitPoint;

            return 0;
        }
    }
}
