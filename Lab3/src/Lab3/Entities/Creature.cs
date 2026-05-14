namespace Lab3.Entities;

public class Creature
{
    public int Attack { get; protected set; }
    public int Health { get; protected set; }

    private bool _magicShieldActive;
    private bool _attackMasteryActive;

    public Creature(int attack, int health)
    {
        Attack = attack;
        Health = health;
    }

    public void ModifyAttack(int delta) => Attack += delta;

    public void ModifyHealth(int delta) => Health += delta;

    public void EnableMagicShield() => _magicShieldActive = true;

    public void EnableAttackMastery() => _attackMasteryActive = true;

    public virtual void PerformAttack(Creature target)
    {
        target.TakeDamage(Attack);

        if (_attackMasteryActive && Health > 0)
            target.TakeDamage(Attack);
    }

    public virtual void TakeDamage(int damage)
    {
        if (_magicShieldActive)
        {
            _magicShieldActive = false;
            return;
        }

        Health -= damage;
    }

    public virtual Creature Clone() => new Creature(Attack, Health);
}
