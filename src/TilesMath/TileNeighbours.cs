using System.Collections;

namespace TilesMath;

/// <summary>
/// The tile neighbours.
/// </summary>
public class TileNeighbours : IEnumerable<Tile>
{
    private readonly Tile _tile;
    private readonly int _left;
    private readonly int _right;
    private readonly int? _top;
    private readonly int? _bottom;

    internal TileNeighbours(Tile tile)
    {
        _tile = tile;

        var last = (1 << _tile.Zoom) - 1;
        _left = tile.X == 0 ? last : tile.X - 1;
        _right = tile.X == last ? 0 : tile.X + 1;
        _bottom = tile.Y == last ? null : tile.Y + 1;
        _top = tile.Y == 0 ? null : tile.Y - 1;
    }

    /// <summary>
    /// Gets the left neighbour.
    /// </summary>
    public Tile Left => Tile.Create(_left, _tile.Y, _tile.Zoom);

    /// <summary>
    /// Gets the right neighbour.
    /// </summary>
    public Tile Right => Tile.Create(_right, _tile.Y, _tile.Zoom);

    /// <summary>
    /// Gets the bottom neighbour.
    /// </summary>
    public Tile? Bottom => _bottom == null ? null : Tile.Create(_tile.X, _bottom.Value, _tile.Zoom);

    /// <summary>
    /// Gets the top neighbour.
    /// </summary>
    public Tile? Top => _top == null ? null : Tile.Create(_tile.X, _top.Value, _tile.Zoom);

    /// <summary>
    /// Gets the top left neighbour.
    /// </summary>
    public Tile? TopLeft => _top == null ? null : Tile.Create(_left, _top.Value, _tile.Zoom);

    /// <summary>
    /// Gets the top right neighbour.
    /// </summary>
    public Tile? TopRight => _top == null ? null : Tile.Create(_right, _top.Value, _tile.Zoom);

    /// <summary>
    /// Gets the bottom neighbour.
    /// </summary>
    public Tile? BottomLeft => _bottom == null ? null : Tile.Create(_left, _bottom.Value, _tile.Zoom);

    /// <summary>
    /// Gets the bottom neighbour.
    /// </summary>
    public Tile? BottomRight => _bottom == null ? null : Tile.Create(_right, _bottom.Value, _tile.Zoom);

    private IEnumerable<Tile> Enumerate()
    {
        yield return this.Left;
        if (this.TopLeft != null) yield return this.TopLeft.Value;
        if (this.Top != null) yield return this.Top.Value;
        if (this.TopRight != null) yield return this.TopRight.Value;
        yield return this.Right;
        if (this.BottomRight != null) yield return this.BottomRight.Value;
        if (this.Bottom != null) yield return this.Bottom.Value;
        if (this.BottomLeft != null) yield return this.BottomLeft.Value;
    }

    /// <inheritdoc/>
    public IEnumerator<Tile> GetEnumerator()
    {
        return this.Enumerate().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
