using System.Collections.Generic;
using System.Linq;
using Entities.Parts.Movements;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Parts.Animations
{
    public class AHTracksMovements : MonoBehaviour, IAnimationsHandler
    {
        private Animator _animator;
        private List<Roll_Tank_Tracks> _rollingTracks = new ();
        
        private static readonly int Walking = Animator.StringToHash("Walking");
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rollingTracks = GetComponentsInChildren<Roll_Tank_Tracks>(false).ToList();
        }

        public void AnimateMovement(Transform partTransform, float speed, Vector3 direction)
        {
            if(direction != Vector3.zero)
                partTransform.forward = direction;
            
            foreach (var track in _rollingTracks)
                track.RollTracks(speed * direction.sqrMagnitude);

            _animator.SetBool(Walking, direction != Vector3.zero);
        }

        public void AnimateAbility(float animationSpeed) { }
    }
}