using System;
using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Entities.Parts.Heads
{
    public class ShieldHeadPart : HeadPart
    {
        public override PartIdentifier PartID => PartIdentifier.HEAD_PART_SHIELD;
        public override string Name => GetLocalizedString("Tête bouclier", "Shield head part");
        public override string Description => GetLocalizedString("Appuyez sur Q pour activer un bouclier dans le", 
            "Press Q to activate Shield");

        public override string[] Info => new[]
        { 
            "\n" + GetLocalizedString("Délai", "Delay") +"\n" +
            GetLocalizedString("Vie du bouclier", "Shield Health") + "\n", 
            "\n" + (Delay == -1 ? 0 : Delay.ToString("F1")) + "\n" +
            _shields[_currentLevel].Health.ToString("F1") + "\n"
            
        };

        private List<GameObject> _shieldLevels = new List<GameObject>();
        [SerializeField] private HealthComponent[] _shields;
        
        protected override void Awake()
        {
            base.Awake();

            Transform shields = transform.Find("Shields").transform;

            _shieldLevels.Clear();
            for (int i = 0; i < shields.childCount; i++)
            {
                _shieldLevels.Add(shields.GetChild(i).gameObject);
            }

            _shields = shields.GetComponentsInChildren<HealthComponent>(true);
        }

        public override void OnEquip(Entity entity, Transform mount)
        {
            foreach (var t in _powerUpThreshold)
            {
                if (_shields[0].Health >= t)
                {
                    _currentLevel++;
                }
            }
            
            base.OnEquip(entity, mount);
        }

        protected override void StartActiveAbility()
        {
            _shieldLevels[_currentLevel].SetActive(true);
        }

        private void OnDisable()
        {
            foreach (var shield in _shields)
            {
                shield.gameObject.SetActive(false);
            }
        }

        public override void UpdatePartPower(float multiplier = 1)
        {
            base.UpdatePartPower(multiplier);

            foreach (var shield in _shields)
            {
                shield.Init();
            }
        }
    }
}
