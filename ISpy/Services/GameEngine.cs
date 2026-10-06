using ISpy.Models;

namespace ISpy.Services;

/// <summary>Outcome of tapping a picture in the scene.</summary>
public enum TapResult
{
    /// <summary>The tapped picture was one of the items to find.</summary>
    Found,

    /// <summary>That item had already been found.</summary>
    AlreadyFound,

    /// <summary>The picture is a decoy and is not on the checklist.</summary>
    NotOnList,

    /// <summary>The round was already finished.</summary>
    RoundComplete,
}

/// <summary>
/// The pure game rules, free of any UI framework types so it can be tested
/// headlessly. A round is a jumbled scene plus the list of items to find.
/// </summary>
public sealed class GameEngine
{
    public const int DefaultTargetCount = 6;
    public const int DefaultDistractorCount = 6;

    private readonly SceneGenerator _generator;
    private readonly List<PlacedItem> _scene = new();
    private readonly List<SpyItem> _targets = new();
    private readonly HashSet<string> _foundKeys = new();
    private int _roundNumber;

    public GameEngine(SceneGenerator? generator = null)
        => _generator = generator ?? new SceneGenerator();

    public IReadOnlyList<PlacedItem> Scene => _scene;

    /// <summary>The items the player must find this round.</summary>
    public IReadOnlyList<SpyItem> Targets => _targets;

    public int RoundNumber => _roundNumber;
    public int TotalCount => _targets.Count;
    public int FoundCount => _foundKeys.Count;
    public int RemainingCount => TotalCount - FoundCount;
    public bool IsRoundComplete => TotalCount > 0 && RemainingCount == 0;

    public bool IsFound(SpyItem item) => _foundKeys.Contains(item.Key);

    /// <summary>Starts a fresh round and returns the newly built scene.</summary>
    public IReadOnlyList<PlacedItem> NewRound(
        int targetCount = DefaultTargetCount,
        int distractorCount = DefaultDistractorCount)
    {
        var (scene, targets) = _generator.CreateRound(targetCount, distractorCount);

        _scene.Clear();
        _scene.AddRange(scene);

        _targets.Clear();
        _targets.AddRange(targets);

        _foundKeys.Clear();
        _roundNumber++;
        return _scene;
    }

    /// <summary>
    /// Applies a tap on the picture identified by <paramref name="key"/>.
    /// Decoys are harmless; targets are ticked off once.
    /// </summary>
    public TapResult Tap(string key)
    {
        if (IsRoundComplete)
            return TapResult.RoundComplete;

        var target = _targets.FirstOrDefault(t => t.Key == key);
        if (target is null)
            return TapResult.NotOnList;

        if (!_foundKeys.Add(key))
            return TapResult.AlreadyFound;

        return TapResult.Found;
    }
}
