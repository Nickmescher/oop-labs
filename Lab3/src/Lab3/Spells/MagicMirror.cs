using Lab3.Entities;

namespace Lab3.Spells;

public class MagicMirror : ISpell
{
    public void Apply(Creature creature)
    {
        int originalAttack = creature.Attack;
        int originalHealth = creature.Health;
        creature.ModifyAttack(originalHealth - originalAttack);
        creature.ModifyHealth(originalAttack - originalHealth);
    }
}
