using Lab3.Entities;

namespace Lab3.Modifiers;

public class AttackMasteryApplicator : IModifierApplicator<Creature>
{
    public void Apply(Creature creature) => creature.EnableAttackMastery();
}
