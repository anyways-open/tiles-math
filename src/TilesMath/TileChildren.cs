using System.Collections;

namespace TilesMath;

public struct TileChildren : IEnumerable<Tile>
{
    private readonly int _x;
    private readonly int _y;
    private readonly int _z;
    
    internal TileChildren(Tile tile)
    {
        _x = tile.X * 2;
        _y = tile.Y * 2;
        _z = tile.Zoom + 1;
    }


    public IEnumerator<Tile> GetEnumerator()
    {
        yield return Tile.Create(_x, _y, _z);
        yield return Tile.Create(_x + 1, _y, _z);
        yield return Tile.Create(_x + 1, _y + 1, _z);
        yield return Tile.Create(_x, _y + 1, _z);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}