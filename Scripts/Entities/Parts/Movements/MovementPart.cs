using System.Collections.Generic;
using Entities.Parts.Animations;
using Managers;
using Managers.Events;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Parts.Movements
{
    public abstract class MovementPart : Part
    {
        [SerializeField] protected float _speed;
        [SerializeField, HideInInspector] protected List<Transform> _cockpitMounts = new List<Transform>();
        
        private IAnimationsHandler _anim;
        protected float Speed => _speed * _currentMultiplier;

        public override string[] Info => new[]
        {
            "\n" + GetLocalizedString("Délai", "Delay") + "\n" +
            GetLocalizedString("Vitesse", "Speed") + "\n", 
            "\n" + (Delay == -1 ? 0 : Delay.ToString("F1")) + "\n" +
            Speed.ToString("F1") + "\n"
        };
        
        public Transform CockpitMount => _cockpitMounts[_currentLevel];
        
        protected override void Awake()
        {
            base.Awake();
            
            Transform mounts = transform.Find("Mounts").transform;
            
            _cockpitMounts.Clear();
            
            for (int i = 0; i < mounts.childCount; i++)
            {
                _cockpitMounts.Add(mounts.GetChild(i).transform);
            }
        }

        public virtual void Move(NavMeshAgent navMeshAgent)
        {
            navMeshAgent.speed = IsPlayer ? Speed : Speed * DifficultyManager.Instance.EnemyMultiplier;
            _anim?.AnimateMovement(_transform, navMeshAgent.speed, navMeshAgent.velocity.normalized);
        }

        #region Part

        public override void OnEquip(Entity entity, Transform mount)
        {
            foreach (var t in _powerUpThreshold)
            {
                if (Speed >= t)
                {
                    _currentLevel++;
                }
            }
            
            base.OnEquip(entity, mount);
            
            _anim = _levels[_currentLevel].GetComponent<IAnimationsHandler>();
        }
        
        protected override void PublishActivation(int partEvent)
        {
            this.Publish(Delay, partEvent);
        }

        #endregion
    }
}