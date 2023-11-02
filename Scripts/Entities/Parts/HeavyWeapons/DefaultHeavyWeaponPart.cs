using System.Collections;
using UnityEngine;

namespace Entities.Parts.HeavyWeapons
{
    public class DefaultHeavyWeaponPart : HeavyWeaponPart
    {
        public override PartIdentifier PartID => PartIdentifier.HEAVY_WEAPON_PART_DEFAULT;
        public override string Name => GetLocalizedString("Arme lourde brisée", "Default heavy weapon");
        public override string Description => GetLocalizedString("", "");

        protected override void Shoot() { }
        protected override void StartActiveAbility() { }
    }
}
