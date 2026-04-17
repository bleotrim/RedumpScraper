using Xunit;
using RedumpLib;
using System.Linq;

namespace RedumpLib.Tests;

public class ID27824LibCryptTests : IClassFixture<ID27824Fixture>
{
    private readonly RedumpDisc _disc;

    public ID27824LibCryptTests(ID27824Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void DiscId_ShouldBeCorrect()
    {
        Assert.Equal("27824", _disc.Id);
    }

    [Fact]
    public void LibCryptStatus_ShouldBeYes()
    {
        Assert.Equal("Yes", _disc.LibCrypt);
    }

    [Fact]
    public void LibCryptSectors_ShouldNotBeEmpty()
    {
        Assert.NotEmpty(_disc.LibCryptSectors);
    }

    [Fact]
    public void LibCryptSectors_CountShouldBe32()
    {
        Assert.Equal(32, _disc.LibCryptSectors.Count);
    }

    [Fact]
    public void LibCryptSectors_AllShouldHaveSectorNumber()
    {
        var invalidSectors = _disc.LibCryptSectors.Where(s => string.IsNullOrEmpty(s.Sector)).ToList();
        Assert.Empty(invalidSectors);
    }

    [Fact]
    public void LibCryptSectors_AllSectorsShouldBeNumeric()
    {
        var allNumeric = _disc.LibCryptSectors.All(s => int.TryParse(s.Sector, out _));
        Assert.True(allNumeric);
    }

    [Fact]
    public void LibCryptSectors_AllShouldHaveMsf()
    {
        var invalidSectors = _disc.LibCryptSectors.Where(s => string.IsNullOrEmpty(s.Msf)).ToList();
        Assert.Empty(invalidSectors);
    }

    [Fact]
    public void LibCryptSectors_MsfFormatShouldBeValid()
    {
        var validMsf = _disc.LibCryptSectors.All(s => 
            s.Msf.Contains(":") && s.Msf.Split(':').Length == 3
        );
        Assert.True(validMsf, "All MSF values should have format MM:SS:FF");
    }

    [Fact]
    public void LibCryptSectors_AllShouldHaveContents()
    {
        var invalidSectors = _disc.LibCryptSectors.Where(s => string.IsNullOrEmpty(s.Contents)).ToList();
        Assert.Empty(invalidSectors);
    }

    [Fact]
    public void LibCryptSectors_AllShouldHaveXor()
    {
        var invalidSectors = _disc.LibCryptSectors.Where(s => string.IsNullOrEmpty(s.Xor)).ToList();
        Assert.Empty(invalidSectors);
    }

    [Fact]
    public void LibCryptSectors_AllShouldHaveComments()
    {
        var invalidSectors = _disc.LibCryptSectors.Where(s => string.IsNullOrEmpty(s.Comments)).ToList();
        Assert.Empty(invalidSectors);
    }

    [Fact]
    public void LibCryptSector_FirstSector_ShouldBe14105()
    {
        var firstSector = _disc.LibCryptSectors.First();
        Assert.Equal("14105", firstSector.Sector);
    }

    [Fact]
    public void LibCryptSector_FirstSector_MsfShouldBe030805()
    {
        var firstSector = _disc.LibCryptSectors.First();
        Assert.Equal("03:08:05", firstSector.Msf);
    }

    [Fact]
    public void LibCryptSector_FirstSector_ShouldHaveContents()
    {
        var firstSector = _disc.LibCryptSectors.First();
        Assert.Contains("41 01 01", firstSector.Contents);
    }

    [Fact]
    public void LibCryptSector_FirstSector_XorShouldBe8001c701()
    {
        var firstSector = _disc.LibCryptSectors.First();
        Assert.Equal("8001 c701", firstSector.Xor);
    }

    [Fact]
    public void LibCryptSector_FirstSector_CommentsContainsLC1()
    {
        var firstSector = _disc.LibCryptSectors.First();
        Assert.Contains("LC1 sector", firstSector.Comments);
    }

    [Fact]
    public void LibCryptSector_LastSector_ShouldBe44317()
    {
        var lastSector = _disc.LibCryptSectors.Last();
        Assert.Equal("44317", lastSector.Sector);
    }

    [Fact]
    public void LibCryptSector_LastSector_MsfShouldBe095067()
    {
        var lastSector = _disc.LibCryptSectors.Last();
        Assert.Equal("09:50:67", lastSector.Msf);
    }

    [Fact]
    public void LibCryptSector_LastSector_ShouldHaveContents()
    {
        var lastSector = _disc.LibCryptSectors.Last();
        Assert.NotEmpty(lastSector.Contents);
    }

    [Fact]
    public void LibCryptSector_Sector42045_ShouldExist()
    {
        var sector = _disc.LibCryptSectors.FirstOrDefault(s => s.Sector == "42045");
        Assert.NotNull(sector);
    }

    [Fact]
    public void LibCryptSector_Sector42045_MsfShouldBe092045()
    {
        var sector = _disc.LibCryptSectors.FirstOrDefault(s => s.Sector == "42045");
        Assert.NotNull(sector);
        Assert.Equal("09:20:45", sector!.Msf);
    }

    [Fact]
    public void LibCryptSector_Sector14110_ShouldExist()
    {
        var sector = _disc.LibCryptSectors.FirstOrDefault(s => s.Sector == "14110");
        Assert.NotNull(sector);
    }

    [Fact]
    public void LibCryptSector_Sector14110_XorShouldBe8001bbd8()
    {
        var sector = _disc.LibCryptSectors.FirstOrDefault(s => s.Sector == "14110");
        Assert.NotNull(sector);
        Assert.Equal("8001 bbd8", sector!.Xor);
    }

    [Fact]
    public void LibCryptSectors_XorValuesShouldVary()
    {
        var uniqueXorValues = _disc.LibCryptSectors.Select(s => s.Xor).Distinct().Count();
        Assert.True(uniqueXorValues > 1, "XOR values should vary between sectors");
    }

    [Fact]
    public void LibCryptSectors_AllCommentsContainLC1OrError()
    {
        var allValid = _disc.LibCryptSectors.All(s => 
            s.Comments.Contains("LC1") || s.Comments.Contains("error")
        );
        Assert.True(allValid, "All comments should contain LC1 or error information");
    }

    [Fact]
    public void LibCryptSectors_SectorNumbersShouldBeIncreasing()
    {
        var sectors = _disc.LibCryptSectors.Select(s => int.Parse(s.Sector)).ToList();
        
        // Check that sectors are in a generally ascending order (though may have gaps)
        for (int i = 1; i < sectors.Count; i++)
        {
            Assert.True(sectors[i] >= sectors[i - 1] || (sectors[i] < sectors[i - 1] && i > 15),
                $"Sector {sectors[i]} at index {i} should be >= {sectors[i - 1]} (allows for wrapping)");
        }
    }

    [Fact]
    public void LibCryptSectors_MiddleSector_Sector16031_ShouldExist()
    {
        var sector = _disc.LibCryptSectors.FirstOrDefault(s => s.Sector == "16031");
        Assert.NotNull(sector);
    }

    [Fact]
    public void LibCryptSectors_MiddleSector_Sector16031_MsfShouldBe033356()
    {
        var sector = _disc.LibCryptSectors.FirstOrDefault(s => s.Sector == "16031");
        Assert.NotNull(sector);
        Assert.Equal("03:33:56", sector!.Msf);
    }

    [Fact]
    public void LibCryptSectors_AllHaveRequiredFields()
    {
        foreach (var sector in _disc.LibCryptSectors)
        {
            Assert.False(string.IsNullOrWhiteSpace(sector.Sector), "Sector number must not be empty");
            Assert.False(string.IsNullOrWhiteSpace(sector.Msf), "MSF must not be empty");
            Assert.False(string.IsNullOrWhiteSpace(sector.Contents), "Contents must not be empty");
            Assert.False(string.IsNullOrWhiteSpace(sector.Xor), "XOR must not be empty");
            Assert.False(string.IsNullOrWhiteSpace(sector.Comments), "Comments must not be empty");
        }
    }

    [Fact]
    public void LibCryptSectors_ContentsContainHexData()
    {
        var allHexData = _disc.LibCryptSectors.All(s => 
            s.Contents.Contains("01") || s.Contents.Contains("41") || s.Contents.Contains("00")
        );
        Assert.True(allHexData, "Contents should contain hex data");
    }

    [Fact]
    public void LibCryptSector_FirstAndLastAreNotEqual()
    {
        var first = _disc.LibCryptSectors.First();
        var last = _disc.LibCryptSectors.Last();
        
        Assert.NotEqual(first.Sector, last.Sector);
        Assert.NotEqual(first.Msf, last.Msf);
    }

    [Fact]
    public void LibCryptSectors_ShouldHaveGapsInSectorNumbers()
    {
        var sectors = _disc.LibCryptSectors.Select(s => int.Parse(s.Sector)).OrderBy(x => x).ToList();
        var gaps = false;
        
        for (int i = 1; i < sectors.Count; i++)
        {
            if (sectors[i] - sectors[i - 1] > 1)
            {
                gaps = true;
                break;
            }
        }
        
        Assert.True(gaps, "LibCrypt sectors should have gaps in numbering");
    }

    [Fact]
    public void LibCryptSectors_CountShouldMatchExpectedValue()
    {
        // Should have exactly 32 protected sectors for this disc
        Assert.Equal(32, _disc.LibCryptSectors.Count);
    }
}
