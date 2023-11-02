using UnityEngine;

namespace Entities.Parts.Animations
{
    public class AHSpiderMovements : MonoBehaviour, IAnimationsHandler
    {
        private Animator _animator;
        private static readonly int DirectionX = Animator.StringToHash("DirectionX");
        private static readonly int DirectionZ = Animator.StringToHash("DirectionZ");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void AnimateMovement(Transform partTransform, float speed, Vector3 direction)
        {
            if(direction != Vector3.zero)
                partTransform.forward = new Vector3(0,Mathf.Abs(direction.y),1);
            
            _animator.speed = speed * 0.1f;
            _animator.SetFloat(DirectionX, direction.x);
            _animator.SetFloat(DirectionZ, direction.z);
        }

        public void AnimateAbility(float animationSpeed) { }
    }
}