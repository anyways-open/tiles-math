using System.Runtime.CompilerServices;

namespace TilesMath;

/// <summary>
/// Represents a tile.
/// </summary>
public readonly struct Tile
{
    private Tile(int x, int y, byte zoom)
    {
        this.X = x;
        this.Y = y;
        this.Zoom = zoom;
    }

    /// <summary>
    /// The x-coordinate.
    /// </summary>
    public int X { get; } = -1;

    /// <summary>
    /// The y-coordinate.
    /// </summary>
    public int Y { get; } = -1;

    /// <summary>
    /// The zoom-level.
    /// </summary>
    public byte Zoom { get; }

    /// <summary>
    /// Returns true if this tile is empty.
    /// </summary>
    public bool IsEmpty => this.X == -1 || this.Y == -1;

    /// <summary>
    /// The local id.
    /// </summary>
    public int LocalId => LocalTileId.ForTile(this);

    /// <summary>
    /// The global id.
    /// </summary>
    public long GlobalId => GlobalTileId.ForTile(this);

    /// <summary>
    /// The tile boundaries.
    /// </summary>
    public TileBounds Boundaries => TileGeo.BoundariesFor(this);
    
    /// <summary>
    /// The neighbours.
    /// </summary>
    public TileNeighbours Neighbours => new TileNeighbours(this);

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
    /// Creates an empty tile.
    /// </summary>
    public static Tile Empty = new Tile(-1, -1, 0);
}