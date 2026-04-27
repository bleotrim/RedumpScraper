using Xunit;
using RedumpLib;
using System.Collections.Generic;
using System.IO;

namespace RedumpLib.Tests;

public class ID201Fixture
{
    public RedumpDisc Disc { get; }

    public ID201Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "ID_201.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }
        
        var html = File.ReadAllText(filePath);
        Disc = scraper.ParseRedumpHtml(html);
        Disc.Id = "201";
    }
}

public class ID201ScraperTests : IClassFixture<ID201Fixture>
{
    private readonly RedumpDisc _disc;

    public ID201ScraperTests(ID201Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeCorrect()
    {
        Assert.NotNull(_disc);
        Assert.Contains("King Kong", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBeCorrect()
    {
        Assert.Equal("201", _disc.Id);
    }

    [Fact]
    public void Tracks_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.Tracks);
    }

    [Fact]
    public void Tracks_CountShouldBeOne()
    {
        Assert.Single(_disc.Tracks);
    }

    [Fact]
    public void Track1_ShouldHaveCorrectNumber()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("1", track.Number);
    }

    [Fact]
    public void Track1_ShouldHaveSectors()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("659728", track.Sectors);
    }

    [Fact]
    public void Track1_ShouldHaveSize()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("1351122944", track.Size);
    }

    [Fact]
    public void Track1_ShouldHaveCorrectCrc32()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("d7a6034e", track.Crc32);
    }

    [Fact]
    public void Track1_ShouldHaveCorrectMd5()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("b5d6353d600fa8ddbb5aed1e666c0b94", track.Md5);
    }

    [Fact]
    public void Track1_ShouldHaveCorrectSha1()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("ba1edaeb0163db9c07dc04c7ea60d6a6b48a761f", track.Sha1);
    }
}
