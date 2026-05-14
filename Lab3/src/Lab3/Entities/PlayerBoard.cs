namespace Lab3.Entities;

public class PlayerBoard
{
    private const int MaxCreatures = 7;
    private readonly List<Creature> _creatures = new();

    public IReadOnlyList<Creature> Creatures => _creatures.AsReadOnly();

    public bool TryAddCreature(Creature creature)
    {
        if (_creatures.Count >= MaxCreatures) return false;
        _creatures.Add(creature);
        return true;
    }

    public Creature? FindAttacker()
        => _creatures.FirstOrDefault(c => c.Health > 0 && c.Attack > 0);

    public Creature? FindTarget()
        => _creatures.FirstOrDefault(c => c.Health > 0);

    public IReadOnlyList<Creature> CloneCreaturesForBattle()
        => _creatures.Select(c => c.Clone()).ToList();
}
