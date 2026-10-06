using ISpy.Models;

namespace ISpy.ViewModels;

/// <summary>An entry on the "find these" checklist.</summary>
public sealed class ChecklistItemViewModel : ObservableObject
{
    private bool _found;

    public ChecklistItemViewModel(SpyItem item) => Item = item;

    public SpyItem Item { get; }
    public string Name => Item.Name;
    public string ImageFile => Item.ImageFile;

    public bool Found
    {
        get => _found;
        set
        {
            if (SetProperty(ref _found, value))
                OnPropertyChanged(nameof(StatusGlyph));
        }
    }

    /// <summary>Check mark shown on the card once the picture is found.</summary>
    public string StatusGlyph => Found ? "✓" : "";
}
