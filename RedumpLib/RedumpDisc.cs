namespace RedumpLib;

public class RedumpDisc
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public GameInfo? GameInfo { get; set; }
    public GameComments? GameComments { get; set; }
    public string? TrackStatus { get; set; } = null;
    public string? CuesheetStatus { get; set; } = null;
    public string? PvdStatus { get; set; } = null;
    public List<DiscTrack>? Tracks { get; set; } = null;
    public List<DiscRing>? Rings { get; set; } = null;
    public List<PvdRecord>? PvdEntries { get; set; } = null;
    public List<LibCryptSector>? LibCryptSectors { get; set; } = null;
    public List<HeaderEntry> HeaderEntries { get; set; } = new();
    public string? HeaderStatus { get; set; } = null;
    public List<SecuritySectorRange> SecuritySectorRanges { get; set; } = new();
    public Metadata? Metadata { get; set; }
    public string? HtmlSource { get; set; } = null;
}
