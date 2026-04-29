namespace RedumpLib;

/// <summary>
/// Encapsulates general game information and metadata about a Redump disc.
/// Contains system details, release information, and administrative dates.
/// </summary>
public class GameInfo
{
    public string System { get; set; } = "";
    public string Media { get; set; } = "";
    public string Category { get; set; } = "";
    public string Region { get; set; } = "";
    public List<string> Languages { get; set; } = new();
    public string Serial { get; set; } = "";
    public string? BuildDate { get; set; } = null;
    public string? Version { get; set; } = null;
    public string? Edition { get; set; } = null;
    public string? ErrorsCount { get; set; } = null;
    public int? NumberOfTracks { get; set; } = null;
    public string? WriteOffset { get; set; } = null;
    public string AddedDate { get; set; } = "";
    public string LastModifiedDate { get; set; } = "";
    public string ExeDate { get; set; } = "";
    public string? Edc { get; set; } = null;
    public string? AntiModchip { get; set; } = null;
    public string LibCrypt { get; set; } = "";
    
    public GameInfo() { }

    public GameInfo(
        string system,
        string media,
        string category,
        string region,
        List<string> languages,
        string serial,
        string buildDate,
        string version,
        string edition,
        string errorsCount,
        int? numberOfTracks,
        string writeOffset,
        string addedDate,
        string lastModifiedDate,
        string exeDate,
        string edc,
        string antiModchip,
        string libCrypt)
    {
        System = system;
        Media = media;
        Category = category;
        Region = region;
        Languages = languages;
        Serial = serial;
        BuildDate = buildDate;
        Version = version;
        Edition = edition;
        ErrorsCount = errorsCount;
        NumberOfTracks = numberOfTracks;
        WriteOffset = writeOffset;
        AddedDate = addedDate;
        LastModifiedDate = lastModifiedDate;
        ExeDate = exeDate;
        Edc = edc;
        AntiModchip = antiModchip;
        LibCrypt = libCrypt;
    }
}
