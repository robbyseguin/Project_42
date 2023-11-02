using System;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Parts.HeavyWeapons.Ammo
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionPrefab;

        private ParticleSystem _explosion;
        
        private void Awake()
        {
            _explosion = Instantiate(_explosionPrefab, transform.parent).GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            _explosion.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _explosion.Stop(true);
        }

        private void OnDisable()
        {
            _explosion.gameObject.SetActive(true);
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit target, 5f, NavMesh.AllAreas))
                _explosion.transform.position = target.position;
            else
                _explosion.transform.position = transform.position;
            _explosion.Play();
            
            Invoke(nameof(StopExplosion), 1f);
        }

        private void StopExplosion() => _explosion.Stop(true);
    }
}