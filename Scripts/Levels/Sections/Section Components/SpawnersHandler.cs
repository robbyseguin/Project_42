using System.Collections;
using System.Linq;
using Entities.Parts;
using Levels.Sections;
using Managers;
using UnityEngine;
using Utility;

namespace SectionComponents
{
    public class SpawnersHandler : SectionComponent
    {
        [SerializeField] private PartIdentifier[] _partsSets;
        [SerializeField] private int _localSpawnLimiter = 5;
        [SerializeField] private float _secondInBetwen = 2.0f;
        [SerializeField] private AiEntitySpawner[] _spawners;

        private WaitForSeconds _wait;
        private Coroutine _spawning;

        public override void OnSectionInitialisation()
        {
            if (!_partsSets.Any())
                _partsSets = new PartIdentifier[] { 0 };
                
            _wait = new WaitForSeconds(_secondInBetwen / DifficultyManager.Instance.SpawnerMultiplier);
            _localSpawnLimiter = (int)(_localSpawnLimiter * DifficultyManager.Instance.SpawnerMultiplier);
        }
        
        public override void OnSectionStart()
        {
            if (_spawners.Any())
                _spawning = StartCoroutine(SpawnEntities());
        }

        public override void OnSectionReset()
        {
            
            if(_spawning != null)
                StopCoroutine(_spawning);

            foreach (AiEntitySpawner spawner in _spawners)
                spawner.KillRemaining();
            
            base.OnSectionReset();
        }

        public override void OnSectionBake()
        {
            base.OnSectionBake();
            
            _spawners = GetComponentsInChildren<AiEntitySpawner>();
        }

        private IEnumerator SpawnEntities()
        {
            for (int i = 0; i < _localSpawnLimiter; i++)
            {
                _spawners.RandomIndex().SpawnEntity(_partsSets.RandomIndex());
                yield return _wait;
            }
        }
    }
}