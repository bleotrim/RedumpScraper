using Xunit;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID47415HeaderTests : IClassFixture<ID47415Fixture>
{
    private readonly RedumpDisc _disc;

    public ID47415HeaderTests(ID47415Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void ParseHeader_ReturnsValidHeaderEntries()
    {
        // Assert
        Assert.NotNull(_disc.HeaderEntries);
        Assert.NotEmpty(_disc.HeaderEntries);
    }

    [Fact]
    public void ParseHeader_HasCorrectNumberOfEntries()
    {
        // Assert - Should have 16 header entries (0x00 to 0xF0)
        Assert.Equal(16, _disc.HeaderEntries.Count);
    }

    [Fact]
    public void ParseHeader_FirstRowContainsSegaSegakatana()
    {
        // Assert
        var firstEntry = _disc.HeaderEntries[0];
        Assert.Equal("0000", firstEntry.Row);
        Assert.Equal("53 45 47 41 20 53 45 47 41 4B 41 54 41 4E 41 20", firstEntry.Contents);
        Assert.Equal("SEGA SEGAKATANA ", firstEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_SecondRowContainsSegaEnterprises()
    {
        // Assert
        var secondEntry = _disc.HeaderEntries[1];
        Assert.Equal("0010", secondEntry.Row);
        Assert.Equal("53 45 47 41 20 45 4E 54 45 52 50 52 49 53 45 53", secondEntry.Contents);
        Assert.Equal("SEGA ENTERPRISES", secondEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_ThirdRowContainsBuildInfo()
    {
        // Assert
        var thirdEntry = _disc.HeaderEntries[2];
        Assert.Equal("0020", thirdEntry.Row);
        Assert.Equal("31 38 45 39 20 47 44 2D 52 4F 4D 31 2F 31 20 20", thirdEntry.Contents);
        Assert.Equal("18E9 GD-ROM1/1  ", thirdEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_FourthRowContainsRegion()
    {
        // Assert
        var fourthEntry = _disc.HeaderEntries[3];
        Assert.Equal("0030", fourthEntry.Row);
        Assert.Equal("20 55 20 20 20 20 20 20 32 37 39 39 41 31 30 20", fourthEntry.Contents);
        Assert.Contains("U", fourthEntry.Ascii); // USA region
    }

    [Fact]
    public void ParseHeader_FifthRowContainsSerial()
    {
        // Assert
        var fifthEntry = _disc.HeaderEntries[4];
        Assert.Equal("0040", fifthEntry.Row);
        Assert.Equal("4D 4B 2D 35 31 30 35 37 20 20 56 31 2E 30 30 30", fifthEntry.Contents);
        Assert.Contains("MK-51057", fifthEntry.Ascii); // Disc serial
    }

    [Fact]
    public void ParseHeader_SixthRowContainsBuildDate()
    {
        // Assert
        var sixthEntry = _disc.HeaderEntries[5];
        Assert.Equal("0050", sixthEntry.Row);
        Assert.Equal("32 30 30 30 30 34 31 32 20 20 20 20 20 20 20 20", sixthEntry.Contents);
        Assert.Equal("20000412        ", sixthEntry.Ascii); // Build date: 2000-04-12
    }

    [Fact]
    public void ParseHeader_SeventhRowContains1stReadBin()
    {
        // Assert
        var seventhEntry = _disc.HeaderEntries[6];
        Assert.Equal("0060", seventhEntry.Row);
        Assert.Equal("31 53 54 5F 52 45 41 44 2E 42 49 4E 20 20 20 20", seventhEntry.Contents);
        Assert.Contains("1ST_READ.BIN", seventhEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_EighthRowContainsPublisher()
    {
        // Assert
        var eighthEntry = _disc.HeaderEntries[7];
        Assert.Equal("0070", eighthEntry.Row);
        Assert.Equal("53 45 47 41 20 45 4E 54 45 52 50 52 49 53 45 53", eighthEntry.Contents);
        Assert.Equal("SEGA ENTERPRISES", eighthEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_NinthRowContainsProductTitle()
    {
        // Assert
        var ninthEntry = _disc.HeaderEntries[8];
        Assert.Equal("0080", ninthEntry.Row);
        Assert.Equal("47 45 4E 45 52 41 54 4F 52 20 32 20 20 20 20 20", ninthEntry.Contents);
        Assert.Contains("GENERATOR 2", ninthEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_LastRow()
    {
        // Assert
        var lastEntry = _disc.HeaderEntries[15];
        Assert.Equal("00F0", lastEntry.Row);
        // The last row should contain spaces
        Assert.NotEmpty(lastEntry.Contents);
        Assert.NotEmpty(lastEntry.Ascii);
    }

    [Fact]
    public void ParseHeader_AllRowsHaveHexFormat()
    {
        // Assert
        foreach (var entry in _disc.HeaderEntries)
        {
            // Row should be hex format (0000, 0010, etc.)
            Assert.Matches(@"^[0-9A-Fa-f]{4}$", entry.Row);
            // Contents should contain hex bytes
            Assert.NotEmpty(entry.Contents);
            // ASCII should not be empty
            Assert.NotEmpty(entry.Ascii);
        }
    }
}
