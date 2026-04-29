using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RedumpDatabase.Models;

/// <summary>
/// MongoDB document model for storing Redump disc information
/// </summary>
[BsonIgnoreExtraElements]
public class DiscDocument
{
    [BsonId]
    public string Id { get; set; } = string.Empty;

    [BsonElement("disc_id")]
    public string DiscId { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("game_info")]
    public GameInfoDocument? GameInfo { get; set; }

    [BsonElement("game_comments")]
    public GameCommentsDocument? GameComments { get; set; }

    [BsonElement("track_status")]
    public string TrackStatus { get; set; } = string.Empty;

    [BsonElement("cuesheet_status")]
    public string CuesheetStatus { get; set; } = string.Empty;

    [BsonElement("pvd_status")]
    public string PvdStatus { get; set; } = string.Empty;

    [BsonElement("tracks")]
    public List<TrackDocument> Tracks { get; set; } = new();

    [BsonElement("rings")]
    public List<RingDocument> Rings { get; set; } = new();

    [BsonElement("pvd_entries")]
    public List<PvdRecordDocument> PvdEntries { get; set; } = new();

    [BsonElement("libcrypt_sectors")]
    public List<LibCryptSectorDocument> LibCryptSectors { get; set; } = new();

    [BsonElement("header_entries")]
    public List<HeaderEntryDocument> HeaderEntries { get; set; } = new();

    [BsonElement("header_status")]
    public string HeaderStatus { get; set; } = string.Empty;

    [BsonElement("security_sector_ranges")]
    public List<SecuritySectorRangeDocument> SecuritySectorRanges { get; set; } = new();

    [BsonElement("metadata")]
    public MetadataDocument? Metadata { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

[BsonIgnoreExtraElements]
public class HeaderEntryDocument
{
    [BsonElement("row")]
    public string Row { get; set; } = string.Empty;

    [BsonElement("contents")]
    public string Contents { get; set; } = string.Empty;

    [BsonElement("ascii")]
    public string Ascii { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class SecuritySectorRangeDocument
{
    [BsonElement("number")]
    public int Number { get; set; }

    [BsonElement("start")]
    public int Start { get; set; }

    [BsonElement("end")]
    public int End { get; set; }

    [BsonElement("note")]
    public string? Note { get; set; }
}

[BsonIgnoreExtraElements]
public class MetadataDocument
{
    [BsonElement("disc_key")]
    public string DiscKey { get; set; } = string.Empty;

    [BsonElement("disc_id")]
    public string DiscId { get; set; } = string.Empty;

    [BsonElement("pic")]
    public string Pic { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class GameCommentsDocument
{
    [BsonElement("metadata")]
    public string Metadata { get; set; } = string.Empty;

    [BsonElement("comments")]
    public string Comments { get; set; } = string.Empty;

    [BsonElement("contents")]
    public string Contents { get; set; } = string.Empty;

    [BsonElement("barcode")]
    public string Barcode { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class GameInfoDocument
{
    [BsonElement("system")]
    public string System { get; set; } = string.Empty;

    [BsonElement("media")]
    public string Media { get; set; } = string.Empty;

    [BsonElement("category")]
    public string Category { get; set; } = string.Empty;

    [BsonElement("region")]
    public string Region { get; set; } = string.Empty;

    [BsonElement("languages")]
    public List<string> Languages { get; set; } = new();

    [BsonElement("serial")]
    public string Serial { get; set; } = string.Empty;

    [BsonElement("build_date")]
    public string? BuildDate { get; set; } = null;

    [BsonElement("version")]
    public string? Version { get; set; } = null;

    [BsonElement("edition")]
    public string? Edition { get; set; } = null;

    [BsonElement("errors_count")]
    public string ErrorsCount { get; set; } = string.Empty;

    [BsonElement("number_of_tracks")]
    public int? NumberOfTracks { get; set; } = null;

    [BsonElement("write_offset")]
    public string WriteOffset { get; set; } = string.Empty;

    [BsonElement("added_date")]
    public string AddedDate { get; set; } = string.Empty;

    [BsonElement("last_modified_date")]
    public string LastModifiedDate { get; set; } = string.Empty;

    [BsonElement("exe_date")]
    public string ExeDate { get; set; } = string.Empty;

    [BsonElement("edc")]
    public string Edc { get; set; } = string.Empty;

    [BsonElement("anti_modchip")]
    public string AntiModchip { get; set; } = string.Empty;

    [BsonElement("libcrypt")]
    public string LibCrypt { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class TrackDocument
{
    [BsonElement("number")]
    public string Number { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("pregap")]
    public string Pregap { get; set; } = string.Empty;

    [BsonElement("length")]
    public string Length { get; set; } = string.Empty;

    [BsonElement("sectors")]
    public string Sectors { get; set; } = string.Empty;

    [BsonElement("size")]
    public string Size { get; set; } = string.Empty;

    [BsonElement("crc32")]
    public string Crc32 { get; set; } = string.Empty;

    [BsonElement("md5")]
    public string Md5 { get; set; } = string.Empty;

    [BsonElement("sha1")]
    public string Sha1 { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class RingDocument
{
    [BsonElement("number")]
    public string Number { get; set; } = string.Empty;

    [BsonElement("mastering_code")]
    public string MasteringCode { get; set; } = string.Empty;

    [BsonElement("mastering_sid_code")]
    public string MasteringSidCode { get; set; } = string.Empty;

    [BsonElement("toolstamp")]
    public string Toolstamp { get; set; } = string.Empty;

    [BsonElement("mould_sid_code")]
    public string MouldSidCode { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class PvdRecordDocument
{
    [BsonElement("entry")]
    public string Entry { get; set; } = string.Empty;

    [BsonElement("contents")]
    public string Contents { get; set; } = string.Empty;

    [BsonElement("date")]
    public string Date { get; set; } = string.Empty;

    [BsonElement("time")]
    public string Time { get; set; } = string.Empty;

    [BsonElement("gmt")]
    public string Gmt { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class LibCryptSectorDocument
{
    [BsonElement("sector")]
    public string Sector { get; set; } = string.Empty;

    [BsonElement("msf")]
    public string Msf { get; set; } = string.Empty;

    [BsonElement("contents")]
    public string Contents { get; set; } = string.Empty;

    [BsonElement("xor")]
    public string Xor { get; set; } = string.Empty;

    [BsonElement("comments")]
    public string Comments { get; set; } = string.Empty;
}
