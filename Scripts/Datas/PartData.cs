using Entities.Parts.Cockpits;
using Entities.Parts.Heads;
using Entities.Parts.HeavyWeapons;
using Entities.Parts.LightWeapons;
using Entities.Parts.Movements;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(menuName = "Project 42/New Part Data", fileName = "PD_Default")]
    public class PartData : ScriptableObject
    {
        [SerializeField] private MovementPart[] _movementParts;
        [SerializeField] private CockpitPart[] _cockpitParts;
        [SerializeField] private HeadPart[] _headParts;
        [SerializeField] private LightWeaponPart[] _lightWeaponParts;
        [SerializeField] private HeavyWeaponPart[] _heavyWeaponParts;
        public MovementPart[] MovementParts => _movementParts;
        public CockpitPart[] CockpitParts => _cockpitParts;
        public HeadPart[] HeadParts => _headParts;
        public LightWeaponPart[] LightWeaponParts => _lightWeaponParts;
        public HeavyWeaponPart[] HeavyWeaponParts => _heavyWeaponParts;
    }
}