using Levels.Interactable;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(menuName = "Project 42/New Loot Data", fileName = "LD_Default")]
    public class LootData : ScriptableObject
    {
        [SerializeField] private PowerUp[] _lootTable;
        public PowerUp[] LootTable => _lootTable;
    }
}