using System;
using System.Collections.Generic;
using Levels;
using Managers;
using Managers.Events;
using UnityEngine;

namespace UI
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowReference;

        private List<Transform> _targets = new List<Transform>();
        private List<Transform> _arrows = new List<Transform>();

        public void UpdateCompassArrow(Portal[] targets)
        {
            _targets.Clear();

            foreach (Transform arrows in _arrows)
                if(arrows != null)
                    Destroy(arrows.gameObject);

            _targets.Clear();
            _arrows.Clear();
            
            foreach (Portal portal in targets)
                AddTarget(portal.transform);
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] == null)
                {
                    _targets.RemoveAt(i);
                    Destroy(_arrows[i].gameObject);
                    _arrows.RemoveAt(i);
                    i--;
                    continue;
                }
                
                _arrows[i].forward = (_targets[i].position - transform.position).normalized;
            }
        }

        public void AddTarget(Transform target)
        {
            _targets.Add(target);
            _arrows.Add(Instantiate(_arrowReference, transform).transform);
            _arrows[^1].gameObject.SetActive(true);
        }

        public void RemoveTarget(Transform target)
        {
            _targets.Remove(target);
            Destroy(_arrows[^1].gameObject);
            _arrows.Remove(_arrows[^1]);
        }
    }
}