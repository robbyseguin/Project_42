using UnityEngine;

namespace Entities.Parts.LightWeapons
{
    public class ProjectilesHandler : MonoBehaviour
    {
        [SerializeField] private WeaponPart _part;
        //public float Damage { private get; set; }

        
        private void OnParticleCollision(GameObject other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
                return;
            
            if((damageable as Entity)?.IsPlayer == _part.IsPlayer)
                return;
            
            damageable.TakeDamage(Mathf.CeilToInt(_part.Damage));
        }
    }
}