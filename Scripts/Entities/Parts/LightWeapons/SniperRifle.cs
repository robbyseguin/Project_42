using UnityEngine;

namespace Entities.Parts.LightWeapons
{
    public class SniperRifle : Rifle
    {
        public override PartIdentifier PartID => PartIdentifier.LIGHT_WEAPON_PART_SNIPER_RIFLE;

        private AHSniperRifle _sniperRifleAnimation;

        public override string Name => GetLocalizedString("Carabine de précision", "Sniper rifle");
        public override string Description => GetLocalizedString("Longue portée, bon dommage et présicion, mais lent", 
            "Long range, decent damage and precision, but slow");
        public override float Delay { get => FireDelay; protected set { } }

        public override void OnEquip(Entity entity, Transform mount)
        {
            base.OnEquip(entity, mount);
            
            _sniperRifleAnimation = _levels[_currentLevel].GetComponent<AHSniperRifle>();
        }

        protected override void Shoot()
        {
            base.Shoot();
            _sniperRifleAnimation.AnimateAbility();
        }
    }
}