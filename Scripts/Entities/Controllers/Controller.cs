using UnityEngine;

namespace Entities.Controllers
{
    public abstract class Controller : MonoBehaviour
    {
        private Entity _entity;
        protected Entity Entity => _entity;

        protected bool _isPlayer = false;

        public bool IsPlayer => _isPlayer;
    
        protected virtual void Awake()
        {
            if (!transform.parent.TryGetComponent(out _entity))
                this.enabled = false;
        }

        public virtual void UpdateControllable()
        {
            if (!transform.parent.TryGetComponent(out _entity))
                this.enabled = false;
        }
    }
}