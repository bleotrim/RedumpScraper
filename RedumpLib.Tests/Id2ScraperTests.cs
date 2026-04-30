using Xunit;
using RedumpLib;
using System.Reflection;

namespace RedumpLib.Tests;

public class Id2BuildDateTests : IClassFixture<Id2Fixture>
{
    private readonly RedumpDisc _disc;

    public Id2BuildDateTests(Id2Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Title_ShouldBeAceCombat3Electrosphere()
    {
        Assert.NotNull(_disc);
        Assert.Contains("Ace Combat 3: Electrosphere", _disc.Title);
    }

    [Fact]
    public void Id_ShouldBe2()
    {
        Assert.NotNull(_disc);
        Assert.Equal("2", _disc.Id);
    }

    [Fact]
    public void System_ShouldBeSonyPlayStation()
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
            "English", "French", "German", "Italian", "Spanish"
        };

        Assert.NotNull(_disc.GameInfo);
        Assert.Equal(5, _disc.GameInfo.Languages.Count);
        Assert.Equivalent(expectedLanguages, _disc.GameInfo.Languages);
    }

    [Fact]
    public void Serial_ShouldBeSCES02066()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("SCES-02066", _disc.GameInfo.Serial);
    }

    [Fact] 
    public void BuildDate_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.BuildDate);
    }

    [Fact] 
    public void Version_ShouldBeNull()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Null(_disc.GameInfo.Version);
    }

    [Fact]
    public void Edition_ShouldBeOriginalPlatinumPromoPressKit()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("Original, Platinum, Promo, Press Kit", _disc.GameInfo.Edition);
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
        Assert.NotEmpty(_disc.Tracks);
        Assert.Single(_disc.Tracks);
    }

    [Fact]
    public void WriteOffset_ShouldBeMinus647()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("-647", _disc.GameInfo.WriteOffset);
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
        Assert.Equal("2022-03-11 18:44", _disc.GameInfo.LastModifiedDate);
    }

    [Fact]
    public void ExeDate_ShouldBe19991211()
    {
        Assert.NotNull(_disc.GameInfo);
        Assert.Equal("1999-12-11", _disc.GameInfo.ExeDate);
    }

    [Fact]
    public void EDC_ShouldBeYes()
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
        var expected = "Disc ID: 9887829 (Promo), 9279228 (Platinum)\n\nAlternative Title: acecombat 3 (Promo disc)\n\nPress Kit edition game disc appears to be a standard retail disc. Press Kit edition is bundled with Ace Combat 3: Electrosphere (Europe) (Press Disc).";
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
        var expected = "7 11719 27922 8, 7 11719 27962 4, 7 11719 88732 4, 7 11719 88742 3, 7 11719 88762 1";
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
    public void CuesheetStatus_ShouldBeCorrect()
    {
        var expected = "Has to be confirmed";
        Assert.NotNull(_disc);
        Assert.Equal(expected, _disc.CuesheetStatus);
    }

    [Fact]
    public void PvdStatus_ShouldBeCorrect()
    {
        var expected = "Has to be confirmed";
        Assert.NotNull(_disc);
        Assert.Equal(expected, _disc.PvdStatus);
    }

    [Fact]
    public void TracksCount_ShouldBeOne()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal(1, _disc.Tracks.Count);
    }

    [Fact]
    public void FirstTrackNumber_ShouldBeOne()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("1", _disc.Tracks[0].Number);
    }

    [Fact]
    public void FirstTrackType_ShouldBeDataMode2()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("Data/Mode 2", _disc.Tracks[0].Type);
    }

    [Fact]
    public void FirstTrackPregap_ShouldBe000000()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("00:00:00", _disc.Tracks[0].Pregap);
    }

    [Fact]
    public void FirstTrackLength_ShouldBe443005()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("44:30:05", _disc.Tracks[0].Length);
    }

    [Fact]
    public void FirstTrackSectors_ShouldBe200255()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("200255", _disc.Tracks[0].Sectors);
    }

    [Fact]
    public void FirstTrackSize_ShouldBe470999760()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("470999760", _disc.Tracks[0].Size);
    }

    [Fact]
    public void FirstTrackCrc32_ShouldBe91cad2df()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("91cad2df", _disc.Tracks[0].Crc32);
    }

    [Fact]
    public void FirstTrackMd5_ShouldBe24c2f5a5e43e4bc4c41081f5ef4dc818()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("24c2f5a5e43e4bc4c41081f5ef4dc818", _disc.Tracks[0].Md5);
    }

    [Fact]
    public void FirstTrackSha1_ShouldBe8c215d983ad7d7f5f8aa122981cbd79d846532ec()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Equal("8c215d983ad7d7f5f8aa122981cbd79d846532ec", _disc.Tracks[0].Sha1);
    }

    [Fact]
    public void RingsCount_ShouldBeFour()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal(4, _disc.Rings.Count);
    }

    [Fact]
    public void Ring1Number_ShouldBe1()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("1", _disc.Rings[0].Number);
    }

    [Fact]
    public void Ring1MasteringCode_ShouldBeNUll()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[0].MasteringCode);
    }

    [Fact]
    public void Ring1MasteringSidCode_ShouldBeNUll()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[0].MasteringSidCode);
    }

    [Fact]
    public void Ring1Toolstamp_ShouldBeNUll()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[0].Toolstamp);
    }

    [Fact]
    public void Ring1MouldSidCode_ShouldBeIFPI944Q()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("IFPI 944Q", _disc.Rings[0].MouldSidCode);
    }

    [Fact]
    public void Ring1Status_ShouldBeCorrect()
    {
        var expected = "Incomplete or not properly formed";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[0].Status);
    }

    [Fact]
    public void Ring1AdditionalMouldText_ShouldBeNUll()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[0].AdditionalMouldText);
    }
    
    [Fact]
    public void Ring1WriteOffset_ShouldBeNUll()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[0].WriteOffset);
    }

    [Fact]
    public void Ring2Number_ShouldBe2()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("2", _disc.Rings[1].Number);
    }

    [Fact]
    public void Ring2MasteringCode_ShouldBeNull()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[1].MasteringCode);
    }

    [Fact]
    public void Ring2MasteringSidCode_ShouldBeNull()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[1].MasteringSidCode);
    }

    [Fact]
    public void Ring2Toolstamp_ShouldBeNull()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[1].Toolstamp);
    }

    [Fact]
    public void Ring2MouldSidCode_ShouldBeIFPI94Z2()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("IFPI 94Z2", _disc.Rings[1].MouldSidCode);
    }

    [Fact]
    public void Ring2Status_ShouldBeCorrect()
    {
        var expected = "Incomplete or not properly formed";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[1].Status);
    }

    [Fact]
    public void Ring2AdditionalMouldText_ShouldBeNull()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[1].AdditionalMouldText);
    }

    [Fact]
    public void Ring2WriteOffset_ShouldBeNull()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Null(_disc.Rings[1].WriteOffset);
    }

    [Fact]
    public void Ring3Number_ShouldBe3()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("3", _disc.Rings[2].Number);
    }

    [Fact]
    public void Ring3MasteringCode_ShouldBeCorrect()
    {
        var expected = "DADC   A0100306117-0101   15";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[2].MasteringCode);
    }

    [Fact]
    public void Ring3MasteringSidCode_ShouldBeIFPIL555()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("IFPI L555", _disc.Rings[2].MasteringSidCode);
    }

    [Fact]
    public void Ring3Toolstamp_ShouldBeCorrect()
    {
        var expected = "A3, A6, B4";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[2].Toolstamp);
    }

    [Fact]
    public void Ring3MouldSidCode_ShouldBeCorrect()
    {   
        var expected = "IFPI 942R, IFPI 947A, IFPI 948Q";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[2].MouldSidCode);
    }

    [Fact]
    public void Ring3Status_ShouldBeCorrect()
    {
        var expected = "Has to be confirmed";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[2].Status);
    }

    [Fact]
    public void Ring3AdditionalMouldText_ShouldBeCorrect()
    {
        var expected = "*";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[2].AdditionalMouldText);
    }

    [Fact]
    public void Ring3WriteOffset_ShouldBeMinus647()
    {   
        Assert.NotNull(_disc.Rings);
        Assert.Equal("-647", _disc.Rings[2].WriteOffset);
    }

    // Ring 4

    [Fact]
    public void Ring4Number_ShouldBe4()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("4", _disc.Rings[3].Number);
    }

    [Fact]
    public void Ring4MasteringCode_ShouldBeCorrect()
    {
        var expected = "DADC   A0100306117-0101   25";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[3].MasteringCode);
    }

    [Fact]
    public void Ring4MasteringSidCode_ShouldBeIFPIL555()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("IFPI L555", _disc.Rings[3].MasteringSidCode);
    }

    [Fact]
    public void Ring4Toolstamp_ShouldBeCorrect()
    {
        var expected = "A3, A4, B2";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[3].Toolstamp);
    }

    [Fact]
    public void Ring4MouldSidCode_ShouldBeCorrect()
    {
        var expected = "IFPI 943P, IFPI 944P, IFPI 947A";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[3].MouldSidCode);
    }

    [Fact]
    public void Ring4Status_ShouldBeCorrect()
    {
        var expected = "Has to be confirmed";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[3].Status);
    }

    [Fact]
    public void Ring4AdditionalMouldText_ShouldBeCorrect()
    {
        var expected = "*";
        Assert.NotNull(_disc.Rings);
        Assert.Equal(expected, _disc.Rings[3].AdditionalMouldText);
    }

    [Fact]
    public void Ring4WriteOffset_ShouldBeMinus647()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal("-647", _disc.Rings[3].WriteOffset);
    }

    // PVD entries

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
        Assert.Equal("31 39 39 39 31 32 31 31 30 31 30 32 30 30 30 30 00", entry.Contents);
        Assert.Equal("1999-12-11", entry.Date);
        Assert.Equal("01:02:00.00", entry.Time);
        Assert.Equal("+00:00", entry.Gmt);
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
}