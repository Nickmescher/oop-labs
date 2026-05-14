using Lab3.Entities;

namespace Lab3.Creatures;

public class MimicChest : Creature
{
    public MimicChest() : base(1, 1) { }

    private MimicChest(int attack, int health) : base(attack, health) { }

    public override void PerformAttack(Creature target)
    {
        int attackDelta = Math.Max(0, target.Attack - Attack);
        int healthDelta = Math.Max(0, target.Health - Health);
        ModifyAttack(attackDelta);
        ModifyHealth(healthDelta);
        base.PerformAttack(target);
    }

    public override Creature Clone() => new MimicChest(Attack, Health);
}
