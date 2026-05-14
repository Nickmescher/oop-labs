using Lab3.Entities;

namespace Lab3.Modifiers;

public class MagicShieldApplicator : IModifierApplicator<Creature>
{
    public void Apply(Creature creature) => creature.EnableMagicShield();
}
