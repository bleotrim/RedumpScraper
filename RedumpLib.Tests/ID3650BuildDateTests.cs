using Xunit;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID3650BuildDateTests : IClassFixture<ID3650Fixture>
{
    private readonly RedumpDisc _disc;

    public ID3650BuildDateTests(ID3650Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeCapcomGeneration()
    {
        Assert.NotNull(_disc);
        Assert.Contains("Capcom Generation", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBeCorrect()
    {
        Assert.Equal("3650", _disc.Id);
    }

    [Fact]
    public void System_ShouldBeSegaSaturn()
    {
        Assert.Equal("Sega Saturn", _disc.System);
    }

    [Fact]
    public void Media_ShouldBeCD()
    {
        Assert.Equal("CD", _disc.Media);
    }

    [Fact]
    public void Category_ShouldBeGames()
    {
        Assert.Equal("Games", _disc.Category);
    }

    [Fact]
    public void Serial_ShouldBeT1232G()
    {
        Assert.Equal("T-1232G", _disc.Serial);
    }

    [Fact]
    public void Region_ShouldBeJapan()
    {
        Assert.Equal("Japan", _disc.Region);
    }

    [Fact]
    public void Edition_ShouldBeOriginal()
    {
        Assert.Equal("Original", _disc.Edition);
    }

    [Fact]
    public void BuildDate_ShouldNotBeEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(_disc.BuildDate));
    }

    [Fact]
    public void BuildDate_ShouldBe19980627()
    {
        Assert.Equal("1998-06-27", _disc.BuildDate);
    }

    [Fact]
    public void BuildDate_ShouldHaveValidDateFormat()
    {
        Assert.Matches(@"^\d{4}-\d{2}-\d{2}$", _disc.BuildDate);
    }

    [Fact]
    public void Version_ShouldBe1002()
    {
        Assert.Equal("1.002", _disc.Version);
    }

    [Fact]
    public void ExeDate_ShouldBeEmpty()
    {
        // This disc does not have an EXE date field in the HTML
        Assert.True(string.IsNullOrWhiteSpace(_disc.ExeDate));
    }

    [Fact]
    public void ErrorsCount_ShouldBeZero()
    {
        Assert.Equal("0", _disc.ErrorsCount);
    }

    [Fact]
    public void NumberOfTracks_ShouldBeTwo()
    {
        Assert.Equal("2", _disc.NumberOfTracks);
    }

    [Fact]
    public void WriteOffset_ShouldBePositive684()
    {
        Assert.Equal("+684", _disc.WriteOffset);
    }
}
