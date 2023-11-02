using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

namespace Entities.Controllers
{
    public class PlayerController : Controller
    {
        private Camera _cam;
       
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Transform _pointer;
              
        private Vector2 _moveDirection = Vector2.zero;
                    
        private PlayerInput _playerInputs;

        private InputAction _move;
        private InputAction _lightWeapon;
        private InputAction _heavyWeapon;
        private InputAction _headAbility;
        private InputAction _legAbility;
        private InputAction _interact;

        protected override void Awake()
        {
            base.Awake();
            
            _playerInputs = new PlayerInput();           
            _cam = Camera.main;
            _pointer.transform.parent = null;
            _isPlayer = true;

            LocalizationSettings.SelectedLocaleChanged += _ => Entity.PartsHandler.UpdateHud();
        }

        private void OnDestroy()
        {
            if(_pointer != null)
                Destroy(_pointer.gameObject);
        }

        private void OnEnable()
        {
            _move = _playerInputs.Keyboard.Move;
            _move.Enable();
            _lightWeapon = _playerInputs.Keyboard.LightWeapon;
            _lightWeapon.Enable();
            _lightWeapon.performed += FireLight;
            _lightWeapon.canceled += FireLight;
            _heavyWeapon = _playerInputs.Keyboard.HeavyWeapon;
            _heavyWeapon.Enable();
            _heavyWeapon.performed += FireHeavy;
            _heavyWeapon.canceled += FireHeavy;
            _headAbility = _playerInputs.Keyboard.HeadAbility;
            _headAbility.Enable();
            _headAbility.performed += HeadAbility;
            _headAbility.canceled += HeadAbility;
            _legAbility = _playerInputs.Keyboard.LegAbility;
            _legAbility.Enable();
            _legAbility.performed += LegAbility;
            _legAbility.canceled += LegAbility;
            _interact = _playerInputs.Keyboard.Interact;
            _interact.Enable();
            _interact.performed += InteractAction;
            _interact.canceled += InteractAction;
        }
        
        private void OnDisable()
        {
            _move.Disable();
            _lightWeapon.Disable();
            _heavyWeapon.Disable();
            _headAbility.Disable();
            _legAbility.Disable();
            _interact.Disable();
        }
        
        private void Update()
        {
            _moveDirection = _move.ReadValue<Vector2>();
        }
        
        private void FixedUpdate()
        {
            MoveCharacter();
            RotateCharacter();
        }

        private void MoveCharacter()
        {
            Vector3 dir = new Vector3(_moveDirection.x, 0, _moveDirection.y);
            Vector3 pos = transform.position;

            Vector3 target = pos + dir;
            
            if(NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                target = hit.position;
            
            Entity.Move(target);
        }
         
        private void RotateCharacter()
        {
            Vector3 newPos;

            if (GetMousePosition(out newPos, out bool isDamageable) && !isDamageable)
                newPos.y += 1.8f;
            
            Entity.AimAt(newPos);
        }
        
        void FireLight(InputAction.CallbackContext value)
        {
            Entity.LightAttack(value.phase);
        }

        void FireHeavy(InputAction.CallbackContext value)
        {
            Entity.HeavyAttack(value.phase);
        }

        void HeadAbility(InputAction.CallbackContext value)
        {
            Entity.HeadAbility(value.phase);
        }

        void LegAbility(InputAction.CallbackContext value)
        {
            Entity.LegAbility(value.phase);
        }

        void InteractAction(InputAction.CallbackContext value)
        {
            Entity.Interact(value.phase);
        }
        
        private bool GetMousePosition(out Vector3 position, out bool isDamageable)
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            position = Vector3.zero;

            isDamageable = false;
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
            {
                Cursor.visible = false;
                _pointer.position = hitInfo.point;
                
                if(Entity != null && hitInfo.transform.TryGetComponent<IInteractable>(out var interactable))
                {
                    Entity.InteractableTarget = interactable;
                }

                if (hitInfo.transform.TryGetComponent(out IDamageable target))
                    isDamageable = true;
                
                position = hitInfo.point;
                return true;
            }

            Cursor.visible = true;
            
            return false;
        }
    }
}