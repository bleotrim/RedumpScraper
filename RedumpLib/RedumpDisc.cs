namespace RedumpLib;

public class RedumpDisc
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public GameInfo? GameInfo { get; set; }
    public GameComments? GameComments { get; set; }
    public string TrackStatus { get; set; } = "";
    public string CuesheetStatus { get; set; } = "";
    public string PvdStatus { get; set; } = "";
    public List<DiscTrack> Tracks { get; set; } = new();
    public List<DiscRing> Rings { get; set; } = new();
    public List<PvdRecord> PvdEntries { get; set; } = new();
    public List<LibCryptSector> LibCryptSectors { get; set; } = new();
    public List<HeaderEntry> HeaderEntries { get; set; } = new();
    public string HeaderStatus { get; set; } = "";
    public List<SecuritySectorRange> SecuritySectorRanges { get; set; } = new();
    public Metadata? Metadata { get; set; }
    public string HtmlSource { get; set; } = "";
}
