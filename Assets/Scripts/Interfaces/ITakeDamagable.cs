public interface ITakeDamagable
{
    float Health { get; }
    void TakeDamage(int damage);
}