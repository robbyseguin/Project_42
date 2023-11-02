using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Entities.Controllers;
using Entities.Parts;
using Levels.Interactable;
using Managers.Events;
using TMPro;
using UnityEngine.UI;
using Utilities;
using Datas;
using Random = UnityEngine.Random;

namespace Entities
{
    public static class EntityEvent
    {
        public const int PLAYER_DEATH = 0;
        public const int AI_DEATH = 1;
        public const int LIFE_UPDATE = 2;
    }
    
    public class Entity : MonoBehaviour, IWarpable, IDamageable
    {
        [Header("Reference")]
        [SerializeField] private PartsHandler _partsHandler;
        [SerializeField] private Controller _brain;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Image _lifeImage;
        [SerializeField] private TMP_Text _serialText;
        [SerializeField] private Loot _loot;

        // Sounds
        [SerializeField] private SoundDefinition _ScriptableObjectSounds;
        private AudioSource _audioSource;

        private Transform _transform;
        private IInteractable _interactableTarget;

        public IInteractable InteractableTarget
        {
            get => _interactableTarget;
            set => _interactableTarget = value;
        }

        
        public int Health { get; private set; }
        public bool IsDead { get; protected set; }
        
        public int MaxHealth => PartsHandler.MaxHealth;
        public bool IsPlayer => _brain.IsPlayer;

        // getter
        public Controller Brain => _brain;
        public PartsHandler PartsHandler => _partsHandler;
        
        // Static getter
        public static Controller Player { get; private set; }
        public static int PlayerGuid { get; private set; }

        #region Unity Events
        
        private void Awake()
        {
            _transform = transform;
            _transform.forward = Vector3.forward;
        }

        private void Start()
        {
            SetBrain(_brain);
        }

        private void OnDestroy()
        {
            Destroy(_loot.gameObject);
        }

        #endregion

        #region Method passthrought
        
        public void Assemble(PartIdentifier partIdentifier = 0) => _partsHandler.Assemble(partIdentifier);
        public void Equip(Part part) => _partsHandler.Equip(part);
        
        #endregion

        #region Health
        
        public void TakeDamage(int hitPoints)
        {
            if(IsDead)
                return;

            hitPoints = _partsHandler.OnDamage(hitPoints);
            
            Health -= hitPoints;
            
            _ScriptableObjectSounds.PlayAsSFX(_audioSource, 0);
            _particleSystem.EmitWords(hitPoints.ToString(), Color.red);

            Health = Mathf.Clamp(Health, 0, MaxHealth);

            float relativeDamage = Mathf.InverseLerp(0,MaxHealth / 10,hitPoints);
            Color c = Color.Lerp(Color.green, Color.red, relativeDamage);
            _particleSystem.EmitWords(hitPoints.ToString(), c);

            UpdateLifeDisplay();
            
            if(Health <= 0)
                Die();
        }

        public void Heal(int hitPoints)
        {
            if(IsDead)
                return;

            Health += hitPoints;
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            
            _particleSystem.EmitWords(Health == MaxHealth? "Full":hitPoints.ToString(), Color.green);
            
            UpdateLifeDisplay();
        }

        public void UpdateLife()
        {
            Health = MaxHealth;
            _particleSystem.EmitWords("Full", Color.green);
            
            UpdateLifeDisplay();
        }

        public void UpdateLifeDisplay()
        {
            float lerpHealth = Mathf.InverseLerp(0, MaxHealth, Health);
            _lifeImage.fillAmount = lerpHealth;
            _audioSource = GetComponent<AudioSource>();
            if (!IsPlayer)
                return;
            
            this.Publish(lerpHealth, EntityEvent.LIFE_UPDATE);
        }
        
        private void Die()
        {
            IsDead = true;
            this.Publish(_partsHandler.Score,IsPlayer ? EntityEvent.PLAYER_DEATH : EntityEvent.AI_DEATH);
            
            if (IsPlayer)
            {
                PartsHandler.Disassemble();
                Destroy(_partsHandler.gameObject);
            }
            else
            {
                if (Random.Range(0, 10) == 0)
                {
                    _loot.gameObject.SetActive(true);
                    _loot.transform.SetParent(_transform.parent);
                    PartsHandler.Disassemble(_loot);
                }
                else
                {
                    PartsHandler.Disassemble();
                }

                gameObject.SetActive(false);
            }
        }
        
        #endregion

        public void SetBrain(Controller newBrain)
        {
            _brain = newBrain;
            _brain.transform.SetParent(_transform, false);
            _brain.UpdateControllable();

            UpdateLifeDisplay();
            _serialText.text = PartsHandler.SerialNumber.ToString("X");
            
            if (!IsPlayer)
                return;
            
            PlayerGuid = gameObject.GetInstanceID();
            _partsHandler.UpdateHud();
            Player = _brain;
        }

        
        #region Controls

        public void AimAt(Vector3 target)
        {
            _partsHandler.AimAt(target);
        }

        public void Interact(InputActionPhase phase)
        {
            if(_interactableTarget == null)
                return;
            
            switch (phase)
            {
                case InputActionPhase.Performed:
                    _interactableTarget.InteractionStarted();
                    break;
                case InputActionPhase.Canceled:
                    _interactableTarget.InteractionCanceled();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }

        public void Move(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);

            _partsHandler.Move(_navMeshAgent);
        }
        
        public virtual void LightAttack(InputActionPhase phase) => _partsHandler.LightAttack(phase);
        
        public virtual void HeavyAttack(InputActionPhase phase) => _partsHandler.HeavyAttack(phase);
        
        public virtual void HeadAbility(InputActionPhase phase) => _partsHandler.HeadAbility(phase);
        
        public virtual void LegAbility(InputActionPhase phase) =>_partsHandler.MovementAbility(phase);

        #endregion

        #region IWarpable
        
        public void WarpTo(Vector3 newPosition, Quaternion newRotation)
        {
            // Disabling CharacterController to move by transform because CharacterController is blocking moving player that way
            _navMeshAgent.enabled = false;

            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas))
            {
                _transform.position = hit.position;
                _transform.rotation = newRotation;
            }
            
            _navMeshAgent.enabled = true;
        }
        
        #endregion
    }
}