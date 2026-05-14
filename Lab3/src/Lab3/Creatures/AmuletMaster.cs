using Lab3.Entities;

namespace Lab3.Creatures;

public class AmuletMaster : Creature
{
    public AmuletMaster() : base(5, 2)
    {
        EnableMagicShield();
        EnableAttackMastery();
    }

    private AmuletMaster(int attack, int health) : base(attack, health)
    {
        EnableMagicShield();
        EnableAttackMastery();
    }

    public override Creature Clone() => new AmuletMaster(Attack, Health);
}
