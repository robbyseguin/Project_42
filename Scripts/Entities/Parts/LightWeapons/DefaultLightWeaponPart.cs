using System.Collections;
using UnityEngine;

namespace Entities.Parts.LightWeapons
{
    public class DefaultLightWeaponPart : LightWeaponPart
    {
        public override PartIdentifier PartID => PartIdentifier.LIGHT_WEAPON_PART_DEFAULT;
        public override string Name => GetLocalizedString("Arme légère par défaut", "Default light weapon");
        public override string Description => GetLocalizedString("", "");

        protected override void Shoot() { }

        protected override void StartActiveAbility() { }
    }
}
