namespace TilesMath;

internal static class GlobalTileId
{
    private static long ForZoom(int zoom)
    {
        switch (zoom)
        {
            case 0:
                // zoom level 0: {0}.
                return 0;
            case 1:
                return 1;
            case 2:
                return 5;
            case 3:
                return 21;
            case 4:
                return 85;
            case 5:
                return 341;
            case 6:
                return 1365;
            case 7:
                return 5461;
            case 8:
                return 21845;
            case 9:
                return 87381;
            case 10:
                return 349525;
            case 11:
                return 1398101;
            case 12:
                return 5592405;
            case 13:
                return 22369621;
            case 14:
                return 89478485;
            case 15:
                return 357913941;
            case 16:
                return 1431655765;
            case 17:
                return 5726623061;
            case 18:
                return 22906492245;
        }

        var xMax = 1 << zoom;
        var tileId = ForZoom(zoom - 1) + xMax;
        return tileId;
    }
    
    internal static (int x, int y, int zoom) From(long globalTileId)
    {
        // find out the zoom level first.
        var zoom = 0;
        if (globalTileId > 0)
        {
            // only if the id is at least at zoom level 1.
            while (globalTileId >= ForZoom(zoom))
            {
                // move to the next zoom level and keep searching.
                zoom++;
            }

            zoom--;
        }

        // calculate the x-y.
        var local = globalTileId - ForZoom(zoom);
        var width = 1 << zoom;
        var x = (int)(local % width);
        var y = (int)(local / width);

        return (x, y, zoom);
    }

    internal static long ForTile(Tile tile)
    {
        var xMax = 1 << tile.Zoom;

        return ForZoom(tile.Zoom) + tile.Y * xMax + tile.X;
    }
}