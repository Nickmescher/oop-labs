using Lab3.Entities;

namespace Lab3.Creatures;

public class ImmortalHorror : Creature
{
    private bool _hasRevived;

    public ImmortalHorror() : base(4, 4) { }

    private ImmortalHorror(int attack, int health, bool hasRevived) : base(attack, health)
    {
        _hasRevived = hasRevived;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Health <= 0 && !_hasRevived)
        {
            _hasRevived = true;
            Health = 1;
        }
    }

    public override Creature Clone() => new ImmortalHorror(Attack, Health, _hasRevived);
}
