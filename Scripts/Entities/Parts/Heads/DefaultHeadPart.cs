namespace Entities.Parts.Heads
{
    public class DefaultHeadPart : HeadPart
    {
        public override PartIdentifier PartID => PartIdentifier.HEAD_PART_DEFAULT;
        public override string Name => GetLocalizedString("Tête par défaut", "Default head part");
        public override string Description => GetLocalizedString("", "");

        protected override void StartActiveAbility()
        {
        }

        protected override void StopActiveAbility()
        {
        }
    }
}
