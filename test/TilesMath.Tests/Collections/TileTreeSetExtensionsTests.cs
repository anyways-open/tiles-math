using TilesMath.Collections;

namespace TilesMath.Tests.Collections;

public class TileTreeSetExtensionsTests
{
    [Fact]
    public void TileTreeSet_SetWithTileZero_CountAtZoom2_ShouldBe16()
    {
        var set = new TileTreeSet { Tile.Create(0, 0, 0) };

        Assert.Equal(16, set.CountAtZoom(2));
    }

    [Fact]
    public void TileTreeSet_SetWithTileZero_CountAtZoom14_ShouldBe268435456()
    {
        var set = new TileTreeSet { Tile.Create(0, 0, 0) };

        Assert.Equal(268435456, set.CountAtZoom(14));
    }

    [Fact]
    public void TileTreeSet_SetWithOneZoom14Tile_CountAtZoom14_ShouldBe1()
    {
        var set = new TileTreeSet { Tile.Create(0, 0, 14) };

        Assert.Equal(1, set.CountAtZoom(14));
    }
}
