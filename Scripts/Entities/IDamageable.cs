namespace Entities
{
    public interface IDamageable
    {
        public int Health { get; }
        //public float resistance { get; }

        public bool IsDead { get; }

        public void TakeDamage(int hitPoints);
        public void Heal(int hitPoints);
    }
}
