namespace RedumpLib;

/// <summary>
/// Represents a header entry containing hex data and ASCII representation
/// </summary>
public class HeaderEntry
{
    /// <summary>Row offset (e.g., "0000", "0010")</summary>
    public string Row { get; set; }
    
    /// <summary>Hex contents (e.g., "53 45 47 41 20 53 45 47 41 4B 41 54 41 4E 41 20")</summary>
    public string Contents { get; set; }
    
    /// <summary>ASCII representation (e.g., "SEGA SEGAKATANA ")</summary>
    public string Ascii { get; set; }

    public HeaderEntry(string Row, string Contents, string Ascii)
    {
        this.Row = Row;
        this.Contents = Contents;
        this.Ascii = Ascii;
    }
}
