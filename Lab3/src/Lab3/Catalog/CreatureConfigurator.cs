using Lab3.Entities;

namespace Lab3.Catalog;

public class CreatureConfigurator<TCreature> : ICreatureConfigurator
    where TCreature : Creature, new()
{
    private int _extraAttack;
    private int _extraHealth;
    private bool _magicShield;
    private bool _attackMastery;

    public ICreatureConfigurator WithExtraAttack(int delta)
    {
        _extraAttack += delta;
        return this;
    }

    public ICreatureConfigurator WithExtraHealth(int delta)
    {
        _extraHealth += delta;
        return this;
    }

    public ICreatureConfigurator WithMagicShield()
    {
        _magicShield = true;
        return this;
    }

    public ICreatureConfigurator WithAttackMastery()
    {
        _attackMastery = true;
        return this;
    }

    public Creature Build()
    {
        var creature = new TCreature();

        if (_extraAttack != 0)
            creature.ModifyAttack(_extraAttack);

        if (_extraHealth != 0)
            creature.ModifyHealth(_extraHealth);

        if (_magicShield)
            creature.EnableMagicShield();

        if (_attackMastery)
            creature.EnableAttackMastery();

        return creature;
    }
}
