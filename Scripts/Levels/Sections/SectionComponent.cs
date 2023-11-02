using System;
using UnityEngine;

namespace Levels.Sections
{
    public abstract class SectionComponent : MonoBehaviour
    {
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _initialScale;

        private Transform _transform;

        protected virtual void Awake()
        {
            _transform = transform;
            
            _initialPosition = _transform.localPosition;
            _initialRotation = _transform.localRotation;
            _initialScale = _transform.localScale;
        }

        public virtual void OnSectionInitialisation()
        {
        }

        public virtual void OnSectionStart()
        {
        }

        public virtual void OnSectionReset()
        {
            ResetState();
        }

        public virtual void OnSectionBake()
        {
        }

        protected virtual void ResetState()
        {
            _transform.localPosition = _initialPosition;
            _transform.localRotation = _initialRotation;
            _transform.localScale = _initialScale;
        }
    }
}
