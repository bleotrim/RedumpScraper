using Xunit;
using RedumpLib;
using System.Collections.Generic;
using System.Reflection;

namespace RedumpLib.Tests;

public class Id2509ScraperTests : IClassFixture<Id2509Fixture>
{
    private readonly RedumpDisc _disc;

    public Id2509ScraperTests(Id2509Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Id_ShouldBe2509()
    {
        Assert.NotNull(_disc);
        Assert.Equal("2509", _disc.Id);
    }

    [Fact]
    public void Title_ShouldBeGodOfWarII()
    {
        Assert.NotNull(_disc);
        Assert.Equal("God of War II", _disc.Title);
    }

    [Fact]
    public void System_ShouldBeSonyPlayStation2()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Sony PlayStation 2", _disc.GameInfo.System);
    }

    [Fact]
    public void Media_ShouldBeDVD9()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("DVD-9", _disc.GameInfo.Media);
    }

    [Fact]
    public void Category_ShouldBeGames()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Games", _disc.GameInfo.Category);
    }

    [Fact]
    public void Region_ShouldBeEuropeAustralia()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Europe, Australia", _disc.GameInfo.Region);
    }

    [Fact]
    public void Languages_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal(6, _disc.GameInfo.Languages.Count);
        Assert.Equal("English", _disc.GameInfo.Languages[0]);
        Assert.Equal("French", _disc.GameInfo.Languages[1]);
        Assert.Equal("German", _disc.GameInfo.Languages[2]);
        Assert.Equal("Italian", _disc.GameInfo.Languages[3]);
        Assert.Equal("Russian", _disc.GameInfo.Languages[4]);
        Assert.Equal("Spanish", _disc.GameInfo.Languages[5]);
    }

    [Fact]
    public void Serial_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("SCES-54206, SCES-54206 ANZ/P, SCES-54206/ANZ, SCES-54206/GER, SCES-54206/P, SCES-54206/P/GER, SCES-54206/UK", _disc.GameInfo.Serial);
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
    public void Edition_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Original, Platinum, Platinum: The Best of PlayStation 2, Special Edition", _disc.GameInfo.Edition);
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
    public void Added_ShouldBe200801270606()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2008-01-27 06:06", _disc.GameInfo.AddedDate);
    }

    [Fact]
    public void LastModified_ShouldBe202408061037()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2024-08-06 10:37", _disc.GameInfo.LastModifiedDate);
    }

    [Fact]
    public void ExeDate_ShouldBe20070306()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2007-03-06", _disc.GameInfo.ExeDate);
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
    public void Layerbreak_ShouldBe2080912()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2080912", _disc.GameInfo.Layerbreak);
    }

    [Fact]
    public void GameComments_ShouldBeNotNull()
    {   
        Assert.NotNull(_disc);    
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
        Assert.NotNull(_disc.GameComments);
        Assert.Equal("Internal Serial: SCES-54206\n\nVolume Label: GODOFWAR2", _disc.GameComments.Comments);
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
        var expected = "7 11719 60619 2, 7 11719 60669 7, 7 11719 60719 9, 7 11719 60789 2, 7 11719 90422 9, 7 11719 90432 8, 7 11719 90442 7";
        Assert.NotNull(_disc.GameComments);
        Assert.Equal(expected, _disc.GameComments.Barcode);
    }

    [Fact]
    public void TrackStatus_ShouldBeCorrect()
    {
        Assert.Equal("2 and more dumps from original media [!]", _disc.TrackStatus);
    }

    [Fact]
    public void CuesheetStatus_ShouldBeNull()
    {
        Assert.Null(_disc.CuesheetStatus);
    }

    [Fact]
    public void PvdStatus_ShouldBeHasToBeConfirmed()
    {
        Assert.NotNull(_disc);
        Assert.Equal("Has to be confirmed", _disc.PvdStatus);
    }

    [Fact]
    public void Track1Data_ShouldBeCorrect()
    {
        Assert.Single(_disc.Tracks);
        var track = _disc.Tracks[0];
        Assert.Equal("1", track.Number);
        Assert.Null(track.Type);
        Assert.Null(track.Pregap);
        Assert.Null(track.Length);
        Assert.Equal("4093488", track.Sectors);
        Assert.Equal("8383463424", track.Size);
        Assert.Equal("e685fce4", track.Crc32);
        Assert.Equal("0cb4033a4596d7a65e5eebdda068534b", track.Md5);
        Assert.Equal("26d7bdfb655420cda13a74a6138e3201c7cac536", track.Sha1);
    }

    [Fact]
    public void Rings_ShouldHaveThreeEntries()
    {
        Assert.Equal(3, _disc.Rings.Count);
    }

    [Fact]
    public void Ring1Data_ShouldBeCorrect()
    {
        var ring = _disc.Rings[0];
        Assert.Equal("1", ring.Number);
        Assert.Equal("Sony DADC   A0100791099-A911   27", ring.MasteringCode);
        Assert.Equal("IFPI L557", ring.MasteringSidCode);
        Assert.Equal("A4, F2, A1", ring.Toolstamp);
        Assert.Equal("IFPI 943V, IFPI 942V, IFPI 942W", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
        Assert.Null(ring.AdditionalMouldText);
        Assert.Null(ring.WriteOffset);
    }

    [Fact]
    public void Ring2Data_ShouldBeCorrect()
    {
        var ring = _disc.Rings[1];
        Assert.Equal("2", ring.Number);
        Assert.Equal("Sony DADC   A0100791099-A911   37", ring.MasteringCode);
        Assert.Equal("IFPI L557", ring.MasteringSidCode);
        Assert.Equal("A06", ring.Toolstamp);
        Assert.Equal("IFPI 948W", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
        Assert.Null(ring.AdditionalMouldText);
        Assert.Null(ring.WriteOffset);
    }

    [Fact]
    public void Ring3Data_ShouldBeCorrect()
    {
        var ring = _disc.Rings[2];
        Assert.Equal("3", ring.Number);
        Assert.Equal("Sony DADC   A0100791099-A911   37", ring.MasteringCode);
        Assert.Equal("IFPI L557", ring.MasteringSidCode);
        Assert.Equal("C02", ring.Toolstamp);
        Assert.Equal("IFPI 949V", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
        Assert.Null(ring.AdditionalMouldText);
        Assert.Null(ring.WriteOffset);
    }

    [Fact]
    public void PvdEntries_ShouldHaveFourEntries()
    {
        Assert.NotNull(_disc.PvdEntries);
        Assert.Equal(4, _disc.PvdEntries.Count);
    }

    [Fact]
    public void PvdEntry_Creation_ShouldBeCorrect()
    {
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Creation");
        Assert.NotNull(entry);
        Assert.Equal("32 30 30 37 30 31 30 32 31 38 34 32 34 33 30 30 24", entry.Contents);
        Assert.Equal("2007-01-02", entry.Date);
        Assert.Equal("18:42:43.00", entry.Time);
        Assert.Equal("+09:00", entry.Gmt);
    }

    [Fact]
    public void PvdEntry_Modification_ShouldBeCorrect()
    {
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Modification");
        Assert.NotNull(entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdEntry_Expiration_ShouldBeCorrect()
    {
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Expiration");
        Assert.NotNull(entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdEntry_Effective_ShouldBeCorrect()
    {
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Effective");
        Assert.NotNull(entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void LibCryptSectors_ShouldBeNull()
    {
        Assert.Null(_disc.LibCryptSectors);
    }

    [Fact]
    public void HeaderEntries_ShouldBeNull()
    {
        Assert.Null(_disc.HeaderEntries);
    }

    [Fact]
    public void HeaderStatus_ShouldBeNull()
    {
        Assert.Null(_disc.HeaderStatus);
    }

    [Fact]
    public void SecuritySectorRanges_ShouldBeNull()
    {
        Assert.Null(_disc.SecuritySectorRanges);
    }

    [Fact]
    public void Metadata_ShouldBeNull()
    {
        Assert.Null(_disc.Metadata);
    }
}