namespace TilesMath.Tests.Pixels;

public class TilePixelsTests
{
    [Fact]
    public void TilePixels_ZeroZero_ShouldBeBottomLeft()
    {
        var tile = Tile.AtLocation(4, 51, 14);

        var pixels = tile.GetPixels();

        var (left, bottom) = pixels.CoordinatesAt(0, 0);
        Assert.Equal(tile.Boundaries.Left, left, 6);
        Assert.Equal(tile.Boundaries.Bottom, bottom, 6);
    }
    
    [Fact]
    public void TilePixels_MaxMax_ShouldBeTopRight()
    {
        var tile = Tile.AtLocation(4, 51, 14);

        var pixels = tile.GetPixels();

        var (left, bottom) = pixels.CoordinatesAt(512, 512);
        Assert.Equal(tile.Boundaries.Right, left, 6);
        Assert.Equal(tile.Boundaries.Top, bottom, 6);
    }
    
    [Fact]
    public void TilePixels_HalfHalf_ShouldBeCenter()
    {
        var tile = Tile.AtLocation(4, 51, 14);

        var pixels = tile.GetPixels();

        var (left, bottom) = pixels.CoordinatesAt(512 / 2, 512 / 2);
        Assert.Equal(tile.Boundaries.CenterLongitude, left, 6);
        Assert.Equal(tile.Boundaries.CenterLatitude, bottom, 6);
    }
    
    [Fact]
    public void TilePixels_HalfLongitude_ShouldBeHalfResolution()
    {
        var tile = Tile.AtLocation(4, 51, 14);

        var pixels = tile.GetPixels();

        var (x, _) = pixels.PixelFor(tile.Boundaries.CenterLongitude, tile.Boundaries.Top);

        Assert.Equal(512/2, x);
    }
    
    [Fact]
    public void TilePixels_HalfLatitude_ShouldBeHalfResolution()
    {
        var tile = Tile.AtLocation(4, 51, 14);

        var pixels = tile.GetPixels();

        var (_, y) = pixels.PixelFor(tile.Boundaries.Left, tile.Boundaries.CenterLatitude);

        Assert.Equal(512/2, y);
    }
}
