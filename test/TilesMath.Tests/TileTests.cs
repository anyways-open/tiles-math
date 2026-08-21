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

    [Fact]
    public void Tile_ChildrenAtZoomCount_SameZoom_ShouldEqual1()
    {
        var tile = Tile.Create(1025, 4511, 14);

        Assert.Equal(1, tile.ChildrenAtZoomCount(14));
    }

    [Fact]
    public void Tile_ChildrenAtZoomCount_Zoom1Higher_ShouldEqual4()
    {
        var tile = Tile.Create(1025, 4511, 14);

        Assert.Equal(4, tile.ChildrenAtZoomCount(15));
    }

    [Fact]
    public void Tile_ChildrenAtZoomCount_Zoom2Higher_ShouldEqual16()
    {
        var tile = Tile.Create(1025, 4511, 14);

        Assert.Equal(16, tile.ChildrenAtZoomCount(16));
    }

    [Fact]
    public void Tile_ChildrenAtZoomCount_Zoom0_ShouldEqual4ToThePowerOfZoom()
    {
        var tile = Tile.Create(0, 0, 0);

        Assert.Equal((int)Math.Pow(4, 14), tile.ChildrenAtZoomCount(14));
        Assert.Equal((int)Math.Pow(4, 5), tile.ChildrenAtZoomCount(5));
    }

    [Fact]
    public void Tile_DefaultEqualityComparer_ShouldNotAllocate()
    {
        // a struct that does not implement IEquatable<T> gets the object-based default comparer,
        // which boxes an argument on every comparison. Tiles are hash keys, so this guards the
        // IEquatable<Tile> implementation against being dropped again.
        var comparer = EqualityComparer<Tile>.Default;
        var tile = Tile.Create(1025, 4511, 14);
        var same = Tile.Create(1025, 4511, 14);
        var other = Tile.Create(1025, 4512, 14);

        Assert.True(comparer.Equals(tile, same));
        Assert.False(comparer.Equals(tile, other));

        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < 1000; i++)
        {
            comparer.Equals(tile, same);
            comparer.Equals(tile, other);
        }
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.Equal(0, allocated);
    }
}
