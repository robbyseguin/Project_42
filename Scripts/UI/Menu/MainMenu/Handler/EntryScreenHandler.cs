public class EntryScreenHandler : UIHandler
{
    protected override void Awake()
    {
        base.Awake();
        Show();
    }

    public override void OnEnableMenu(float transitionTime)
    {
        gameObject.SetActive(false);
    }
}
