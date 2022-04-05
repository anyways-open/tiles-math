namespace TilesMath;

/// <summary>
/// Contains extensions for tiles.
/// </summary>
public static class TileExtensions
{
    /// <summary>
    /// Enumerates all tiles between the top left and bottom right tile.
    /// </summary>
    /// <param name="topLeft">The top left tile.</param>
    /// <param name="bottomRight">The bottom right tile.</param>
    /// <returns>The tiles between, including the two tiles defining the range.</returns>
    public static IEnumerable<Tile> EnumerateBetween(this Tile topLeft, Tile bottomRight)
    {
        if (topLeft.Zoom != bottomRight.Zoom) throw new Exception("can only enumerate within on zoom level");
        if (topLeft.Y > bottomRight.Y) throw new Exception("top tile is below bottom tile");

        IEnumerable<int> EnumerateX()
        {
            if (topLeft.X > bottomRight.X)
            {
                var xMax = 1 << topLeft.Zoom;
                for (var x = topLeft.X; x < xMax; x++)
                {
                    yield return x;
                }

                for (var x = 0; x <= bottomRight.X; x++)
                {
                    yield return x;
                }

                yield break;
            }

            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                yield return x;
            }
        }

        foreach (var x in EnumerateX())
            for (var y = topLeft.Y; y <= bottomRight.Y; y++)
            {
                yield return Tile.Create(x, y, topLeft.Zoom);
            }
    }
}