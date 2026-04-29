using Xunit;
using RedumpLib;
using System.Collections.Generic;

namespace RedumpLib.Tests;

public class ID17031ScraperTests : IClassFixture<ID17031Fixture>
{
    private readonly RedumpDisc _disc;

    public ID17031ScraperTests(ID17031Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeCorrect()
    {
        Assert.NotNull(_disc);
        Assert.Contains("Sheep, Dog 'n' Wolf", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBeCorrect()
    {
        Assert.Equal("17031", _disc.Id);
    }

    [Fact]
    public void TrackStatus_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.TrackStatus);
    }

    [Fact]
    public void TrackStatus_ShouldContainStatusInfo()
    {
        Assert.Contains("from original media", _disc.TrackStatus);
    }

    [Fact]
    public void CuesheetStatus_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.CuesheetStatus);
    }

    [Fact]
    public void PvdStatus_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.PvdStatus);
    }

    [Fact]
    public void Ring_ShouldHaveStatus()
    {
        Assert.NotEmpty(_disc.Rings);
        Assert.NotNull(_disc.Rings[0].Status);
    }

    [Fact]
    public void System_ShouldBePlayStation()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Sony PlayStation", _disc.GameInfo.System);
    }

    [Fact]
    public void Media_ShouldBeCD()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("CD", _disc.GameInfo.Media);
    }

    [Fact]
    public void Category_ShouldBeGames()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Games", _disc.GameInfo.Category);
    }

    [Fact]
    public void Region_ShouldBeEurope()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Europe", _disc.GameInfo.Region);
    }

    [Fact]
    public void Languages_ShouldHaveCorrectCountAndContent()
    {
        var expectedLanguages = new List<string>
        {
            "Italian", "English", "French", "German", "Spanish", "Dutch"
        };

        Assert.NotNull(_disc.GameInfo);
        Assert.Equal(6, _disc.GameInfo.Languages.Count);
        Assert.Equivalent(expectedLanguages, _disc.GameInfo.Languages);
    }
    [Fact]
    public void Serial_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("SLES-02895", _disc.GameInfo.Serial);
    }
    [Fact]
    public void ExeDate_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2001-04-26", _disc.GameInfo.ExeDate);
    }
    
    [Fact]
    public void Edition_ShouldBeOriginal()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Original", _disc.GameInfo.Edition);
    }
    [Fact]
    public void Edc_ShouldBeYes()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Yes", _disc.GameInfo.Edc);
    }
    [Fact]
    public void AntiModchip_ShouldBeNo()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("No", _disc.GameInfo.AntiModchip);
    }

    [Fact]
public void LibCrypt_ShouldBeNo()
{
    Assert.Equal("No", _disc.GameInfo!.LibCrypt);
}

[Fact]
public void ErrorsCount_ShouldBeZero()
{
    Assert.NotNull(_disc.GameInfo);
    Assert.Equal("0", _disc.GameInfo.ErrorsCount);
}

[Fact]
public void NumberOfTracks_ShouldBeOne()
{
    Assert.Single(_disc.Tracks);
}
[Fact]
public void WriteOffset_ShouldBeCorrect()
{
    Assert.NotNull(_disc.GameInfo);
    Assert.Equal("-647", _disc.GameInfo.WriteOffset);
}
[Fact]
public void AddedDate_ShouldBeCorrect()
{
    Assert.NotNull(_disc.GameInfo);
    Assert.Equal("2010-11-28 11:00", _disc.GameInfo.AddedDate);
}
[Fact]
public void LastModifiedDate_ShouldBeCorrect()
{
    Assert.NotNull(_disc.GameInfo);
    Assert.Equal("2019-07-05 19:14", _disc.GameInfo.LastModifiedDate);
}
[Fact] 
public void Barcode_ShouldBeCorrect() => Assert.Equal("3 546430 014486, 3 546430 014493", _disc.GameComments.Barcode);
[Fact]
public void Comments_ShouldMatchExpectedText()
{
    var expected = "French Title: Une faim de Loup\n\nItalian Title: Ralph il Lupo all'attacco\n\nSpanish Title: Looney Tunes: Perro & Lobo";
    Assert.NotNull(_disc.GameComments);
    Assert.Equal(expected, _disc.GameComments.Comments);
}

[Fact]
public void Track1_Metadata_ShouldBeCorrect()
{
    var track = _disc.Tracks[0];
    
    Assert.Equal("1", track.Number);
    Assert.Equal("Data/Mode 2", track.Type); 
    Assert.Equal("633254832", track.Size);
}

[Fact]
public void Track1_Checksums_ShouldMatch()
{
    var track = _disc.Tracks[0];

    Assert.Equal("f6da6902", track.Crc32.ToLower());
    Assert.Equal("7b95531bd5021c48ace4f2df1cfd86c3", track.Md5.ToLower());
    Assert.Equal("b3aada568b220c6bb813c8ac775e647db8c2a3d2", track.Sha1.ToLower());
}
[Fact]
public void Ring_ThirdEntry_ShouldHaveFullData()
{
    var ring = _disc.Rings.FirstOrDefault(r => r.Number == "3");
    
    Assert.NotNull(ring);
    Assert.Contains("Sony DADC", ring!.MasteringCode);
    Assert.Equal("IFPI L555", ring.MasteringSidCode);
}

[Fact]
public void Ring_FirstEntry_ShouldBeNull()
{
    var ring = _disc.Rings.FirstOrDefault(r => r.Number == "1");
    
    Assert.NotNull(ring);
    Assert.Equal("", ring!.MasteringCode);
}

[Fact]
public void Pvd_TotalEntries_ShouldBeFour()
{
    Assert.Equal(4, _disc.PvdEntries.Count);
}

[Fact]
public void Pvd_CreationDate_ShouldBeCorrect()
{
    var creation = _disc.PvdEntries.FirstOrDefault(e => e.Entry == "Creation");
    
    Assert.NotNull(creation);
    
    Assert.Equal("2001-05-04", creation!.Date);
    Assert.Equal("15:45:14.00", creation.Time);
    Assert.Equal("+09:00", creation.Gmt);
}

[Fact]
public void Pvd_ModificationDate_ShouldBeEmpty()
{
    var modification = _disc.PvdEntries.FirstOrDefault(e => e.Entry == "Modification");
    
    Assert.NotNull(modification);
    Assert.Equal("0000-00-00", modification!.Date);
}

[Fact]
public void Pvd_EffectiveDate_ShouldMatchCreationOrZero()
{
    var effective = _disc.PvdEntries.FirstOrDefault(e => e.Entry == "Effective");
    
    Assert.NotNull(effective);
    Assert.Equal("0000-00-00", effective.Date);
}

[Fact]
public void LibCryptSectors_ShouldBeEmpty()
{
    // The test data (ID_17031.html) has LibCrypt: No, so no sectors should be parsed
    Assert.Empty(_disc.LibCryptSectors);
}

[Fact]
public void LibCryptSectors_ShouldBeInitialized()
{
    // Verify the collection is initialized (not null)
    Assert.NotNull(_disc.LibCryptSectors);
}
}