using Datas;
using Entities;
using Levels.Sections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Levels.Interactable
{
    public class DestructibleObject : SectionComponent, IDamageable
    {
        [SerializeField] protected int _maxHealth = 50;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private SoundDefinition _soundDefinition;
        private AudioSource _audioSource;
        public int Health { get; private set; }
        public bool IsDead { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            Health = _maxHealth;
            _audioSource = GetComponent<AudioSource>();
        }

        public void TakeDamage(int hitPoints)
        {
            Health -= hitPoints;
            _soundDefinition.PlayAsSFX(_audioSource, 0);
            
            float relativeDamage = Mathf.InverseLerp(0,_maxHealth / 10,hitPoints);
            Color c = Color.Lerp(Color.green, Color.red, relativeDamage);
            _particleSystem.EmitWords(hitPoints.ToString(), c);
            
            if (Health <= 0)
                Die();
        }

        public void Heal(int hitPoints)
        {
        }

        private void Die()
        {
            IsDead = true;
            OnDeath();
        }

        protected virtual void OnDeath()
        {
            gameObject.SetActive(false);
        }

        public override void OnSectionReset()
        {
            base.OnSectionReset();
            
            gameObject.SetActive(true);
            Health = _maxHealth;
            IsDead = false;
        }
    }
}