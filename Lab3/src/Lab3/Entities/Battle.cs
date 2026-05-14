namespace Lab3.Entities;

public enum BattleOutcome { Player1Wins, Player2Wins, Draw }

public class Battle
{
    public BattleOutcome Conduct(PlayerBoard board1, PlayerBoard board2)
    {
        List<Creature> side1 = board1.CloneCreaturesForBattle().ToList();
        List<Creature> side2 = board2.CloneCreaturesForBattle().ToList();

        bool isPlayer1Turn = true;

        while (true)
        {
            List<Creature> attackers = isPlayer1Turn ? side1 : side2;
            List<Creature> defenders = isPlayer1Turn ? side2 : side1;

            Creature? attacker = attackers.FirstOrDefault(c => c.Health > 0 && c.Attack > 0);
            Creature? target = defenders.FirstOrDefault(c => c.Health > 0);

            if (attacker == null && target == null)
                return BattleOutcome.Draw;

            if (attacker != null && target == null)
                return isPlayer1Turn ? BattleOutcome.Player1Wins : BattleOutcome.Player2Wins;

            if (attacker == null)
            {
                isPlayer1Turn = !isPlayer1Turn;
                continue;
            }

            attacker.PerformAttack(target!);
            isPlayer1Turn = !isPlayer1Turn;
        }
    }
}
