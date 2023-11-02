using UnityEngine;

public class BackgroundHandler : UIHandler
{
    private Animator _titleAnimator;
    private static readonly int _isGoingMenu = Animator.StringToHash("isGoingMenu");

    protected override void Awake()
    {
        base.Awake();
        Show();
        
        _titleAnimator = transform.GetComponentInChildren<Animator>();
    }

    public override void OnEnableMenu(float transitionTime)
    {
        _titleAnimator.SetTrigger(_isGoingMenu);
    }
}
