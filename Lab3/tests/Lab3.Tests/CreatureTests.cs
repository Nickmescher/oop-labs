using Lab3.Catalog;
using Lab3.Creatures;
using Lab3.Entities;
using Lab3.Modifiers;
using Lab3.Spells;
using Xunit;

namespace Lab3.Tests;

public class CreatureTests
{
    // Unit: BattleAnalyst gains +2 attack before each attack
    [Fact]
    public void BattleAnalyst_GainsTwoAttackBeforeEachAttack()
    {
        // Arrange
        var analyst = new BattleAnalyst();
        var target = new Creature(0, 100);

        // Act
        analyst.PerformAttack(target);

        // Assert
        Assert.Equal(4, analyst.Attack); // 2 + 2 = 4
        Assert.Equal(96, target.Health);  // 100 - 4 = 96
    }

    // Unit: AngryFighter doubles attack after receiving non-lethal damage
    [Fact]
    public void AngryFighter_DoublesAttackOnNonLethalDamage()
    {
        // Arrange
        var fighter = new AngryFighter();

        // Act
        fighter.TakeDamage(3);

        // Assert
        Assert.Equal(2, fighter.Attack); // 1 doubled = 2
        Assert.Equal(3, fighter.Health);
    }

    // Unit: AngryFighter doesn't double attack on lethal damage
    [Fact]
    public void AngryFighter_DoesNotDoubleAttackOnLethalDamage()
    {
        // Arrange
        var fighter = new AngryFighter();

        // Act
        fighter.TakeDamage(10);

        // Assert
        Assert.Equal(1, fighter.Attack);
        Assert.True(fighter.Health <= 0);
    }

    // Unit: MimicChest copies max stats before attacking
    [Fact]
    public void MimicChest_CopiesMaxStatsBeforeAttack()
    {
        // Arrange
        var mimic = new MimicChest(); // 1/1
        var target = new Creature(5, 2); // 5/2

        // Act
        mimic.PerformAttack(target);

        // Assert: before attack mimic becomes (5/1) since max(1,5)=5 for attack, max(1,2)=2 but target health<mimic health? No
        // max attack = max(1,5)=5, max health = max(1,2)=2 -> mimic becomes (5/2) before attacking
        Assert.Equal(5, mimic.Attack);
        Assert.Equal(2, mimic.Health);
    }

    // Unit: ImmortalHorror revives once with health=1
    [Fact]
    public void ImmortalHorror_RevivesOnceWithHealthOne()
    {
        // Arrange
        var horror = new ImmortalHorror(); // 4/4

        // Act
        horror.TakeDamage(10);

        // Assert
        Assert.Equal(1, horror.Health);
        Assert.Equal(4, horror.Attack);
    }

    // Unit: ImmortalHorror doesn't revive a second time
    [Fact]
    public void ImmortalHorror_DoesNotReviveTwice()
    {
        // Arrange
        var horror = new ImmortalHorror();
        horror.TakeDamage(10); // first revival

        // Act
        horror.TakeDamage(10); // lethal again

        // Assert
        Assert.True(horror.Health <= 0);
    }

    // Unit: MagicShield blocks first hit
    [Fact]
    public void MagicShield_BlocksFirstDamage()
    {
        // Arrange
        var creature = new Creature(0, 10);
        creature.EnableMagicShield();

        // Act
        creature.TakeDamage(5);

        // Assert
        Assert.Equal(10, creature.Health);
    }

    // Unit: MagicShield allows damage after being consumed
    [Fact]
    public void MagicShield_AllowsDamageAfterBeingConsumed()
    {
        // Arrange
        var creature = new Creature(0, 10);
        creature.EnableMagicShield();
        creature.TakeDamage(5); // consumes shield

        // Act
        creature.TakeDamage(3);

        // Assert
        Assert.Equal(7, creature.Health);
    }

    // Unit: AttackMastery attacks twice if creature survives first attack
    [Fact]
    public void AttackMastery_AttacksTwice()
    {
        // Arrange
        var creature = new Creature(2, 10);
        creature.EnableAttackMastery();
        var target = new Creature(0, 100);

        // Act
        creature.PerformAttack(target);

        // Assert
        Assert.Equal(96, target.Health); // 100 - 2 - 2
    }

    // Unit: PowerPotion increases attack by 5
    [Fact]
    public void PowerPotion_IncreasesAttackByFive()
    {
        // Arrange
        var creature = new Creature(3, 5);
        var potion = new PowerPotion();

        // Act
        potion.Apply(creature);

        // Assert
        Assert.Equal(8, creature.Attack);
    }

    // Unit: EndurancePotion increases health by 5
    [Fact]
    public void EndurancePotion_IncreasesHealthByFive()
    {
        // Arrange
        var creature = new Creature(3, 5);
        var potion = new EndurancePotion();

        // Act
        potion.Apply(creature);

        // Assert
        Assert.Equal(10, creature.Health);
    }

    // Unit: MagicMirror swaps attack and health
    [Fact]
    public void MagicMirror_SwapsAttackAndHealth()
    {
        // Arrange
        var creature = new Creature(3, 8);
        var mirror = new MagicMirror();

        // Act
        mirror.Apply(creature);

        // Assert
        Assert.Equal(8, creature.Attack);
        Assert.Equal(3, creature.Health);
    }

    // Unit: ProtectionAmulet applies magic shield
    [Fact]
    public void ProtectionAmulet_BlocksFirstDamage()
    {
        // Arrange
        var creature = new Creature(0, 10);
        var amulet = new ProtectionAmulet();

        // Act
        amulet.Apply(creature);
        creature.TakeDamage(5);

        // Assert
        Assert.Equal(10, creature.Health);
    }

    // Unit: PlayerBoard limits to 7 creatures
    [Fact]
    public void PlayerBoard_MaxSevenCreatures()
    {
        // Arrange
        var board = new PlayerBoard();
        for (int i = 0; i < 7; i++)
            board.TryAddCreature(new Creature(1, 1));

        // Act
        bool result = board.TryAddCreature(new Creature(1, 1));

        // Assert
        Assert.False(result);
        Assert.Equal(7, board.Creatures.Count);
    }

    // Functional: battle - one player wins
    [Fact]
    public void Battle_PlayerWithStrongerCreature_Wins()
    {
        // Arrange
        var board1 = new PlayerBoard();
        board1.TryAddCreature(new Creature(10, 10));

        var board2 = new PlayerBoard();
        board2.TryAddCreature(new Creature(1, 1));

        var battle = new Battle();

        // Act
        BattleOutcome outcome = battle.Conduct(board1, board2);

        // Assert
        Assert.Equal(BattleOutcome.Player1Wins, outcome);
    }

    // Functional: battle doesn't affect original board creatures
    [Fact]
    public void Battle_DoesNotModifyOriginalBoardCreatures()
    {
        // Arrange
        var creature = new Creature(5, 10);
        var board1 = new PlayerBoard();
        board1.TryAddCreature(creature);

        var board2 = new PlayerBoard();
        board2.TryAddCreature(new Creature(3, 5));

        var battle = new Battle();

        // Act
        battle.Conduct(board1, board2);

        // Assert
        Assert.Equal(10, creature.Health); // unchanged
    }

    // Functional: catalog creates correct creature types
    [Fact]
    public void CreatureCatalog_CreatesConfiguredCreature()
    {
        // Arrange
        var catalog = new CreatureCatalog();

        // Act
        Creature creature = catalog.BattleAnalyst().WithExtraAttack(3).Build();

        // Assert
        Assert.Equal(5, creature.Attack); // 2 base + 3 extra
        Assert.Equal(4, creature.Health);
    }

    // Functional: modifier applicator applies shield to any creature
    [Fact]
    public void MagicShieldApplicator_AppliesShieldToAnyCreature()
    {
        // Arrange
        var applicator = new MagicShieldApplicator();
        var creature = new Creature(1, 5);

        // Act
        applicator.Apply(creature);
        creature.TakeDamage(3);

        // Assert
        Assert.Equal(5, creature.Health);
    }
}
