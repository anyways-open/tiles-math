using System.Collections;

namespace TilesMath.Collections;

/// <summary>
/// A tile tree set that keeps a collection of tiles using leaf tiles. If all children of a tile are included only the parent is stored.
/// </summary>
public class TileTreeSet : IEnumerable<Tile>
{
    private readonly HashSet<Tile> _tiles = new();

    /// <summary>
    /// Adds the given tile.
    /// </summary>
    /// <param name="tile">The tile.</param>
    /// <returns>True if the tile was added, false if it was already present.</returns>
    public bool Add(Tile tile)
    {
        while (true)
        {
            if (_tiles.Contains(tile)) return false;

            // add the tile.
            _tiles.Add(tile);

            // check if this leads to a new 'leaf'.
            var parent = tile.Parent;
            if (parent == null)
            {
                // tile was added and it is now a new leaf.
                // this is the top level tile that was added.
                return true;
            }
            var hasAllChildren = parent.Value.Children.All(x => _tiles.Contains(x));
            if (!hasAllChildren)
            {
                // tile was added and it is now a new leaf.
                return true;
            }
            else
            {
                // remove all the children, the leaf will cover them.
                _tiles.ExceptWith(parent.Value.Children);

                // the parent needs to be added.
                tile = parent.Value;
            }
        }
    }

    /// <summary>
    /// Checks if a tile is covered by this tree.
    /// </summary>
    /// <param name="tile">The tile.</param>
    /// <returns>True if the tile is covered, false otherwise.</returns>
    public bool Contains(Tile tile)
    {
        return this.ContainsInternal(tile) != null;
    }

    private Tile? ContainsInternal(Tile tile)
    {
        while (true)
        {
            if (_tiles.Contains(tile)) return tile;

            // check parent.
            var parent = tile.Parent;
            if (parent == null) return null;

            tile = parent.Value;
        }
    }

    /// <summary>
    /// Removes a tile from the set.
    /// </summary>
    /// <param name="tile">The tile to remove.</param>
    /// <returns>True if the tile was removed, false if not.</returns>
    public bool Remove(Tile tile)
    {
        if (_tiles.Remove(tile)) return true;

        // find the parent that is there.
        var parent = this.ContainsInternal(tile);

        // compose blacklist of the entire parent queue.
        if (parent == null) return false; // tile is not in this set, no need to remove it.
        _tiles.Remove(parent.Value); // we are already sure this tile is not a leaf anymore.

        // add all new leaves one by one.
        _tiles.UnionWith(EnumerateTreeExceptAncestors(parent.Value));
        return true;

        IEnumerable<Tile> EnumerateTreeExceptAncestors(Tile p)
        {
            foreach (var child in p.Children)
            {
                if (child == tile) continue; // the tile itself we do not want to add again.
                if (child.IsAncestor(tile))
                {
                    // we do not want to add this ancestor again, but perhaps the children.
                    foreach (var grandChild in EnumerateTreeExceptAncestors(child))
                    {
                        yield return grandChild;
                    }
                }
                else
                {
                    yield return child;
                }
            }
        }
    }

    /// <summary>
    /// True if the set is empty.
    /// </summary>
    public bool IsEmpty => _tiles.Count == 0;

    /// <summary>
    /// Gets the inverted set.
    /// </summary>
    /// <returns></returns>
    public TileTreeSet GetInvertedSet()
    {
        // we start full and just remove all tiles in this set.
        var invertedSet = new TileTreeSet() { Tile.Create(0, 0, 0) };
        foreach (var tile in _tiles)
        {
            invertedSet.Remove(tile);
        }

        return invertedSet;
    }

    public IEnumerator<Tile> GetEnumerator()
    {
        return _tiles.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
