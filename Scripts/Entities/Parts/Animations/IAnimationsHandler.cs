using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Parts.Animations
{
    public interface IAnimationsHandler
    {
        public void AnimateMovement(Transform partTransform, float animationSpeed, Vector3 direction);
        public void AnimateAbility(float animationSpeed = 1);
    }
}