namespace Entities.Parts.Cockpits
{
    public class DefaultCockpit : CockpitPart
    {
        public override PartIdentifier PartID => PartIdentifier.COCKPIT_PART_DEFAULT;
        public override string Name => GetLocalizedString("Cockpit par défaut", "Default cockpit");
        public override string Description => GetLocalizedString("", "");
    }
}
