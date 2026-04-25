using Xunit;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID123974SecuritySectorTests : IClassFixture<ID123974Fixture>
{
    private readonly RedumpDisc _disc;

    public ID123974SecuritySectorTests(ID123974Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeCorrect()
    {
        Assert.NotNull(_disc);
        Assert.Contains("Madden NFL 08", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBeCorrect()
    {
        Assert.Equal("123974", _disc.Id);
    }

    [Fact]
    public void System_ShouldBeXbox360()
    {
        Assert.Equal("Microsoft Xbox 360", _disc.System);
    }

    [Fact]
    public void Media_ShouldBeDVD9()
    {
        Assert.Equal("DVD-9", _disc.Media);
    }

    [Fact]
    public void Category_ShouldBeGames()
    {
        Assert.Equal("Games", _disc.Category);
    }

    [Fact]
    public void Serial_ShouldBeEA2077()
    {
        Assert.Equal("EA-2077", _disc.Serial);
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
    public void SecuritySectorRanges_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.SecuritySectorRanges);
    }

    [Fact]
    public void SecuritySectorRanges_ShouldHaveTwoRanges()
    {
        Assert.Equal(2, _disc.SecuritySectorRanges.Count);
    }

    [Fact]
    public void FirstSecuritySectorRange_NumberShouldBeOne()
    {
        Assert.Equal(1, _disc.SecuritySectorRanges[0].Number);
    }

    [Fact]
    public void FirstSecuritySectorRange_StartShouldBe108976()
    {
        Assert.Equal(108976, _disc.SecuritySectorRanges[0].Start);
    }

    [Fact]
    public void FirstSecuritySectorRange_EndShouldBe113071()
    {
        Assert.Equal(113071, _disc.SecuritySectorRanges[0].End);
    }

    [Fact]
    public void SecondSecuritySectorRange_NumberShouldBeTwo()
    {
        Assert.Equal(2, _disc.SecuritySectorRanges[1].Number);
    }

    [Fact]
    public void SecondSecuritySectorRange_StartShouldBe3719856()
    {
        Assert.Equal(3719856, _disc.SecuritySectorRanges[1].Start);
    }

    [Fact]
    public void SecondSecuritySectorRange_EndShouldBe3723951()
    {
        Assert.Equal(3723951, _disc.SecuritySectorRanges[1].End);
    }

    [Fact]
    public void SecondSecuritySectorRange_ShouldHaveNote()
    {
        Assert.NotNull(_disc.SecuritySectorRanges[1].Note);
    }

    [Fact]
    public void SecondSecuritySectorRange_NoteShouldContainXGD2()
    {
        var note = _disc.SecuritySectorRanges[1].Note;
        Assert.NotNull(note);
        Assert.Contains("XGD2", note);
    }

    [Fact]
    public void Barcode_ShouldBeCorrect()
    {
        Assert.Equal("4 988648 539471", _disc.Barcode);
    }

    [Fact]
    public void GameComments_ShouldNotBeNull()
    {
        Assert.NotNull(_disc.GameComments);
    }

    [Fact]
    public void GameComments_Comments_ShouldContainXeMID()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Contains("XeMID", _disc.GameComments.Comments);
    }

    [Fact]
    public void GameComments_Comments_ShouldContainDMI()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Contains("DMI", _disc.GameComments.Comments);
    }

    [Fact]
    public void TrackStatus_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.TrackStatus);
    }
}
