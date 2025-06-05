namespace TilesMath.Pixels;

public class TilePixels
{
    private readonly double _left;
    private readonly double _bottom;
    private readonly double _latPixelSize;
    private readonly double _lonPixelSize;

    internal TilePixels(Tile tile, int resolution)
    {
        var b = tile.Boundaries;
        _left = b.Left;
        _bottom = b.Bottom;

        _latPixelSize = b.HeightLatitude / resolution;
        _lonPixelSize = b.WidthLongitude / resolution;
    }

    public (int x, int y) PixelFor(double longitude, double latitude)
    {
        var l = longitude - _left;
        var b = latitude - _bottom;

        return ((int)Math.Round(l / _lonPixelSize), (int)Math.Round(b / _latPixelSize));
    }

    public (double longitude, double latitude) CoordinatesAt(int x, int y)
    {
        return (_left + (x * _lonPixelSize), (_bottom + (y * _latPixelSize)));
    }
}
