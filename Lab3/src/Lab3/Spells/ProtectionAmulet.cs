using Lab3.Entities;

namespace Lab3.Spells;

public class ProtectionAmulet : ISpell
{
    public void Apply(Creature creature) => creature.EnableMagicShield();
}
