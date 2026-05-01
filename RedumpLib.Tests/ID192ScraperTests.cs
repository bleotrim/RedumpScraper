using Xunit;
using RedumpLib;
using System.IO;

namespace RedumpLib.Tests;

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
        Assert.NotNull(_disc);
        Assert.Equal("192", _disc.Id);
    }

    [Fact]
    public void Rings_ShouldNotBeEmpty()
    {
        Assert.NotNull(_disc.Rings);
        Assert.NotEmpty(_disc.Rings);
    }

    [Fact]
    public void Rings_CountShouldBeFive()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal(5, _disc.Rings.Count);
    }

    [Fact]
    public void Ring1_ShouldHaveCorrectNumber()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[0];
        Assert.Equal("1", ring.Number);
    }

    [Fact]
    public void Ring1_ShouldHaveStatus()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[0];
        Assert.NotNull(ring.Status);
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring2_ShouldHaveCorrectNumber()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[1];
        Assert.Equal("2", ring.Number);
    }

    [Fact]
    public void Ring2_ShouldHaveStatus()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[1];
        Assert.NotNull(ring.Status);
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring3_ShouldHaveCorrectNumber()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[2];
        Assert.Equal("3", ring.Number);
    }

    [Fact]
    public void Ring3_ShouldHaveStatus()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[2];
        Assert.NotNull(ring.Status);
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring4_ShouldHaveCorrectNumber()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[3];
        Assert.Equal("4", ring.Number);
    }

    [Fact]
    public void Ring4_ShouldHaveStatus()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[3];
        Assert.NotNull(ring.Status);
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }

    [Fact]
    public void Ring5_ShouldHaveCorrectNumber()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[4];
        Assert.Equal("5", ring.Number);
    }

    [Fact]
    public void Ring5_ShouldHaveStatus()
    {
        Assert.NotNull(_disc.Rings);
        var ring = _disc.Rings[4];
        Assert.NotNull(ring.Status);
        Assert.NotEmpty(ring.Status);
        Assert.Contains("confirmed", ring.Status.ToLower());
    }
}