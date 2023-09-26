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

    internal static IEnumerable<Tile> EnumerableTilesForLine(IEnumerable<(double longitude, double latitude)> line,
        int zoom)
    {
        using var enumerator = line.GetEnumerator();
        var hasNext = enumerator.MoveNext();
        if (!hasNext) yield break;
        var point1 = enumerator.Current;
        hasNext = enumerator.MoveNext();

        Tile? previous = null;
        while (hasNext)
        {
            var segmentTiles = EnumerateTilesForLineSegment(point1, enumerator.Current, zoom);
            foreach (var segmentTile in segmentTiles)
            {
                if (previous == segmentTile) continue;

                previous = segmentTile;
                yield return segmentTile;
            }

            point1 = enumerator.Current;
            hasNext = enumerator.MoveNext();
        }
    }

    private static IEnumerable<Tile> EnumerateTilesForLineSegment((double longitude, double latitude) point1,
        (double longitude, double latitude) point2, int zoom)
    {
        var point1Tile = Tile.TryAtLocation(point1.longitude, point1.latitude, zoom);
        if (point1Tile == null) yield break;
        var point2Tile = Tile.TryAtLocation(point2.longitude, point2.latitude, zoom);
        if (point2Tile == null) yield break;

        // line equation: y = ax + b
        // a = slope
        // b = start

        var a = (point2.latitude - point1.latitude) / (point2.longitude - point1.longitude);
        var b = 0.0;

        var isMoreVertical = a > 0.5 || a < -0.5;
        if (isMoreVertical)
        {
            a = (point2.longitude - point1.longitude) / (point2.latitude - point1.latitude);
            b = point1.longitude - (a * point1.latitude);
        }
        else
        {
            b = point1.latitude - (a * point1.longitude);
        }

        var tileX = point1Tile.Value.X;
        var tileY = point1Tile.Value.Y;
        while (tileX != point2Tile.Value.X ||
               tileY != point2Tile.Value.Y)
        {
            var tile = Tile.Create(tileX, tileY, zoom);
            yield return tile;

            // determine to change x or y.

            // if y at the next x is inside the current tile range (miny, maxy), we step x.
            if (tileX != point2Tile.Value.X)
            {
                if (point2Tile.Value.X > tileX)
                {
                    // try to move right.
                    var nextLongitude = tile.Boundaries.Right;
                    var latitude = GetY(nextLongitude);

                    if (latitude <= tile.Boundaries.Top &&
                        latitude >= tile.Boundaries.Bottom)
                    {
                        tileX += 1;
                        continue;
                    }
                }
                if (point2Tile.Value.X < tileX)
                {
                    // try to move left.
                    var nextLongitude = tile.Boundaries.Left;
                    var latitude = GetY(nextLongitude);

                    if (latitude <= tile.Boundaries.Top &&
                        latitude >= tile.Boundaries.Bottom)
                    {
                        tileX -= 1;
                        continue;
                    }
                }
            }

            // if x at the next y is inside the current tile range (minx, maxx), we step y.
            if (tileY != point2Tile.Value.Y)
            {
                if (point2Tile.Value.Y > tileY)
                {
                    // try to move down.
                    var nextLatitude = tile.Boundaries.Bottom;
                    var longitude = GetX(nextLatitude);

                    if (longitude >= tile.Boundaries.Left &&
                        longitude <= tile.Boundaries.Right)
                    {
                        tileY += 1;
                        continue;
                    }
                }
                if (point2Tile.Value.Y < tileY)
                {
                    // try to move up.
                    var nextLatitude = tile.Boundaries.Top;
                    var longitude = GetX(nextLatitude);

                    if (longitude >= tile.Boundaries.Left &&
                        longitude <= tile.Boundaries.Right)
                    {
                        tileY -= 1;
                        continue;
                    }
                }
            }
        }

        yield return point2Tile.Value;
        yield break;

        double GetX(double y)
        {
            if (isMoreVertical) return a * y + b;
            return (y - b) / a;
        }

        double GetY(double x)
        {
            if (isMoreVertical) return (x - b) / a;
            return a * x + b;
        }
    }
}
