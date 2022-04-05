namespace TilesMath;

internal static class LocalTileId
{
    internal static (int x, int y) From(int localId, int zoom)
    {
        var xMax = 1 << zoom;

        var x = localId % xMax;
        var y = localId / xMax;
        return (x, y);
    }

    internal static int Max(int zoom)
    {
        var xMax = 1 << zoom;

        return xMax * xMax;
    }
    
    internal static int ForTile(Tile tile)
    {
        var xMax = 1 << (int)tile.Zoom;

        return tile.Y * xMax + tile.X;
    }
}