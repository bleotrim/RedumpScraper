using Xunit;
using RedumpLib;
using System.Collections.Generic;
using System.Reflection;

namespace RedumpLib.Tests;

public class Id625ScraperTests : IClassFixture<Id625Fixture>
{
    private readonly RedumpDisc _disc;

    public Id625ScraperTests(Id625Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Id_ShouldBe625()
    {
        Assert.NotNull(_disc);
        Assert.Equal("625", _disc.Id);
    }

    [Fact]
    public void Title_ShouldBeTimeGal()
    {
        Assert.NotNull(_disc);
        Assert.Equal("Time Gal", _disc.Title);
    }

    [Fact]
    public void System_ShouldBeSegaMegaCDAndSegaCD()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Sega Mega CD &amp; Sega CD", _disc.GameInfo.System);
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
    public void Region_ShouldBeUSA()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("USA", _disc.GameInfo.Region);
    }

    [Fact]
    public void Languages_ShouldBeEnglish()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Single(_disc.GameInfo.Languages);
        Assert.Equal("English", _disc.GameInfo.Languages[0]);
    }

    [Fact]
    public void Serial_ShouldBeT6214()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("T-6214", _disc.GameInfo.Serial);
    }

    [Fact] 
    public void BuildDate_ShouldBe199304()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("1993-04", _disc.GameInfo.BuildDate);
    }

    [Fact] 
    public void Version_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.Version);
    }

    [Fact]
    public void Edition_ShouldBeOriginal()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Original", _disc.GameInfo.Edition);
    }

    [Fact]
    public void ErrorsCount_ShouldBeZero()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("0", _disc.GameInfo.ErrorsCount);
    }

    [Fact]
    public void NumberOfTracks_ShouldBeTwo()
    {
        Assert.NotEmpty(_disc.Tracks);
        Assert.Equal(2, _disc.Tracks.Count);
    }

    [Fact]
    public void WriteOffset_ShouldBeMinus65()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("-65", _disc.GameInfo.WriteOffset);
    }

   [Fact]
    public void Added_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.AddedDate);
    }

    [Fact]
    public void LastModified_ShouldBe201305012159()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2013-05-01 21:59", _disc.GameInfo.LastModifiedDate);
    }

    [Fact]
    public void ExeDate_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.ExeDate);
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
        Assert.Null(_disc.GameInfo!.LibCrypt);
    }

    [Fact]
    public void GameComments_ShouldBeNotNull()
    {   
        Assert.NotNull(_disc);    
        Assert.NotNull(_disc.GameComments);
    }

    [Fact]
    public void MetaData_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Equal("CATALOG 0000000000000", _disc.GameComments.Metadata);
    }

    [Fact]
    public void Comments_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Null(_disc.GameComments.Comments);
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
        var expected = "7 20238 10310 5";
        Assert.NotNull(_disc.GameComments);
        Assert.Equal(expected, _disc.GameComments.Barcode);
    }

    [Fact]
    public void TrackStatus_ShouldBeCorrect()
    {
        Assert.Equal("2 and more dumps from original media [!]", _disc.TrackStatus);
    }

    [Fact]
    public void CuesheetStatus_ShouldBeCorrect()
    {
        Assert.Equal("Has been confirmed [!]", _disc.CuesheetStatus);
    }

    [Fact]
    public void PvdStatus_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.PvdStatus);
    }

    [Fact]
    public void Track1Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("1", track.Number);
        Assert.Equal("Data/Mode 1", track.Type);
        Assert.Equal("00:00:00", track.Pregap);
        Assert.Equal("28:14:49", track.Length);
        Assert.Equal("127099", track.Sectors);
        Assert.Equal("298936848", track.Size);
        Assert.Equal("2941c00f", track.Crc32);
        Assert.Equal("4002e973efb2812d2c793068d86c4833", track.Md5);
        Assert.Equal("d57b76bd9f3f51ae78796a92e60f150c7d5b32be", track.Sha1);
    }

    [Fact]
    public void Track2Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[1];
        Assert.Equal("2", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:02:00", track.Pregap);
        Assert.Equal("00:09:28", track.Length);
        Assert.Equal("703", track.Sectors);
        Assert.Equal("1653456", track.Size);
        Assert.Equal("bf577267", track.Crc32);
        Assert.Equal("eb7c557b5ac6bdaacf253bc12a31db75", track.Md5);
        Assert.Equal("20cb5fd3f9b02c56b9e43c83d6d24b9dad5ac7cb", track.Sha1);
    }

    [Fact]
    public void Ring1Data_ShouldBeCorrect()
    {
        var ring = _disc.Rings[0];
        Assert.Equal("1", ring.Number);
        Assert.Equal("SEGAT49025 R2C   MFD BY JVC", ring.MasteringCode);
        Assert.Equal("13", ring.Toolstamp);
        Assert.Null(ring.MasteringSidCode);
        Assert.Equal("Has been confirmed [!]", ring.Status);
        Assert.Null(ring.AdditionalMouldText);
        Assert.Equal("-65 ✔", ring.WriteOffset);
    }

    [Fact]
    public void PvdEntries_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.PvdEntries);
    }

    [Fact]
    public void LibCryptSectors_ShouldBeNull()
    {
        Assert.NotNull(_disc);
        Assert.Null(_disc.LibCryptSectors);
    }

[Fact]
    public void HeaderEntries_ShouldHaveSixteenEntries()
    {
        Assert.NotNull(_disc.HeaderEntries);
        Assert.Equal(16, _disc.HeaderEntries.Count);
    }

    [Fact]
    public void HeaderEntry_0100_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0100");
        Assert.NotNull(entry);
        Assert.Equal("53 45 47 41 20 47 45 4E 45 53 49 53 20 20 20 20", entry.Contents);
        Assert.Equal("SEGA GENESIS    ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0110_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0110");
        Assert.NotNull(entry);
        Assert.Equal("28 43 29 54 2D 34 39 20 31 39 39 33 2E 41 50 52", entry.Contents);
        Assert.Equal("(C)T-49 1993.APR", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0120_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0120");
        Assert.NotNull(entry);
        Assert.Equal("54 49 4D 45 20 47 41 4C 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("TIME GAL        ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0130_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0130");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0140_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0140");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0150_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0150");
        Assert.NotNull(entry);
        Assert.Equal("54 49 4D 45 20 47 41 4C 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("TIME GAL        ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0160_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0160");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0170_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0170");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0180_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0180");
        Assert.NotNull(entry);
        Assert.Equal("47 4D 20 54 2D 36 32 31 34 20 2D 30 30 20 20 20", entry.Contents);
        Assert.Equal("GM T-6214 -00   ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_0190_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "0190");
        Assert.NotNull(entry);
        Assert.Equal("4A 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("J               ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_01A0_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "01A0");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_01B0_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "01B0");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_01C0_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "01C0");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_01D0_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "01D0");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_01E0_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "01E0");
        Assert.NotNull(entry);
        Assert.Equal("20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("                ", entry.Ascii);
    }

    [Fact]
    public void HeaderEntry_01F0_ShouldBeCorrect()
    {
        var entry = _disc.HeaderEntries.Find(e => e.Row == "01F0");
        Assert.NotNull(entry);
        Assert.Equal("4A 55 20 20 20 20 20 20 20 20 20 20 20 20 20 20", entry.Contents);
        Assert.Equal("JU              ", entry.Ascii);
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