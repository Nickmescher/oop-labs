using Lab3.Entities;

namespace Lab3.Creatures;

public class AngryFighter : Creature
{
    public AngryFighter() : base(1, 6) { }

    private AngryFighter(int attack, int health) : base(attack, health) { }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (Health > 0)
            ModifyAttack(Attack);
    }

    public override Creature Clone() => new AngryFighter(Attack, Health);
}
