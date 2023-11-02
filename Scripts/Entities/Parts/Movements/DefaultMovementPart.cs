namespace Entities.Parts.Movements
{
    public class DefaultMovementPart : MovementPart
    {
        public override PartIdentifier PartID => PartIdentifier.MOVEMENT_PART_DEFAULT;
        public override string Name => GetLocalizedString("Jambes par défault", "Default legs");
        public override string Description => GetLocalizedString("", "");

        protected override void StartActiveAbility()
        {
        }
    }
}
