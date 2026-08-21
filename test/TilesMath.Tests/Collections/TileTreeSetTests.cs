using TilesMath.Collections;

namespace TilesMath.Tests.Collections;

public class TileTreeSetTests
{
    [Fact]
    public void TileTreeSet_NewSet_ShouldBeEmpty()
    {
        var set = new TileTreeSet();

        Assert.True(set.IsEmpty);
    }

    [Fact]
    public void TileTreeSet_OneTile_ShouldNotBeEmpty()
    {
        var set = new TileTreeSet { Tile.Create(1025, 4511, 14) };

        Assert.False(set.IsEmpty);
    }

    [Fact]
    public void TileTreeSet_OneTile_ShouldEnumerateOneTile()
    {
        var set = new TileTreeSet { Tile.Create(1025, 4511, 14) };

        var leaves = set.ToList();
        Assert.Single(leaves);
        Assert.Equal(Tile.Create(1025, 4511, 14), leaves[0]);
    }

    [Fact]
    public void TileTreeSet_AllChildren_OneZoomLower_ShouldEnumerateOneLeaf()
    {
        var set = new TileTreeSet();
        var expectedLeaf = Tile.Create(102, 451, 13);
        foreach (var tile in expectedLeaf.Children)
        {
            set.Add(tile);
        }

        var leaves = set.ToList();
        Assert.Single(leaves);
        Assert.Equal(expectedLeaf, leaves[0]);
    }

    [Fact]
    public void TileTreeSet_AllChildren_ThreeZoomsLower_ShouldEnumerateOneLeaf()
    {
        var set = new TileTreeSet();
        var expectedLeaf = Tile.Create(2, 4, 4);
        foreach (var tile in expectedLeaf.ChildrenAtZoom(7))
        {
            set.Add(tile);
        }

        var leaves = set.ToList();
        Assert.Single(leaves);
        Assert.Equal(expectedLeaf, leaves[0]);
    }

    [Fact]
    public void TileTreeSet_SetWithTileZero_ShouldContainAllTiles()
    {
        var set = new TileTreeSet { Tile.Create(0, 0, 0) };

        Assert.True(set.Contains(Tile.Create(2, 4, 4)));
        Assert.True(set.Contains(Tile.Create(102, 451, 13)));
        Assert.True(set.Contains(Tile.Create(1025, 4511, 14)));
    }

    [Fact]
    public void TileTreeSet_SetWithTileZero_RemoveChildTile_ShouldContainAllExceptRemovedTile()
    {
        var set = new TileTreeSet { Tile.Create(0, 0, 0) };

        var removedTile = Tile.Create(1, 1, 1);
        set.Remove(removedTile);

        Assert.True(set.Contains(Tile.Create(0, 0, 1)));
        Assert.True(set.Contains(Tile.Create(1, 0, 1)));
        Assert.True(set.Contains(Tile.Create(0, 1, 1)));
        Assert.False(set.Contains(removedTile));
    }

    [Fact]
    public void TileTreeSet_SetWithTileZero_RemoveGranChildTile_ShouldContainAllExceptRemovedTile()
    {
        var set = new TileTreeSet { Tile.Create(0, 0, 0) };

        var removedTile = Tile.Create(2, 2, 2);
        set.Remove(removedTile);

        Assert.True(set.Contains(Tile.Create(0, 0, 1)));
        Assert.True(set.Contains(Tile.Create(1, 0, 1)));
        Assert.True(set.Contains(Tile.Create(0, 1, 1)));
        foreach (var leaf in removedTile.Parent.Value.Children.Where(x => x != removedTile))
        {
            Assert.True(set.Contains(leaf));
        }
        Assert.False(set.Contains(removedTile));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void TileTreeSet_AllChildrenExceptOne_ShouldNotCollapse(int missingChild)
    {
        var parent = Tile.Create(102, 451, 13);
        var children = parent.Children.ToList();

        var set = new TileTreeSet();
        for (var c = 0; c < children.Count; c++)
        {
            if (c == missingChild) continue;
            set.Add(children[c]);
        }

        // one child short of a full set, so nothing may collapse: the three added children
        // are still the leaves and the parent must not be covering anything.
        var leaves = set.ToList();
        Assert.Equal(3, leaves.Count);
        Assert.DoesNotContain(parent, leaves);
        Assert.False(set.Contains(children[missingChild]));
        for (var c = 0; c < children.Count; c++)
        {
            if (c == missingChild) continue;
            Assert.Contains(children[c], leaves);
        }
    }

    [Fact]
    public void TileTreeSet_AllChildren_AddedInReverse_ShouldEnumerateOneLeaf()
    {
        var set = new TileTreeSet();
        var expectedLeaf = Tile.Create(102, 451, 13);
        foreach (var tile in expectedLeaf.Children.Reverse())
        {
            set.Add(tile);
        }

        var leaves = set.ToList();
        Assert.Single(leaves);
        Assert.Equal(expectedLeaf, leaves[0]);
    }

    [Fact]
    public void TileTreeSet_Add_ShouldNotAllocate()
    {
        // spread the tiles out so no four of them are ever siblings: every Add then takes the
        // 'parent does not have all its children' path, which is where Add spends virtually all
        // of its time on a real set and the path that used to allocate a closure, a boxed
        // TileChildren and an iterator state machine on every single call.
        var tiles = new List<Tile>();
        for (var t = 0; t < 4096; t++)
        {
            tiles.Add(Tile.Create(t * 4, t * 4, 14));
        }

        // fill once so the underlying hashset grows to capacity, then empty it again. A hashset
        // keeps its capacity across Remove, so the measured pass below cannot be resizing and
        // any allocation it reports comes from Add itself.
        var set = new TileTreeSet();
        foreach (var tile in tiles) set.Add(tile);
        foreach (var tile in tiles) set.Remove(tile);
        Assert.True(set.IsEmpty);

        // jit every path the measured pass will walk.
        var warmup = new TileTreeSet();
        foreach (var tile in tiles) warmup.Add(tile);

        var before = GC.GetAllocatedBytesForCurrentThread();
        foreach (var tile in tiles) set.Add(tile);
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.Equal(0, allocated);
    }
}
