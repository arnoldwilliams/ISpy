using ISpy.Models;

namespace ISpy.Services;

/// <summary>
/// Builds the jumbled scene for a round: picks the items to find, then places
/// them at scattered, non-overlapping, rotated positions on the canvas.
/// </summary>
public sealed class SceneGenerator
{
    private readonly Random _random;

    public SceneGenerator(Random? random = null) => _random = random ?? Random.Shared;

    /// <summary>
    /// Creates a round with <paramref name="targetCount"/> items to find and
    /// <paramref name="distractorCount"/> extra decoy pictures to keep the
    /// scene busy. Positions are normalized to the 0..1 canvas.
    /// </summary>
    public (IReadOnlyList<PlacedItem> Scene, IReadOnlyList<SpyItem> Targets) CreateRound(
        int targetCount = 6,
        int distractorCount = 6)
    {
        var pool = ItemCatalog.All.OrderBy(_ => _random.Next()).ToList();
        var targets = pool.Take(targetCount).ToList();
        var distractors = pool.Skip(targetCount).Take(distractorCount).ToList();

        var toPlace = targets.Concat(distractors).ToList();
        var placed = PlaceAll(toPlace);

        // Targets keep a stable, readable order for the checklist.
        return (placed, targets);
    }

    private List<PlacedItem> PlaceAll(List<SpyItem> items)
    {
        // Roughly square cells keep the jumble readable on a phone in portrait.
        int columns = 3;
        int rows = (int)Math.Ceiling(items.Count / (double)columns);
        var cells = new List<(int Col, int Row)>();
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
                cells.Add((c, r));

        cells = cells.OrderBy(_ => _random.Next()).ToList();

        double cellW = 1.0 / columns;
        double cellH = 1.0 / rows;

        var result = new List<PlacedItem>();
        int z = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var (col, row) = cells[i];

            // Keep the picture inside its cell with a little breathing room.
            const double maxCellFill = 0.86;
            double cellPxW = cellW * maxCellFill;
            double cellPxH = cellH * maxCellFill;

            // Fit the art's aspect ratio inside the cell.
            double w = cellPxW;
            double h = w / item.AspectRatio;
            if (h > cellPxH)
            {
                h = cellPxH;
                w = h * item.AspectRatio;
            }

            double x = col * cellW + (cellW - w) / 2;
            double y = row * cellH + (cellH - h) / 2;

            // Small jitter so the grid does not look mechanical.
            double jitter = 0.02;
            x += (_random.NextDouble() * 2 - 1) * jitter;
            y += (_random.NextDouble() * 2 - 1) * jitter;
            x = Math.Clamp(x, 0.0, Math.Max(0.0, 1.0 - w));
            y = Math.Clamp(y, 0.0, Math.Max(0.0, 1.0 - h));

            double rotation = (_random.NextDouble() * 2 - 1) * 16.0;

            result.Add(new PlacedItem
            {
                Item = item,
                X = x,
                Y = y,
                W = w,
                H = h,
                Rotation = rotation,
                ZIndex = z++,
            });
        }

        return result;
    }
}
