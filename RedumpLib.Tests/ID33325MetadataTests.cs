using Xunit;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID33325MetadataTests : IClassFixture<ID33325Fixture>
{
    private readonly RedumpDisc _disc;

    public ID33325MetadataTests(ID33325Fixture fixture)
    {
        _disc = fixture.Disc;
    }

    [Fact]
    public void Metadata_ShouldNotBeNull()
    {
        Assert.NotNull(_disc.Metadata);
    }

    [Fact]
    public void Metadata_DiscKey_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Metadata);
        Assert.Equal("7ED309572E76886B4DF644A0F5CCF170", _disc.Metadata.DiscKey);
    }

    [Fact]
    public void Metadata_DiscId_ShouldBeCorrect()
    {
        Assert.NotNull(_disc.Metadata);
        Assert.Equal("00000000000000FF00020001XXXXXXXX", _disc.Metadata.DiscId);
    }

    [Fact]
    public void Metadata_Pic_ShouldNotBeEmpty()
    {
        Assert.NotNull(_disc.Metadata);
        Assert.NotEmpty(_disc.Metadata.Pic);
    }

    [Fact]
    public void Metadata_Pic_ShouldStartWithCorrectHex()
    {
        Assert.NotNull(_disc.Metadata);
        Assert.StartsWith("10020000444901080000200042444F01", _disc.Metadata.Pic);
    }
}
