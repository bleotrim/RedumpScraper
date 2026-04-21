namespace RedumpLib;

public class RedumpDisc
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string System { get; set; } = "";
    public string Media { get; set; } = "";
    public string Category { get; set; } = "";
    public string Region { get; set; } = "";
    public List<string> Languages { get; set; } = new();
    public string Serial { get; set; } = "";
    public string ExeDate { get; set; } = "";
    public string Version { get; set; } = "";
    public string Edition { get; set; } = "";
    public string Edc { get; set; } = "";
    public string AntiModchip { get; set; } = "";
    public string LibCrypt { get; set; } = "";
    public string ErrorsCount { get; set; } = "";
    public string NumberOfTracks { get; set; } = "";
    public string WriteOffset { get; set; } = "";
    public string AddedDate { get; set; } = "";
    public string LastModifiedDate { get; set; } = "";
    public string Barcode { get; set; } = "";
    public string Comments { get; set; } = "";
    public string TrackStatus { get; set; } = "";
    public string CuesheetStatus { get; set; } = "";
    public string PvdStatus { get; set; } = "";
    public List<DiscTrack> Tracks { get; set; } = new();
    public List<DiscRing> Rings { get; set; } = new();
    public List<PvdRecord> PvdEntries { get; set; } = new();
    public List<LibCryptSector> LibCryptSectors { get; set; } = new();
    public List<HeaderEntry> HeaderEntries { get; set; } = new();
}
