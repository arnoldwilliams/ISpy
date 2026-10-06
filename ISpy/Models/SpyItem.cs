namespace ISpy.Models;

/// <summary>
/// A single item picture that can appear in a scene and on the checklist.
/// </summary>
/// <param name="Key">File name (without extension) inside Resources/Images.</param>
/// <param name="Name">Human readable label shown to the player.</param>
/// <param name="PixelWidth">Intrinsic width of the bundled art.</param>
/// <param name="PixelHeight">Intrinsic height of the bundled art.</param>
public sealed record SpyItem(string Key, string Name, int PixelWidth, int PixelHeight)
{
    public string ImageFile => $"{Key}.png";

    public double AspectRatio => PixelHeight == 0 ? 1.0 : (double)PixelWidth / PixelHeight;
}
