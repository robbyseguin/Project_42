using System.Collections.Generic;
using Levels.Interactable;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Entities.Parts
{
    public static class PartsEvents
    {
        public const int EQUIPED = 0;
        public const int ACTIVATED = 1;
    }
    
    public abstract class Part : MonoBehaviour, ILootable
    {
        [SerializeField] protected bool _canRemove = true;
        [SerializeField, HideInInspector] protected List<GameObject> _levels;
        [SerializeField] protected float[] _powerUpThreshold;
        [SerializeField] protected Sprite _icon;

        public abstract PartIdentifier PartID { get; }

        public bool CanRemove => _canRemove;
        
        public Sprite Icon => _icon;
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual string[] Info => default;

        public Color MainColor => GetLevelColor(_currentLevel);

        protected string GetLocalizedString(string fr, string en) => LocalizationManager.GetLocalizedString(fr, en);
        
        public virtual float Delay
        {
            get => -1.0f;
            protected set { } 
        }

        public bool IsPlayer => _entity.IsPlayer;
        
        protected Entity _entity { get; private set; }
        protected Transform _transform;
        protected int _currentLevel = 0;
        protected float _currentMultiplier = 1;
        private float _lastActivation;
        
        protected virtual void Awake()
        {
            _transform = transform;

            Transform levels = transform.Find("Levels").transform;

            _levels.Clear();
            for (int i = 0; i < levels.childCount; i++)
            {
                _levels.Add(levels.GetChild(i).gameObject);
            }
        }

        protected virtual void OnEnable()
        {
            UpdatePartPower();
        }

        #region Part
        
        public void Ability(InputActionPhase phase, bool isPlayer)
        {
            switch (phase)
            {
                case InputActionPhase.Performed:
                    if (!(Delay <= 0 || Time.time > _lastActivation + Delay))
                        return;

                    StartActiveAbility();
                    _lastActivation = Time.time;
                    if(isPlayer)
                        PublishActivation(PartsEvents.ACTIVATED);
                    
                    break;
                case InputActionPhase.Canceled:
                    StopActiveAbility();
                    break;
            }
        }
        
        public virtual void OnEquip(Entity entity, Transform mount)
        {
            _entity = entity;
            _transform.parent = mount;
            ResetTransform();
            _lastActivation = Time.time - Delay;
            
            foreach (var go in _levels)
            {
                go.SetActive(false);
            }

            if (_currentLevel >= _levels.Count)
                _currentLevel = _levels.Count-1;
            
            _levels[_currentLevel].SetActive(true);
            
            if (IsPlayer)
                PublishActivation(PartsEvents.EQUIPED);
        }

        protected abstract void StartActiveAbility();
        protected virtual void StopActiveAbility() { }
        protected virtual void PublishActivation(int partEvent) { }
        
        public virtual void UpdatePartPower(float multiplier = 1)
        {
            _currentMultiplier = DifficultyManager.Instance.PartMultiplier * multiplier;
        }
        
        private void ResetTransform()
        {
            _transform.localPosition = Vector3.zero;
            _transform.localScale = Vector3.one;
            _transform.localRotation = Quaternion.identity;
        }
        
        #endregion

        #region ILootable
        
        public virtual void OnDrop()
        {
            this.Destroy();
        }

        public bool OnPickUp(GameObject target)
        {
            if (!target.TryGetComponent(out Entity entity))
                return false;

            AudioManager.Instance.PlaySFX(0);

            enabled = true;
            entity.Equip(this);
            
            return true;
        }
        
        #endregion

        private static Color GetLevelColor(int lvl)
        {
            switch (lvl)
            {
                case 0:
                    return Color.white;
                case 1:
                    return Color.green;
                case 2:
                    return Color.blue;
                case 3:
                    return Color.magenta;
                default:
                    return Color.red;
            }
        }
    }
}