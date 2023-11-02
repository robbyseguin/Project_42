using Entities;
using UnityEngine;

namespace Datas
{
    
    [CreateAssetMenu(menuName = "Project 42/New Entities Data", fileName = "ED_Default")]
    public class EntitiesData : ScriptableObject
    {
        [SerializeField] private Entity _playerPrefab;
        [SerializeField] private Entity _aiPrefab;
        [SerializeField] private Entity _dummyPrefab;

        public Entity PlayerPrefab => _playerPrefab;
        public Entity AiPrefab => _aiPrefab;
        public Entity DummyPrefab => _dummyPrefab;
    }
}