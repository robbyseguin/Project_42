using System.Collections.Generic;
using Entities.Parts.Cockpits;
using Entities.Parts.Heads;
using Entities.Parts.HeavyWeapons;
using Entities.Parts.LightWeapons;
using Entities.Parts.Movements;
using Factories;
using Levels.Interactable;
using Managers.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utilities;

namespace Entities.Parts
{
    public class PartsHandler : MonoBehaviour
    {
        [SerializeField] private Entity _entity;
        
        private PartsFactory _partsFactory => PartsFactory.Instance;
        private bool IsPlayer => _entity.IsPlayer;

        private MovementPart _movementPart;
        private CockpitPart _cockpitPart;
        private HeadPart _headPart;
        private LightWeaponPart _lightWeaponPart;
        private HeavyWeaponPart _heavyWeaponPart;

        public int MaxHealth => _cockpitPart? _cockpitPart.MaxHealth: 1;
        public PartIdentifier SerialNumber => _headPart.PartID | _movementPart.PartID | _cockpitPart.PartID | _lightWeaponPart.PartID | _heavyWeaponPart.PartID;
        
        public int Score => _cockpitPart.Score;

        private Transform _movementPartMount => gameObject.transform;
        private Transform _cockpitMount;
        private Transform _headPartMount;
        private Transform _lightWeaponMount;
        private Transform _heavyWeaponMount;

        public int OnDamage(int hitPoint) => _cockpitPart.OnDamage(hitPoint);
        
        private void Start()
        {
            if (!_movementPart) 
                Assemble();
        }
        
        //private void OnDestroy() => Disassemble();

        public void AimAt(Vector3 target)
        {
            _lightWeaponPart?.AimAt(target);
            _heavyWeaponPart?.AimAt(target);
            _cockpitPart?.AimAt(target);
        }

        public void Move(NavMeshAgent agent) => _movementPart?.Move(agent);
        public void LightAttack(InputActionPhase phase) => _lightWeaponPart?.Ability(phase, IsPlayer);
        public void HeavyAttack(InputActionPhase phase) => _heavyWeaponPart?.Ability(phase, IsPlayer);
        public void HeadAbility(InputActionPhase phase) => _headPart?.Ability(phase, IsPlayer);
        public void MovementAbility(InputActionPhase phase) => _movementPart?.Ability(phase, IsPlayer);

        public void Assemble(PartIdentifier partIdentifier = 0)
        {
            PartIdentifier serialNumber = 0;

            serialNumber |= ValidatePartSelection(partIdentifier, PartIdentifier.GROUP_MOVEMENT_PART);
            serialNumber |= ValidatePartSelection(partIdentifier, PartIdentifier.GROUP_COCKPIT_PART);
            serialNumber |= ValidatePartSelection(partIdentifier, PartIdentifier.GROUP_HEAD_PART);
            serialNumber |= ValidatePartSelection(partIdentifier, PartIdentifier.GROUP_LIGHT_WEAPON_PART);
            serialNumber |= ValidatePartSelection(partIdentifier, PartIdentifier.GROUP_HEAVY_WEAPON_PART);
            
            Equip(_partsFactory.GetPart(serialNumber & PartIdentifier.GROUP_MOVEMENT_PART));
            Equip(_partsFactory.GetPart(serialNumber & PartIdentifier.GROUP_COCKPIT_PART));
            Equip(_partsFactory.GetPart(serialNumber & PartIdentifier.GROUP_HEAD_PART));
            Equip(_partsFactory.GetPart(serialNumber & PartIdentifier.GROUP_LIGHT_WEAPON_PART));
            Equip(_partsFactory.GetPart(serialNumber & PartIdentifier.GROUP_HEAVY_WEAPON_PART));
        }

        public void UpdateHud()
        {
            _movementPart.Publish(PartsEvents.EQUIPED);
            _cockpitPart.Publish(PartsEvents.EQUIPED);
            _headPart.Publish(PartsEvents.EQUIPED);
            _lightWeaponPart.Publish(PartsEvents.EQUIPED);
            _heavyWeaponPart.Publish(PartsEvents.EQUIPED);
        }

        public void UpgradePart(PartIdentifier partGroup)
        {
            if((partGroup & PartIdentifier.GROUP_HEAD_PART) != 0)
            {
                _headPart.UpdatePartPower();
                _headPart.Publish(PartsEvents.EQUIPED);
            }
            if((partGroup & PartIdentifier.GROUP_MOVEMENT_PART) != 0)
            {
                _movementPart.UpdatePartPower();
                _movementPart.Publish(PartsEvents.EQUIPED);
            }
            if((partGroup & PartIdentifier.GROUP_COCKPIT_PART) != 0)
            {
                _cockpitPart.UpdatePartPower();
                _cockpitPart.Publish(PartsEvents.EQUIPED);
            }
            if((partGroup & PartIdentifier.GROUP_LIGHT_WEAPON_PART) != 0)
            {
                _lightWeaponPart.UpdatePartPower();
                _lightWeaponPart.Publish(PartsEvents.EQUIPED);
            }
            if((partGroup & PartIdentifier.GROUP_HEAVY_WEAPON_PART) != 0)
            {
                _heavyWeaponPart.UpdatePartPower();
                _heavyWeaponPart.Publish(PartsEvents.EQUIPED);
            }
        }
        
        private PartIdentifier ValidatePartSelection(PartIdentifier partIdentifier, PartIdentifier partValidation)
        {
            return (partIdentifier & partValidation) == 0 ? partValidation : partIdentifier & partValidation;
        }

        public void Disassemble()
        {
            _movementPart.Destroy();
            _movementPart = null;
            
            _cockpitPart.Destroy();
            _cockpitPart = null;
            
            _headPart.Destroy();
            _headPart = null;
            
            _lightWeaponPart.Destroy();
            _lightWeaponPart = null;
            
            _heavyWeaponPart.Destroy();
            _heavyWeaponPart = null;
        }
        public void Disassemble(Loot loot)
        {
            switch (Random.Range(0,5))
            {
                case 0:
                    loot.SetLoot(_movementPart.gameObject);
                    _movementPart = null;
                    break;
                case 1:
                    loot.SetLoot(_cockpitPart.gameObject);
                    _cockpitPart = null;
                    break;
                case 2:
                    loot.SetLoot(_headPart.gameObject);
                    _headPart = null;
                    break;
                case 3:
                    loot.SetLoot(_lightWeaponPart.gameObject);
                    _lightWeaponPart = null;
                    break;
                case 4:
                    loot.SetLoot(_heavyWeaponPart.gameObject);
                    _heavyWeaponPart = null;
                    break;
            }
            
            Disassemble();
        }

        public void ResetPartToDefault(PartIdentifier illegalParts)
        {
            illegalParts &= SerialNumber;
            
            if(illegalParts == 0)
                return;
            
            if ((illegalParts & PartIdentifier.GROUP_MOVEMENT_PART) != 0 & _movementPart.CanRemove)
                Equip(_partsFactory.GetPart(~illegalParts & PartIdentifier.GROUP_MOVEMENT_PART));
            
            if ((illegalParts & PartIdentifier.GROUP_COCKPIT_PART) != 0 & _cockpitPart.CanRemove)
                Equip(_partsFactory.GetPart(~illegalParts & PartIdentifier.GROUP_COCKPIT_PART));
            
            if ((illegalParts & PartIdentifier.GROUP_HEAD_PART) != 0 & _headPart.CanRemove)
                Equip(_partsFactory.GetPart(~illegalParts & PartIdentifier.GROUP_HEAD_PART));
            
            if ((illegalParts & PartIdentifier.GROUP_LIGHT_WEAPON_PART) != 0 & _lightWeaponPart.CanRemove)
                Equip(_partsFactory.GetPart(~(illegalParts | PartIdentifier.LIGHT_WEAPON_PART_DEFAULT) & PartIdentifier.GROUP_LIGHT_WEAPON_PART));
            
            if ((illegalParts & PartIdentifier.GROUP_HEAVY_WEAPON_PART) != 0 & _heavyWeaponPart.CanRemove)
                Equip(_partsFactory.GetPart(~illegalParts & PartIdentifier.GROUP_HEAVY_WEAPON_PART));
        }
        
        public void Equip(Part part)
        {
            switch (part)
            {
                case MovementPart movementPart:
                    _cockpitMount = movementPart.CockpitMount;
                    _cockpitPart?.transform.SetParent(_cockpitMount,false);
                    Equip(movementPart, ref _movementPart, _movementPartMount);
                    break;
                case CockpitPart cockpitPart:
                    _headPartMount = cockpitPart.HeadMount;
                    _lightWeaponMount = cockpitPart.LightWeaponMount;
                    _heavyWeaponMount = cockpitPart.HeavyWeaponMount;
                    _headPart?.transform.SetParent(_headPartMount,false);
                    _lightWeaponPart?.transform.SetParent(_lightWeaponMount,false);
                    _heavyWeaponPart?.transform.SetParent(_heavyWeaponMount,false);
                    Equip(cockpitPart, ref _cockpitPart, _cockpitMount);
                    _entity.UpdateLife();
                    break;
                case HeadPart headPart:
                    Equip(headPart, ref _headPart, _headPartMount);
                    break;
                case LightWeaponPart weapon:
                    Equip(weapon,ref _lightWeaponPart, _lightWeaponMount);
                    break;
                case HeavyWeaponPart heavyWeapon:
                    Equip(heavyWeapon, ref _heavyWeaponPart, _heavyWeaponMount);
                    break;
            }
        }

        private void Equip<T>(T newPart, ref T slotPart, Transform mount) where T : Part
        {
            slotPart?.OnDrop();
            newPart.OnEquip(_entity, mount);
            slotPart = newPart;
        }

        public string[] GetPartInfo(PartIdentifier partIdentifier)
        {
            string[] info = new []
            {
                "",
                ""
            };

            if ((partIdentifier & PartIdentifier.GROUP_MOVEMENT_PART) != 0)
            {
                info[0] += _movementPart.Info[0];
                info[1] += _movementPart.Info[1];
            }

            if ((partIdentifier & PartIdentifier.GROUP_COCKPIT_PART) != 0)
            {
                info[0] +=_cockpitPart.Info[0];
                info[1] +=_cockpitPart.Info[1];
            }

            if ((partIdentifier & PartIdentifier.GROUP_HEAD_PART) != 0)
            {
                info[0] += _headPart.Info[0];
                info[1] += _headPart.Info[1];
            }

            if ((partIdentifier & PartIdentifier.GROUP_LIGHT_WEAPON_PART) != 0)
            {
                info[0] += _lightWeaponPart.Info[0];
                info[1] += _lightWeaponPart.Info[1];
            }

            if ((partIdentifier & PartIdentifier.GROUP_HEAVY_WEAPON_PART) != 0)
            {
                info[0] += _heavyWeaponPart.Info[0];
                info[1] += _heavyWeaponPart.Info[1];
            }

            return info;
        }

        public Sprite[] GetPartImage(PartIdentifier partIdentifier)
        {
            List<Sprite> sprites = new List<Sprite>();

            if ((partIdentifier & PartIdentifier.GROUP_MOVEMENT_PART) != 0)
                sprites.Add(_movementPart.Icon);

            if ((partIdentifier & PartIdentifier.GROUP_COCKPIT_PART) != 0)
                sprites.Add(_cockpitPart.Icon);

            if ((partIdentifier & PartIdentifier.GROUP_HEAD_PART) != 0)
                sprites.Add(_headPart.Icon);

            if ((partIdentifier & PartIdentifier.GROUP_LIGHT_WEAPON_PART) != 0)
                sprites.Add(_lightWeaponPart.Icon);

            if ((partIdentifier & PartIdentifier.GROUP_HEAVY_WEAPON_PART) != 0)
                sprites.Add(_heavyWeaponPart.Icon);

            return sprites.ToArray();
        }
    }
}