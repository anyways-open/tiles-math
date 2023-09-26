namespace TilesMath.Tests;

public class TileStaticTests
{
    [Fact]
    public void Tile_BetweenLine_WhenLineInOneTile_ShouldEnumerateOneTile()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.257307188009435),
            (4.807845526446613,
                51.25502328861376)
        }, 14).ToList();

        Assert.Single(tiles);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5466, tiles[0].Y);
    }

    [Fact]
    public void Tile_BetweenLine_WhenLineHorizontal_OneTileHop_ShouldEnumerateTwoTiles()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.257307188009435),
            (4.827976785148991,
                51.257307188009435)
        }, 14).ToList();

        Assert.Equal(2, tiles.Count);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5466, tiles[0].Y);
        Assert.Equal(14, tiles[1].Zoom);
        Assert.Equal(8411, tiles[1].X);
        Assert.Equal(5466, tiles[1].Y);
    }

    [Fact]
    public void Tile_BetweenLine_WhenLineHorizontal_TwoTileHop_ShouldEnumerateThreeTiles()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.257307188009435),
            (4.847976785148991,
                51.257307188009435)
        }, 14).ToList();

        Assert.Equal(3, tiles.Count);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5466, tiles[0].Y);
        Assert.Equal(14, tiles[1].Zoom);
        Assert.Equal(8411, tiles[1].X);
        Assert.Equal(5466, tiles[1].Y);
        Assert.Equal(14, tiles[2].Zoom);
        Assert.Equal(8412, tiles[2].X);
        Assert.Equal(5466, tiles[2].Y);
    }

    [Fact]
    public void Tile_BetweenLine_WhenLineVertical_OneTileHopDown_ShouldEnumerateTwoTiles()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.257307188009435),
            (4.802056351009384,
                51.267307188009435)
        }, 14).ToList();

        Assert.Equal(2, tiles.Count);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5466, tiles[0].Y);
        Assert.Equal(14, tiles[1].Zoom);
        Assert.Equal(8410, tiles[1].X);
        Assert.Equal(5465, tiles[1].Y);
    }

    [Fact]
    public void Tile_BetweenLine_WhenLineVertical_OneTileHopUp_ShouldEnumerateTwoTiles()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.267307188009435),
            (4.802056351009384,
                51.257307188009435)
        }, 14).ToList();

        Assert.Equal(2, tiles.Count);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5465, tiles[0].Y);
        Assert.Equal(14, tiles[1].Zoom);
        Assert.Equal(8410, tiles[1].X);
        Assert.Equal(5466, tiles[1].Y);
    }

    [Fact]
    public void Tile_BetweenLine_WhenLineVertical_TwoTileHop_ShouldEnumerateThreeTiles()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.257307188009435),
            (4.802056351009384,
                51.277307188009435)
        }, 14).ToList();

        Assert.Equal(3, tiles.Count);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5466, tiles[0].Y);
        Assert.Equal(14, tiles[1].Zoom);
        Assert.Equal(8410, tiles[1].X);
        Assert.Equal(5465, tiles[1].Y);
        Assert.Equal(14, tiles[2].Zoom);
        Assert.Equal(8410, tiles[2].X);
        Assert.Equal(5464, tiles[2].Y);
    }

    [Fact]
    public void Tile_BetweenLine_WhenLineToNorthEast_TwoTileHop_ShouldEnumerateTwoTiles()
    {
        var tiles = Tile.BelowLine(new (double longitude, double latitude)[]
        {
            (4.802056351009384,
                51.257307188009435),
            (4.847976785148991,
                51.277307188009435)
        }, 14).ToList();

        Assert.Equal(5, tiles.Count);
        Assert.Equal(14, tiles[0].Zoom);
        Assert.Equal(8410, tiles[0].X);
        Assert.Equal(5466, tiles[0].Y);
        Assert.Equal(14, tiles[1].Zoom);
        Assert.Equal(8411, tiles[1].X);
        Assert.Equal(5466, tiles[1].Y);
        Assert.Equal(14, tiles[2].Zoom);
        Assert.Equal(8411, tiles[2].X);
        Assert.Equal(5465, tiles[2].Y);
        Assert.Equal(14, tiles[3].Zoom);
        Assert.Equal(8412, tiles[3].X);
        Assert.Equal(5465, tiles[3].Y);
        Assert.Equal(14, tiles[4].Zoom);
        Assert.Equal(8412, tiles[4].X);
        Assert.Equal(5464, tiles[4].Y);
    }
}
