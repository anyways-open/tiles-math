namespace TilesMath;

/// <summary>
/// Represents tile boundaries.
/// </summary>
public readonly struct TileBounds
{
    internal TileBounds(double left, double top, double right, double bottom)
    {
        this.Left = left;
        this.Top = top;
        this.Right = right;
        this.Bottom = bottom;
    }

    /// <summary>
    /// The longitude at the left of the tile
    /// </summary>
    public double Left { get; }

    /// <summary>
    /// The longitude at the right of the tile.
    /// </summary>
    public double Right { get; }

    /// <summary>
    /// The latitude at the top of the tile.
    /// </summary>
    public double Top { get; }

    /// <summary>
    /// The latitude at the bottom of the tile.
    /// </summary>
    public double Bottom { get; }

    /// <summary>
    /// The center latitude of the tile
    /// </summary>
    public double CenterLatitude => (this.Top + this.Bottom) / 2.0;

    /// <summary>
    /// The center longitude of the tile.
    /// </summary>
    public double CenterLongitude => (this.Left + this.Right) / 2.0;
}
