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
}
