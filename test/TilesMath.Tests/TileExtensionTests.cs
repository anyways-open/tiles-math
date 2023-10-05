namespace TilesMath.Tests;

public class TileExtensionTests
{
    [Fact]
    public void Tile_IsAncestor_WhenParent_ShouldBeTrue()
    {
        var tile = Tile.Create(1025, 4511, 14);
        Assert.True(tile.Parent != null && tile.Parent.Value.IsAncestor(tile));
    }

    [Fact]
    public void Tile_IsAncestor_WhenGranParent_ShouldBeTrue()
    {
        var tile = Tile.Create(1025, 4511, 14);
        Assert.True(tile.Parent?.Parent is not null && tile.Parent.Value.Parent.Value.IsAncestor(tile));
    }
}
