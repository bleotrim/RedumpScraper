using Xunit;
using RedumpLib;
using System.IO;

namespace RedumpLib.Tests;

public class ID192Fixture
{
    public RedumpDisc Disc { get; }

    public ID192Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "192.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }
        
        var html = File.ReadAllText(filePath);
        Disc = scraper.ParseRedumpHtml(html);
        Disc.Id = "192";
    }
}

public class ID192ScraperTests : IClassFixture<ID192Fixture>
{
    private readonly RedumpDisc _disc;

    public ID192ScraperTests(ID192Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeCorrect()
    {
        Assert.NotNull(_disc);
        Assert.Contains("Final Fantasy X", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBeCorrect()
    {
        Assert.Equal("192", _disc.Id);
    }

    [Fact]
    public void Rings_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.Rings);
    }

    [Fact]
    public void Rings_CountShouldBeFive()
    {
        Assert.Equal(5, _disc.Rings.Count);
    }

    [Fact]
    public void Ring1_ShouldHaveCorrectNumber()
    {
        var ring = _disc.Rings[0];
        Assert.Equal("1", ring.Number);
    }

    [Fact]
    public void Ring1_ShouldHaveStatus()
    {
        var ring = _disc.Rings[0];
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring2_ShouldHaveCorrectNumber()
    {
        var ring = _disc.Rings[1];
        Assert.Equal("2", ring.Number);
    }

    [Fact]
    public void Ring2_ShouldHaveStatus()
    {
        var ring = _disc.Rings[1];
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring3_ShouldHaveCorrectNumber()
    {
        var ring = _disc.Rings[2];
        Assert.Equal("3", ring.Number);
    }

    [Fact]
    public void Ring3_ShouldHaveStatus()
    {
        var ring = _disc.Rings[2];
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring4_ShouldHaveCorrectNumber()
    {
        var ring = _disc.Rings[3];
        Assert.Equal("4", ring.Number);
    }

    [Fact]
    public void Ring4_ShouldHaveStatus()
    {
        var ring = _disc.Rings[3];
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring5_ShouldHaveCorrectNumber()
    {
        var ring = _disc.Rings[4];
        Assert.Equal("5", ring.Number);
    }

    [Fact]
    public void Ring5_ShouldHaveStatus()
    {
        var ring = _disc.Rings[4];
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }
}
