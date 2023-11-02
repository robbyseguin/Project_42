using Entities.Parts.Animations;
using UnityEngine;

public class AHHumanoidMovements : MonoBehaviour, IAnimationsHandler
{
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimateMovement(Transform partTransform, float speed, Vector3 direction)
    {
        if(direction != Vector3.zero)
            partTransform.forward = direction;

        _animator.speed = speed * 0.2f;
        _animator.SetBool(Walking, direction != Vector3.zero);
    }

    public void AnimateAbility(float animationSpeed = 1)
    {
    }
}
