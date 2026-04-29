namespace RedumpLib;

/// <summary>
/// Represents game-specific comments and metadata from the gamecomments section
/// </summary>
public class GameComments
{
    /// <summary>Catalog and metadata information</summary>
    public string? Metadata { get; set; } = null;

    /// <summary>Comments about the disc (internal serial, volume label, etc.)</summary>
    public string Comments { get; set; } = "";

    /// <summary>Contents listing (games, files, etc.)</summary>
    public string Contents { get; set; } = "";

    /// <summary>Barcode information, if available</summary>
    public string Barcode { get; set; } = "";

    public GameComments() { }

    public GameComments(string metadata, string comments, string contents, string barcode)
    {
        Metadata = metadata;
        Comments = comments;
        Contents = contents;
        Barcode = barcode;
    }
}
