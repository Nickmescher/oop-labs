using Lab3.Entities;

namespace Lab3.Spells;

public class PowerPotion : ISpell
{
    public void Apply(Creature creature) => creature.ModifyAttack(5);
}
