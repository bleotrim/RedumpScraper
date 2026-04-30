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
    public void Track1Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Tracks);
        Assert.Single(_disc.Tracks); 
        
        var track = _disc.Tracks[0];

        Assert.Equal("1", track.Number);
        Assert.Equal("Data/Mode 2", track.Type);
        Assert.Equal("00:00:00", track.Pregap);
        Assert.Equal("44:30:05", track.Length);
        Assert.Equal("200255", track.Sectors);
        Assert.Equal("470999760", track.Size);
        Assert.Equal("91cad2df", track.Crc32);
        Assert.Equal("24c2f5a5e43e4bc4c41081f5ef4dc818", track.Md5);
        Assert.Equal("8c215d983ad7d7f5f8aa122981cbd79d846532ec", track.Sha1);
    }

    [Fact]
    public void RingsCount_ShouldBeFour()
    {
        Assert.NotNull(_disc.Rings);
        Assert.Equal(4, _disc.Rings.Count);
    }

    [Fact]
    public void Ring1Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Rings);
        Assert.NotEmpty(_disc.Rings);
        
        var ring = _disc.Rings[0];

        Assert.Equal("1", ring.Number);
        Assert.Null(ring.MasteringCode);
        Assert.Null(ring.MasteringSidCode);
        Assert.Null(ring.Toolstamp);
        Assert.Equal("IFPI 944Q", ring.MouldSidCode);
        Assert.Equal("Incomplete or not properly formed", ring.Status);
        Assert.Null(ring.AdditionalMouldText);
        Assert.Null(ring.WriteOffset);
    }

    [Fact]
    public void Ring2Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Rings);
        Assert.True(_disc.Rings.Count > 1);
        
        var ring = _disc.Rings[1];

        Assert.Equal("2", ring.Number);
        Assert.Null(ring.MasteringCode);
        Assert.Null(ring.MasteringSidCode);
        Assert.Null(ring.Toolstamp);
        Assert.Equal("IFPI 94Z2", ring.MouldSidCode);
        Assert.Equal("Incomplete or not properly formed", ring.Status);
        Assert.Null(ring.AdditionalMouldText);
        Assert.Null(ring.WriteOffset);
    }

    [Fact]
    public void Ring3Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Rings);
        Assert.True(_disc.Rings.Count > 2);
        
        var ring = _disc.Rings[2];

        Assert.Equal("3", ring.Number);
        Assert.Equal("DADC   A0100306117-0101   15", ring.MasteringCode);
        Assert.Equal("IFPI L555", ring.MasteringSidCode);
        Assert.Equal("A3, A6, B4", ring.Toolstamp);
        Assert.Equal("IFPI 942R, IFPI 947A, IFPI 948Q", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
        Assert.Equal("*", ring.AdditionalMouldText);
        Assert.Equal("-647", ring.WriteOffset);
    }

    [Fact]
    public void Ring4Data_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Rings);
        Assert.True(_disc.Rings.Count > 3);
        
        var ring = _disc.Rings[3];

        Assert.Equal("4", ring.Number);
        Assert.Equal("DADC   A0100306117-0101   25", ring.MasteringCode);
        Assert.Equal("IFPI L555", ring.MasteringSidCode);
        Assert.Equal("A3, A4, B2", ring.Toolstamp);
        Assert.Equal("IFPI 943P, IFPI 944P, IFPI 947A", ring.MouldSidCode);
        Assert.Equal("Has to be confirmed", ring.Status);
        Assert.Equal("*", ring.AdditionalMouldText);
        Assert.Equal("-647", ring.WriteOffset);
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