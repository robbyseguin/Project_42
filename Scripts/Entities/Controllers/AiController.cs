using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Managers;

namespace Entities.Controllers
{
    public class AiController : Controller
    {
        private float _attackRange;
        private Transform _objective;
        private NavMeshAgent _navMeshAgent;
        private CapsuleCollider _collider;

        private Vector3 _destination;
        private Vector3 _aimSpot;
        private bool _followPlayer;

        private void Start()
        {
            _objective = GameObject.FindObjectOfType<PlayerController>().transform;
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _attackRange = AIManager.Instance.GetAttackRange();
            _navMeshAgent.stoppingDistance = 0.0f;
        }

        private void Update()
        {
            if(_followPlayer)
            {
                Entity.Move(_destination);
                Entity.AimAt(_objective.position + Vector3.up);

                float distance = Vector3.Distance(_objective.position, transform.position);

                if(distance > _attackRange)
                {
                    Entity.LightAttack(InputActionPhase.Canceled);
                    Entity.HeavyAttack(InputActionPhase.Canceled);
                }
                else
                {
                    Entity.LightAttack(InputActionPhase.Performed);
                    Entity.HeavyAttack(InputActionPhase.Performed);
                }
            }
            else
            {
                Entity.Move(_destination);
                Entity.AimAt(_aimSpot + Vector3.up);
            } 
        }

        public void MoveTowards(Vector3 pos) => _destination = pos;
        public void AimTowards(Vector3 pos) => _aimSpot = pos;
        public bool PlayerInRange() { return _followPlayer; }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.GetComponentInChildren<PlayerController>())
            {
                _followPlayer = true;
                AIManager.Instance.AddUnitToSquad(this);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if(collider.GetComponentInChildren<PlayerController>())
            {
                _followPlayer = false;
                _destination = transform.position;
                AIManager.Instance.RemoveUnitFromSquad(this);
            }
        }

        private void OnDrawGizmos()
        {
            
            if(!Application.isPlaying) 
                return;
            
            Vector3[] corners = _navMeshAgent.path.corners;
        
            if(!corners.Any())
                return;
            
            Vector3 last = Vector3.zero;
            bool first = true;
        
            foreach (Vector3 corner in corners)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(corner,.2f);
            
                if(!first)
                    Gizmos.DrawLine(last, corner);

                last = corner;
                first = false;
            }
        }
    }
}
