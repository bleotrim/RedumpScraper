using Xunit;
using RedumpLib;
using System.Collections.Generic;

namespace RedumpLib.Tests;

public class Id277ScraperTests : IClassFixture<Id277Fixture>
{
    private readonly RedumpDisc _disc;

    public Id277ScraperTests(Id277Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeSoulcaliburII()
    {
        Assert.NotNull(_disc);
        Assert.Equal("Soulcalibur II", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBe277()
    {
        Assert.NotNull(_disc);
        Assert.Equal("277", _disc.Id);
    }

    [Fact]
    public void System_ShouldBeSonyPlayStation2()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Sony PlayStation 2", _disc.GameInfo.System);
    }

    [Fact]
    public void Media_ShouldBeDVD5()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("DVD-5", _disc.GameInfo.Media);
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

    // This disc has "✔ Languages" instead of "Languages" in the table header, so we need to ignore the "✔ " part when comparing the expected languages.
    [Fact]
    public void Languages_ShouldHaveCorrectCountAndContent()
    {
        var expectedLanguages = new List<string>
        {
            "English", "French", "German", "Italian", "Spanish"
        };

        Assert.NotNull(_disc.GameInfo);
        Assert.Equal(5, _disc.GameInfo.Languages.Count);
        Assert.Equivalent(expectedLanguages, _disc.GameInfo.Languages);
    }

    [Fact]
    public void Serial_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("SLES-51799, SLES-51799/GER", _disc.GameInfo.Serial);
    }

    [Fact] 
    public void BuildDate_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.BuildDate);
    }

    [Fact] 
    public void Version_ShouldBe100()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("1.00", _disc.GameInfo.Version);
    }

    [Fact]
    public void Edition_ShouldBeOriginal()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Original", _disc.GameInfo.Edition);
    }

    [Fact]
    public void ErrorsCount_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.ErrorsCount);
    }

    [Fact]
    public void NumberOfTracks_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.NumberOfTracks);
    }

    [Fact]
    public void WriteOffset_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.WriteOffset);
    }

    [Fact]
    public void Added_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.AddedDate);
    }

    [Fact]
    public void LastModified_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2025-01-10 16:29", _disc.GameInfo.LastModifiedDate);
    }

    [Fact]
    public void ExeDate_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2003-07-28", _disc.GameInfo.ExeDate);
    }

    [Fact]
    public void EDC_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.Edc);
    }

    [Fact]
    public void AntiModchip_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.AntiModchip);
    }

    [Fact]
    public void LibCrypt_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.LibCrypt);
    }

    [Fact]
    public void GameComments_ShouldBeNotNull()
    {
        Assert.NotNull(_disc.GameComments);
    }

    [Fact]
    public void MetaData_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Null(_disc.GameComments.Metadata);
    }

    [Fact]
    public void Comments_ShouldBeCorrect()
    {
        var expected = "Internal Serial: SLES-51799\n\nNamco ID: NOJ03404059IS (Cover), NOJ03404059M (Manual), NOX03404059D (Disc), NOD03404059IS (Cover), NOX03404059D/GER (Disc)\n\nLanguages in bios. Language selector for Dutch + Portuguese\n\nOption for English / Japanese voices\n\nVideo mode selector 50 Hz / 60 Hz / Progressive Scan";
        Assert.NotNull(_disc.GameComments);
        Assert.Equal(expected, _disc.GameComments.Comments);
    }

    [Fact]
    public void Contents_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Null(_disc.GameComments.Contents);
    }

    [Fact]
    public void Barcode_ShouldBeCorrect()
    {
        var expected = "3 348542 192041 &gt;, 5 030930 034900, 5 030932 034908 &gt;, 5 030944 034903 &gt;, 5 030945 034902, 5 035228 034907 &gt;";
        Assert.NotNull(_disc.GameComments);
        Assert.Equal(expected, _disc.GameComments.Barcode);
    }

    [Fact]
    public void TrackStatus_ShouldBeCorrect()
    {
        var expected = "2 and more dumps from original media [!]";
        Assert.NotNull(_disc);
        Assert.Equal(expected, _disc.TrackStatus);
    }

    [Fact]
    public void CuesheetStatus_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.CuesheetStatus);
    }

    [Fact]
    public void PvdStatus_ShouldBeCorrect()
    {
        var expected = "Has to be confirmed";
        Assert.NotNull(_disc);
        Assert.Equal(expected, _disc.PvdStatus);
    }

    [Fact]
    public void Track1Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Single(_disc.Tracks); 
        
        var track = _disc.Tracks[0];

        Assert.Equal("1", track.Number);
        Assert.Equal("Data", track.Type);
        Assert.Equal(string.Empty, track.Pregap);
        Assert.Equal(string.Empty, track.Length);
        Assert.Equal("2123328", track.Sectors);
        Assert.Equal("4348575744", track.Size);
        Assert.Equal("1136a195", track.Crc32);
        Assert.Equal("0b474052584b451c755cc6d423fb5ef0", track.Md5);
        Assert.Equal("7e6dcaba4bd2ddc21bb918630dcdd259e1a721f4", track.Sha1);
    }

    [Fact]
    public void RingsCount_ShouldBeTwo()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal(2, _disc.Rings.Count);
    }

    [Fact]
    public void Ring1Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Rings);
        Assert.NotEmpty(_disc.Rings);
        
        var ring = _disc.Rings[0];

        Assert.Equal("1", ring.Number);
        Assert.Equal("Sony DADC   A0100478790-A511   17", ring.MasteringCode);
        Assert.Equal("IFPI L557", ring.MasteringSidCode);
        Assert.Equal("A8, A7", ring.Toolstamp);
        Assert.Equal("IFPI 945W, IFPI 949Z", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
    }

    [Fact]
    public void Ring2Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Rings);
        Assert.True(_disc.Rings.Count > 1);
        
        var ring = _disc.Rings[1];

        Assert.Equal("2", ring.Number);
        Assert.Equal("Sony DADC   A0100478790-A511   27", ring.MasteringCode);
        Assert.Equal("IFPI L557", ring.MasteringSidCode);
        Assert.Equal("A2, B2, A5", ring.Toolstamp);
        Assert.Equal("IFPI 945Y, IFPI 945Z", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
    }

    [Fact]
    public void PvdEntriesCount_ShouldBeFour()
    {
        Assert.NotNull(_disc.PvdEntries);
        Assert.Equal(4, _disc.PvdEntries.Count);
    }

    [Fact]
    public void PvdCreationEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
        var entry = _disc.PvdEntries[0];
        Assert.Equal("Creation", entry.Entry);
        Assert.Equal("32 30 30 33 30 37 32 39 32 33 31 38 34 31 30 30 24", entry.Contents);
        Assert.Equal("2003-07-29", entry.Date);
        Assert.Equal("23:18:41.00", entry.Time);
        Assert.Equal("+09:00", entry.Gmt);
    }

    [Fact]
    public void PvdModificationEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
        var entry = _disc.PvdEntries[1];
        Assert.Equal("Modification", entry.Entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdExpirationEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
        var entry = _disc.PvdEntries[2];
        Assert.Equal("Expiration", entry.Entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdEffectiveEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
        var entry = _disc.PvdEntries[3];
        Assert.Equal("Effective", entry.Entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void LibCryptSectors_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.LibCryptSectors);
    }

    [Fact]
    public void HeaderEntries_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.HeaderEntries);
    }

    [Fact]
    public void HeaderStatus_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.HeaderStatus);
    }

    [Fact]
    public void SecuritySectorRanges_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.SecuritySectorRanges);
    }

    [Fact]
    public void Metadata_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.Metadata);
    }
}