using Lab3.Creatures;

namespace Lab3.Catalog;

public class CreatureCatalog
{
    public ICreatureConfigurator BattleAnalyst() => new CreatureConfigurator<BattleAnalyst>();

    public ICreatureConfigurator AngryFighter() => new CreatureConfigurator<AngryFighter>();

    public ICreatureConfigurator MimicChest() => new CreatureConfigurator<MimicChest>();

    public ICreatureConfigurator ImmortalHorror() => new CreatureConfigurator<ImmortalHorror>();

    public ICreatureConfigurator AmuletMaster()
        => new CreatureConfigurator<AmuletMaster>()
            .WithMagicShield()
            .WithAttackMastery();
}
