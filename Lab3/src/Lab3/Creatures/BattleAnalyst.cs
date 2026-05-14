using Lab3.Entities;

namespace Lab3.Creatures;

public class BattleAnalyst : Creature
{
    public BattleAnalyst() : base(2, 4) { }

    private BattleAnalyst(int attack, int health) : base(attack, health) { }

    public override void PerformAttack(Creature target)
    {
        ModifyAttack(2);
        base.PerformAttack(target);
    }

    public override Creature Clone() => new BattleAnalyst(Attack, Health);
}
