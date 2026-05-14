using Lab3.Entities;

namespace Lab3.Spells;

public class EndurancePotion : ISpell
{
    public void Apply(Creature creature) => creature.ModifyHealth(5);
}
