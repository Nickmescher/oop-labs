using Lab3.Entities;

namespace Lab3.Modifiers;

public interface IModifierApplicator<in TCreature> where TCreature : Creature
{
    void Apply(TCreature creature);
}
