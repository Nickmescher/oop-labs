using Lab3.Entities;

namespace Lab3.Catalog;

public interface ICreatureConfigurator
{
    ICreatureConfigurator WithExtraAttack(int delta);
    ICreatureConfigurator WithExtraHealth(int delta);
    ICreatureConfigurator WithMagicShield();
    ICreatureConfigurator WithAttackMastery();
    Creature Build();
}
