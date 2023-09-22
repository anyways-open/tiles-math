﻿using System.Runtime.CompilerServices;

namespace TilesMath;

/// <summary>
/// Represents a tile.
/// </summary>
public readonly partial struct Tile
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
    /// The parent tile, if any.
    /// </summary>
    public Tile? Parent
    {
        get
        {
            if (this.Zoom == 0) return null;

            return Tile.Create(this.X / 2, this.Y / 2, this.Zoom - 1);
        }
    }

    /// <summary>
    /// The children.
    /// </summary>
    public TileChildren Children => new TileChildren(this);

    public IEnumerable<Tile> ChildrenAtZoom(int zoom)
    {
        if (zoom < this.Zoom) throw new Exception("Cannot calculate sub tiles for a smaller zoom level");

        if (zoom == this.Zoom)
        {
            yield return this;
            yield break;
        }

        if (zoom - 1 == this.Zoom)
        {
            foreach (var child in this.Children)
            {
                yield return child;
            }
            yield break;
        }

        foreach (var childOneLevelLess in this.ChildrenAtZoom(zoom - 1))
        {
            foreach (var child in childOneLevelLess.Children)
            {
                yield return child;
            }
        }
    }

    public bool Equals(Tile other)
    {
        return this.X == other.X && this.Y == other.Y && this.Zoom == other.Zoom;
    }

    public override bool Equals(object? obj)
    {
        return obj is Tile other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.X, this.Y, this.Zoom);
    }

    public static bool operator ==(Tile left, Tile right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Tile left, Tile right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Creates an empty tile.
    /// </summary>
    public static Tile Empty = new Tile(-1, -1, 0);

    public override string ToString()
    {
        return $"{nameof(this.X)}: {this.X}, {nameof(this.Y)}: {this.Y}, {nameof(this.Zoom)}: {this.Zoom}, {nameof(this.LocalId)}: {this.LocalId}, {nameof(this.GlobalId)}: {this.GlobalId}";
    }
}
