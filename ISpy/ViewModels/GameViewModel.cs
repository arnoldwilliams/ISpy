using System.Collections.ObjectModel;
using ISpy.Services;

namespace ISpy.ViewModels;

/// <summary>
/// Drives one game of I Spy. All rules live in <see cref="GameEngine"/>; this
/// type only maps them onto observable collections for the view.
/// </summary>
public sealed class GameViewModel : ObservableObject
{
    private readonly GameEngine _engine;
    private int _foundCount;
    private bool _isRoundComplete;
    private string _statusText = string.Empty;

    public GameViewModel(GameEngine? engine = null)
    {
        _engine = engine ?? new GameEngine();
        NewRoundCommand = new Command(NewRound);
        NewRound();
    }

    public ObservableCollection<SceneItemViewModel> Scene { get; } = new();
    public ObservableCollection<ChecklistItemViewModel> Checklist { get; } = new();

    public Command NewRoundCommand { get; }

    public int TotalCount => _engine.TotalCount;

    public int FoundCount
    {
        get => _foundCount;
        private set
        {
            if (SetProperty(ref _foundCount, value))
            {
                OnPropertyChanged(nameof(RemainingCount));
                OnPropertyChanged(nameof(ProgressText));
            }
        }
    }

    public int RemainingCount => TotalCount - FoundCount;

    public string ProgressText => $"{FoundCount} / {TotalCount}";

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public bool IsRoundComplete
    {
        get => _isRoundComplete;
        private set => SetProperty(ref _isRoundComplete, value);
    }

    public void NewRound()
    {
        _engine.NewRound();

        Scene.Clear();
        foreach (var placed in _engine.Scene)
            Scene.Add(new SceneItemViewModel(placed));

        Checklist.Clear();
        foreach (var target in _engine.Targets)
            Checklist.Add(new ChecklistItemViewModel(target));

        FoundCount = _engine.FoundCount;
        IsRoundComplete = _engine.IsRoundComplete;
        StatusText = $"Round {_engine.RoundNumber} — find {_engine.TotalCount} pictures!";
    }

    /// <summary>
    /// Called when the player taps a picture. Only the requested targets count;
    /// decoys give gentle feedback without penalty.
    /// </summary>
    public void TapItem(SceneItemViewModel tapped)
    {
        switch (_engine.Tap(tapped.Item.Key))
        {
            case TapResult.Found:
                tapped.Found = true;
                var card = Checklist.FirstOrDefault(c => c.Item.Key == tapped.Item.Key);
                if (card is not null)
                    card.Found = true;
                FoundCount = _engine.FoundCount;
                IsRoundComplete = _engine.IsRoundComplete;
                StatusText = IsRoundComplete
                    ? "You found them all! Well done."
                    : $"Found {tapped.Name}! {RemainingCount} to go.";
                break;

            case TapResult.NotOnList:
                StatusText = $"{tapped.Name} is not on the list. Keep looking!";
                break;

            case TapResult.AlreadyFound:
            case TapResult.RoundComplete:
            default:
                break;
        }
    }
}
