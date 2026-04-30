using Xunit;
using RedumpLib;
using System.Collections.Generic;

namespace RedumpLib.Tests;

public class Id96925ScraperTests : IClassFixture<Id96925Fixture>
{
    private readonly RedumpDisc _disc;

    public Id96925ScraperTests(Id96925Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Id_ShouldBe96925()
    {
        Assert.NotNull(_disc);
        Assert.Equal("96925", _disc.Id);
    }

    [Fact]
    public void Title_ShouldBe4DDriving()
    {
        Assert.NotNull(_disc);
        Assert.Equal("4D Driving", _disc.Title);
    }

    [Fact]
    public void System_ShouldBeFujitsuFMTownsSeries()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Fujitsu FM Towns series", _disc.GameInfo.System);
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
    public void Region_ShouldBeJapan()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Japan", _disc.GameInfo.Region);
    }

    [Fact]
    public void Languages_ShouldBeJapanese()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Single(_disc.GameInfo.Languages);
        Assert.Equal("Japanese", _disc.GameInfo.Languages[0]);
    }

    [Fact]
    public void Serial_ShouldBeHMD222A()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("HMD-222A", _disc.GameInfo.Serial);
    }

    [Fact] 
    public void BuildDate_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.BuildDate);
    }

    [Fact] 
    public void Version_ShouldBeRevA()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Rev A", _disc.GameInfo.Version);
    }

    [Fact]
    public void Edition_ShouldBeFMTownsMartyRerelease()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("FM Towns Marty Rerelease", _disc.GameInfo.Edition);
    }

    [Fact]
    public void ErrorsCount_ShouldBeZero()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("0", _disc.GameInfo.ErrorsCount);
    }

    [Fact]
    public void NumberOfTracks_ShouldBeTwelve()
    {
        Assert.NotEmpty(_disc.Tracks);
        Assert.Equal(12, _disc.Tracks.Count);
    }

    [Fact]
    public void WriteOffset_ShouldBePlus1107()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("+1107", _disc.GameInfo.WriteOffset);
    }

   [Fact]
    public void Added_ShouldBe202209140211()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2022-09-14 02:11", _disc.GameInfo.AddedDate);
    }

    [Fact]
    public void LastModified_ShouldBe202209160025()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("2022-09-16 00:25", _disc.GameInfo.LastModifiedDate);
    }

    [Fact]
    public void GameComments_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Equal("Electronic Arts ID: EFT-7005", _disc.GameComments.Comments);
        Assert.Equal("4 938833 000828", _disc.GameComments.Barcode);
    }

    [Fact]
    public void ExeDate_ShouldBe19930611()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("1993-06-11", _disc.GameInfo.ExeDate);
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
    public void MetaData_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameComments);
        Assert.Null(_disc.GameComments.Metadata);
    }

    [Fact]
    public void Comments_ShouldBeCorrect()
    {
        var expected = "Electronic Arts ID: EFT-7005";
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
        var expected = "4 938833 000828";
        Assert.NotNull(_disc.GameComments);
        Assert.Equal(expected, _disc.GameComments.Barcode);
    }

    [Fact]
    public void TrackStatus_ShouldBeCorrect()
    {
        Assert.Equal("Dumped from original media", _disc.TrackStatus);
    }

    [Fact]
    public void CuesheetStatus_ShouldBeCorrect()
    {
        Assert.Equal("Has been confirmed [!]", _disc.CuesheetStatus);
    }

    [Fact]
    public void PvdStatus_ShouldBeCorrect()
    {
        Assert.Equal("Has to be confirmed", _disc.PvdStatus);
    }

    [Fact]
    public void Track1Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[0];
        Assert.Equal("1", track.Number);
        Assert.Equal("Data/Mode 1", track.Type);
        Assert.Equal("00:00:00", track.Pregap);
        Assert.Equal("00:58:00", track.Length);
        Assert.Equal("4350", track.Sectors);
        Assert.Equal("10231200", track.Size);
        Assert.Equal("1e390b90", track.Crc32);
        Assert.Equal("69d1c32dd06487851fc415365721bbad", track.Md5);
        Assert.Equal("cf375b85c09afb8d3104f0417fc46f7975febd28", track.Sha1);
    }

    [Fact]
    public void Track2Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[1];
        Assert.Equal("2", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:02:00", track.Pregap);
        Assert.Equal("00:49:00", track.Length);
        Assert.Equal("3675", track.Sectors);
        Assert.Equal("8643600", track.Size);
        Assert.Equal("ab5f071b", track.Crc32);
        Assert.Equal("c309c3031f528a5e9140498c3747bed4", track.Md5);
        Assert.Equal("3f2dbd7e0ec41dadd6e374065eb2df1cc01a0bc7", track.Sha1);
    }

    [Fact]
    public void Track3Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[2];
        Assert.Equal("3", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:03:00", track.Pregap);
        Assert.Equal("00:27:00", track.Length);
        Assert.Equal("2025", track.Sectors);
        Assert.Equal("4762800", track.Size);
        Assert.Equal("f1b9040f", track.Crc32);
        Assert.Equal("272829a6d7b6b58a4d9d2f7cc56dc71d", track.Md5);
        Assert.Equal("33930f5755172ac37f192c0dac50ede38f3a3117", track.Sha1);
    }

    [Fact]
    public void Track4Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[3];
        Assert.Equal("4", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:03:00", track.Pregap);
        Assert.Equal("00:45:00", track.Length);
        Assert.Equal("3375", track.Sectors);
        Assert.Equal("7938000", track.Size);
        Assert.Equal("5af6f044", track.Crc32);
        Assert.Equal("54e81d631750af194ef83c9f5ea06a08", track.Md5);
        Assert.Equal("5729c270b493d74ef24eef51aaa66eb2242931f4", track.Sha1);
    }

    [Fact]
    public void Track5Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[4];
        Assert.Equal("5", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:03:00", track.Pregap);
        Assert.Equal("00:41:00", track.Length);
        Assert.Equal("3075", track.Sectors);
        Assert.Equal("7232400", track.Size);
        Assert.Equal("5d877a88", track.Crc32);
        Assert.Equal("ff79e5f66dee116fe42a203d4a9575d4", track.Md5);
        Assert.Equal("7e6b2783c25521c59e3da7e706a36cd972c81271", track.Sha1);
    }

    [Fact]
    public void Track6Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[5];
        Assert.Equal("6", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:04:00", track.Pregap);
        Assert.Equal("00:45:00", track.Length);
        Assert.Equal("3375", track.Sectors);
        Assert.Equal("7938000", track.Size);
        Assert.Equal("37541ef0", track.Crc32);
        Assert.Equal("f9fa5d090466c4697c41ebfe91c862f3", track.Md5);
        Assert.Equal("6f93d7add53d6ee7a66e55d59b6b87cd7043af52", track.Sha1);
    }

    [Fact]
    public void Track7Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[6];
        Assert.Equal("7", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:04:00", track.Pregap);
        Assert.Equal("00:37:00", track.Length);
        Assert.Equal("2775", track.Sectors);
        Assert.Equal("6526800", track.Size);
        Assert.Equal("440e0a71", track.Crc32);
        Assert.Equal("cfaada556c46479a0da289068dd8a6e4", track.Md5);
        Assert.Equal("0aa104dd90c71d856eff36c9dfe92ee3b13dffa4", track.Sha1);
    }

    [Fact]
    public void Track8Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[7];
        Assert.Equal("8", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:03:00", track.Pregap);
        Assert.Equal("00:45:00", track.Length);
        Assert.Equal("3375", track.Sectors);
        Assert.Equal("7938000", track.Size);
        Assert.Equal("e30827c1", track.Crc32);
        Assert.Equal("f479711afd75f2ac2b86f804559a67d1", track.Md5);
        Assert.Equal("eada05052c104fb96340d1114f17a90c906f243c", track.Sha1);
    }

    [Fact]
    public void Track9Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[8];
        Assert.Equal("9", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:04:00", track.Pregap);
        Assert.Equal("00:09:00", track.Length);
        Assert.Equal("675", track.Sectors);
        Assert.Equal("1587600", track.Size);
        Assert.Equal("e0ea7260", track.Crc32);
        Assert.Equal("47f37da61e6a998b6ca2678bf2528ba9", track.Md5);
        Assert.Equal("301c09953eb42e9766d8c5b62b85b8a738782db8", track.Sha1);
    }

    [Fact]
    public void Track10Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[9];
        Assert.Equal("10", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:15:00", track.Pregap);
        Assert.Equal("00:22:00", track.Length);
        Assert.Equal("1650", track.Sectors);
        Assert.Equal("3880800", track.Size);
        Assert.Equal("6abdbae0", track.Crc32);
        Assert.Equal("0cd6042be92913e3c1b093b88e4b422a", track.Md5);
        Assert.Equal("bd6003b1595c10b8167f19970af556b552479275", track.Sha1);
    }

    [Fact]
    public void Track11Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[10];
        Assert.Equal("11", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:18:00", track.Pregap);
        Assert.Equal("01:03:00", track.Length);
        Assert.Equal("4725", track.Sectors);
        Assert.Equal("11113200", track.Size);
        Assert.Equal("881174cd", track.Crc32);
        Assert.Equal("e7a2fa2597a37b776db518fb3de0693c", track.Md5);
        Assert.Equal("97ffa1ac92c65802d40437217a2d8153a26bd0e6", track.Sha1);
    }

    [Fact]
    public void Track12Data_ShouldBeCorrect()
    {
        var track = _disc.Tracks[11];
        Assert.Equal("12", track.Number);
        Assert.Equal("Audio", track.Type);
        Assert.Equal("00:03:00", track.Pregap);
        Assert.Equal("00:43:00", track.Length);
        Assert.Equal("3225", track.Sectors);
        Assert.Equal("7585200", track.Size);
        Assert.Equal("4dd0b5a3", track.Crc32);
        Assert.Equal("54c5e2389accde66e13d04c89efcdf81", track.Md5);
        Assert.Equal("f81d02f3e788b112fc75b11d69ce975bc3bf0c90", track.Sha1);
    }

    [Fact]
    public void Ring1Data_ShouldBeCorrect()
    {
        var ring = _disc.Rings[0];
        Assert.Equal("1", ring.Number);
        Assert.Equal("HMD-222A", ring.MasteringCode);
        Assert.Equal("Has been confirmed [!]", ring.Status);
        Assert.Equal("+1107 ✔", ring.WriteOffset);
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
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Creation");
        Assert.NotNull(entry);
        Assert.Equal("31 39 39 33 30 36 31 31 31 35 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("1993-06-11", entry.Date);
        Assert.Equal("15:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdModificationEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Modification");
        Assert.NotNull(entry);
        Assert.Equal("31 39 39 33 30 36 31 31 31 35 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("1993-06-11", entry.Date);
        Assert.Equal("15:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdExpirationEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
        var entry = _disc.PvdEntries.Find(e => e.Entry == "Expiration");
        Assert.NotNull(entry);
        Assert.Equal("30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 00", entry.Contents);
        Assert.Equal("0000-00-00", entry.Date);
        Assert.Equal("00:00:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
    }

    [Fact]
    public void PvdEffectiveEntry_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.PvdEntries);
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