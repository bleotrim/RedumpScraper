namespace RedumpLib;

public class Metadata
{
    public string DiscKey { get; set; } = "";
    public string DiscId { get; set; } = "";
    public string Pic { get; set; } = "";

    public Metadata(string discKey, string discId, string pic)
    {
        DiscKey = discKey;
        DiscId = discId;
        Pic = pic;
    }
}
