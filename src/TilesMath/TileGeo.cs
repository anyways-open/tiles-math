namespace TilesMath;

internal static class TileGeo
{
    private const double MaxLat = 85.0511;
    private const double MinLat = -85.0511;
    private const double MinLon = -180;
    private const double MaxLon = 180;

    public static (int x, int y)? TryForLocation(double longitude, double latitude, int zoom)
    {
        if (latitude is > MaxLat or < MinLat) return null;
        if (longitude is > MaxLon or < MinLon) return null;

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (longitude == 180) longitude -= 0.000001;

        var x = (int)((longitude + 180.0) / 360.0 * (1 << zoom));
        var latRad = latitude * Math.PI / 180.0;
        var y = (int)((1.0 - Math.Log(Math.Tan(latRad) +
                                       1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom));

        return (x, y);
    }

    public static (int x, int y) ForLocation(double longitude, double latitude, int zoom)
    {
        if (latitude is > MaxLat or < MinLat) throw new ArgumentOutOfRangeException(nameof(latitude));
        if (longitude is > MaxLon or < MinLon) throw new ArgumentOutOfRangeException(nameof(longitude));

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (longitude == 180) longitude -= 0.000001;

        var x = (int)((longitude + 180.0) / 360.0 * (1 << zoom));
        var latRad = latitude * Math.PI / 180.0;
        var y = (int)((1.0 - Math.Log(Math.Tan(latRad) +
                                   1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom));

        return (x, y);
    }

    public static TileBounds BoundariesFor(Tile tile)
    {
        var size = (double)(1 << tile.Zoom);
        var n = Math.PI - ((2.0 * Math.PI * tile.Y) / size);

        var left = ((tile.X / size * 360.0) - 180.0);
        var top = (180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

        n = Math.PI - ((2.0 * Math.PI * (tile.Y + 1)) / size);
        var right = (((tile.X + 1) / size * 360.0) - 180.0);
        var bottom = (180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

        return new TileBounds(left, top, right, bottom);
    }
}
