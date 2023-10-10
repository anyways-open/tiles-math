namespace TilesMath.Collections;

public static class TileTreeSetExtensions
{
    /// <summary>
    /// Enumerates all the tiles in the set at the given zoom level.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <param name="zoom">The zoom level.</param>
    /// <returns>An enumerable with all tiles covered by the set in the given zoom level.</returns>
    /// <exception cref="Exception">When tiles are found at a higher zoom level in the set it is not possible to enumerate.</exception>
    public static IEnumerable<Tile> ToEnumerableAtZoom(this TileTreeSet set, int zoom)
    {
        foreach (var tile in set)
        {
            if (tile.Zoom == zoom)
            {
                yield return tile;
            }
            else if (tile.Zoom < zoom)
            {
                foreach (var child in tile.ChildrenAtZoom(zoom))
                {
                    yield return child;
                }
            }
            else
            {
                throw new Exception(
                    $"Cannot enumerate this set at {zoom}, found at tile at a higher zoom level: {tile.Zoom}");
            }
        }
    }

    /// <summary>
    /// Calculates the number of tiles at the given zoom level.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <param name="zoom">The zoom level.</param>
    /// <returns>The number of tiles the set represents at the given zoom.</returns>
    /// <exception cref="Exception">When tiles are found at a higher zoom level in the set it is not possible to enumerate.</exception>
    public static long CountAtZoom(this TileTreeSet set, int zoom)
    {
        var count = 0L;
        foreach (var tile in set)
        {
            if (tile.Zoom == zoom)
            {
                count++;
            }
            else if (tile.Zoom < zoom)
            {
                count += (long)System.Math.Pow(4, zoom - tile.Zoom);
            }
            else
            {
                throw new Exception(
                    $"Cannot enumerate this set at {zoom}, found at tile at a higher zoom level: {tile.Zoom}");
            }
        }

        return count;
    }
}
