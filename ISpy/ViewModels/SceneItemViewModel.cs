using ISpy.Models;

namespace ISpy.ViewModels;

/// <summary>
/// A picture drawn in the jumbled scene. Placement values are normalized to
/// the 0..1 canvas; the view converts them to absolute positions on layout.
/// </summary>
public sealed class SceneItemViewModel : ObservableObject
{
    private bool _found;

    public SceneItemViewModel(PlacedItem placed)
    {
        Placed = placed;
    }

    public PlacedItem Placed { get; }

    public SpyItem Item => Placed.Item;
    public string ImageFile => Placed.Item.ImageFile;
    public string Name => Placed.Item.Name;
    public double X => Placed.X;
    public double Y => Placed.Y;
    public double W => Placed.W;
    public double H => Placed.H;
    public double Rotation => Placed.Rotation;
    public int ZIndex => Placed.ZIndex;

    public bool Found
    {
        get => _found;
        set
        {
            if (SetProperty(ref _found, value))
            {
                Placed.Found = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
    }

    /// <summary>Found pictures fade back so they stop competing for attention.</summary>
    public double Opacity => Found ? 0.35 : 1.0;
}
