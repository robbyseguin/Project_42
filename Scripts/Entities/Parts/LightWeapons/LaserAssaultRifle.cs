namespace Entities.Parts.LightWeapons
{
    public class LaserAssaultRifle : Rifle
    {
        public override PartIdentifier PartID => PartIdentifier.LIGHT_WEAPON_PART_LASER_ASSAULT_RIFLE;
        public override string Name => GetLocalizedString("Fusil d'assault laser", "Laser assault rifle");
        public override string Description => GetLocalizedString("Rapide, mais imprécis et faible", 
            "Quick, but unprecise and low power");
    }
}