using Entities.Parts.Animations;
using UnityEngine;

public class AHSniperRifle : MonoBehaviour, IAnimationsHandler
{
    private Animator _animator;
    private static readonly int Shoot = Animator.StringToHash("Shoot");
        
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimateMovement(Transform partTransform, float animationSpeed, Vector3 direction) { }

    public void AnimateAbility(float animationSpeed = 1)
    {
        _animator.SetTrigger(Shoot);
    }
}
