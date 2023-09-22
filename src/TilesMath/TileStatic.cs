namespace TilesMath;

public readonly partial struct Tile
{
    /// <summary>
    /// Creates a new tile from x-y coordinate and zoom level.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="zoom"></param>
    /// <returns></returns>
    public static Tile Create(int x, int y, int zoom)
    {
        return new Tile(x, y, (byte)zoom);
    }

    /// <summary>
    /// Creates a new tile from its global id.
    /// </summary>
    /// <param name="globalId">The global id.</param>
    /// <returns>The tile equivalent to the global id.</returns>
    public static Tile FromGlobalId(long globalId)
    {
        var (x, y, z) = GlobalTileId.From(globalId);

        return new Tile(x, y, (byte)z);
    }

    /// <summary>
    /// Creates a new tile from its local id.
    /// </summary>
    /// <param name="localId">The local id.</param>
    /// <param name="zoom"></param>
    /// <returns>The tile equivalent to for the local id at the given zoom level.</returns>
    public static Tile FromLocalId(int localId, int zoom)
    {
        var (x, y) = LocalTileId.From(localId, zoom);

        return new Tile(x, y, (byte)zoom);
    }

    /// <summary>
    /// Calculates the maximum local id for the given zoom level.
    /// </summary>
    /// <param name="zoom">The zoom level.</param>
    /// <returns>The maximum local id for the given zoom level</returns>
    public static int MaxLocalId(int zoom)
    {
        return LocalTileId.Max(zoom);
    }

    /// <summary>
    /// Creates the tile at the given WGS84 coordinates and zoom level.
    /// </summary>
    /// <param name="longitude">The longitude.</param>
    /// <param name="latitude">The latitude.</param>
    /// <param name="zoom">The zoom-level.</param>
    /// <returns>The tile at the given location and zoom level.</returns>
    public static Tile AtLocation(double longitude, double latitude, int zoom)
    {
        var (x, y) = TileGeo.ForLocation(longitude, latitude, zoom);

        return new Tile(x, y, (byte)zoom);
    }

    /// <summary>
    /// Creates the tile at the given WGS84 coordinates and zoom level.
    /// </summary>
    /// <param name="longitude">The longitude.</param>
    /// <param name="latitude">The latitude.</param>
    /// <param name="zoom">The zoom-level.</param>
    /// <returns>The tile at the given location and zoom level.</returns>
    public static Tile? TryAtLocation(double longitude, double latitude, int zoom)
    {
        var result = TileGeo.TryForLocation(longitude, latitude, zoom);
        if (result == null) return null;

        var (x, y) = result.Value;
        return new Tile(x, y, (byte)zoom);
    }

    /// <summary>
    /// Enumerates all the tiles below the line represented by the given coordinate sequence.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="zoom">The zoom level.</param>
    /// <returns>The tiles.</returns>
    public static IEnumerable<Tile> BelowLine(IEnumerable<(double longitude, double latitude)> line, int zoom)
    {
        return TileExtensions.EnumerableTilesForLine(line, zoom);
    }
}
