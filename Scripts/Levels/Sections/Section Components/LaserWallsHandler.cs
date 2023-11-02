using System.Collections;
using Levels.Sections;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace SectionComponents
{
    public class LaserWallsHandler : SectionComponent
    {
        [SerializeField] private LaserLine[] _poles;
        [SerializeField] private NavMeshObstacle[] _forceField;
        [SerializeField] private float _timer = 60;

        public override void OnSectionBake()
        {
            base.OnSectionBake();

            _poles = GetComponentsInChildren<LaserLine>();
            _forceField = GetComponentsInChildren<NavMeshObstacle>();
        }

        public override void OnSectionReset()
        {
            LowerWalls();
            _timer *= DifficultyManager.Instance.SpawnerMultiplier;
            foreach (var field in _forceField)
            {
                field.enabled = true;
            }
            
            base.OnSectionReset();
        }

        public override void OnSectionStart()
        {
            base.OnSectionStart();

            for (int i = 0; i < _poles.Length; ++i)
            {
                _poles[i].SetLaserLines(i < _poles.Length - 1 ? _poles[i + 1].Target : _poles[0].Target);
            }
            
            StartCoroutine(ArenaTimer());
        }

        private IEnumerator ArenaTimer()
        {
            yield return new WaitForSeconds(_timer);
            
            LowerWalls();
        }

        private void LowerWalls()
        {
            foreach (var field in _forceField)
            {
                field.enabled = false;
            }
            
            foreach (var wall in _poles)
            {
                wall.SetLaserLines(wall.Target);
            }

            base.OnSectionReset();
        }
    }
}
