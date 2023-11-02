using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities.Controllers;

namespace Managers
{
    public class AIManager : Singleton<AIManager>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _radiusAroundTarget;
        [SerializeField] private List<AiController> Units = new List<AiController>();

        private void Update()
        {
            if(Units.Count > 0)
            {
                UnitsCircleTarget();
            }
        }

        public void InitializeTarget(GameObject player)
        {
            _target = player.transform;
            ClearSquadList();
        }

        private void ClearSquadList()
        {
            Units.Clear();
        }

        public float GetAttackRange()
        {
            return _radiusAroundTarget;
        }

        private void UnitsCircleTarget()
        {
            Units.RemoveAll(AiController => AiController == null);
            
            for(int i = 0; i < Units.Count; i++)
            {
                Units[i].MoveTowards(new Vector3(
                    _target.position.x + _radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Units.Count),
                    _target.position.y,
                    _target.position.z + _radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / Units.Count)));
            }
        }

        public void AddUnitToSquad(AiController ai)
        {
            Units.Add(ai);
        }

        public void RemoveUnitFromSquad(AiController ai)
        {
            Units.Remove(ai);
        }
    }
}
