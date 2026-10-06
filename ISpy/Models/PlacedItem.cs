namespace ISpy.Models;

/// <summary>
/// One placed picture inside the jumbled scene. Coordinates are normalized
/// (0..1) relative to the scene canvas so the layout survives any screen size.
/// </summary>
public sealed class PlacedItem
{
    public required SpyItem Item { get; init; }

    /// <summary>Left edge, 0..1.</summary>
    public required double X { get; init; }

    /// <summary>Top edge, 0..1.</summary>
    public required double Y { get; init; }

    /// <summary>Width as a fraction of the canvas width, 0..1.</summary>
    public required double W { get; init; }

    /// <summary>Height as a fraction of the canvas height, 0..1.</summary>
    public required double H { get; init; }

    /// <summary>Rotation in degrees, so the scene looks jumbled.</summary>
    public required double Rotation { get; init; }

    /// <summary>True once the player has found and tapped this picture.</summary>
    public bool Found { get; set; }

    /// <summary>Draw order. Higher values sit on top.</summary>
    public int ZIndex { get; init; }
}
