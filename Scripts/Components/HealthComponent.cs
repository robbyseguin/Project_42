using System;
using Entities;
using Managers;
using UnityEngine;

namespace Components
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _initialLife = 60;

        private int _calculatedHealth;

        public void Init()
        {
            _calculatedHealth = (int)(_initialLife * DifficultyManager.Instance.HealthMultiplier);
            Health = _calculatedHealth;
        }

        private void OnEnable()
        {
            Health = _calculatedHealth;
        }

        public int Health { get; private set; }
        public bool IsDead { get; private set; }

        public void TakeDamage(int hitPoints)
        {
            Health -= hitPoints;

            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            gameObject.SetActive(false);
            //BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        }

        public void Heal(int hitPoints)
        {
        }
    }
}