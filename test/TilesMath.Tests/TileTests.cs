namespace TilesMath.Tests;

public class TileTests
{
    [Fact]
    public void Tile_ParentAt_OneZoomUp_ShouldEqualParent()
    {
        var tile = Tile.Create(1025, 4511, 14);

        var parent = tile.Parent ?? throw new Exception("No parent");
        Assert.Equal(parent, tile.ParentAt(tile.Zoom - 1));
    }

    [Fact]
    public void Tile_ParentAt_TwoZoomsUp_ShouldEqualParentOfParent()
    {
        var tile = Tile.Create(1025, 4511, 14);

        var parent = tile.Parent?.Parent ?? throw new Exception("No parent");
        Assert.Equal(parent, tile.ParentAt(tile.Zoom - 2));
    }

    [Fact]
    public void Tile_ParentAt_5ZoomsUp_ShouldEqual5thParentUp()
    {
        var tile = Tile.Create(1025, 4511, 14);

        var parent = tile.Parent?.Parent?.Parent?.Parent?.Parent ?? throw new Exception("No parent");
        Assert.Equal(parent, tile.ParentAt(tile.Zoom - 5));
    }
}
