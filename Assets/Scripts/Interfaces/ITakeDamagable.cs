public interface ITakeDamagable
{
    int Health { get; }
    void TakeDamage(int damage);
}