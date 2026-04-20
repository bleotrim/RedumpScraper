using RedumpLib;
using RedumpDatabase.Models;
using System.Security.Cryptography;

namespace RedumpDatabase.Mappers;

/// <summary>
/// Mapper for converting RedumpDisc objects to MongoDB DiscDocument objects
/// </summary>
public static class DiscMapper
{
    // Standard DNS namespace UUID for UUID v5 generation
    private static readonly Guid DnsNamespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

    /// <summary>
    /// Generate a UUID v5 from a disc ID using SHA-1 hashing
    /// </summary>
    private static string GenerateUuidV5(string discId)
    {
        // Combine namespace bytes with the disc ID name
        var namespaceBytes = DnsNamespace.ToByteArray();
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(discId);
        
        var combined = new byte[namespaceBytes.Length + nameBytes.Length];
        Buffer.BlockCopy(namespaceBytes, 0, combined, 0, namespaceBytes.Length);
        Buffer.BlockCopy(nameBytes, 0, combined, namespaceBytes.Length, nameBytes.Length);
        
        // Hash using SHA-1
        byte[] hash = SHA1.HashData(combined);
        
        // Take first 16 bytes and set version/variant bits for UUID v5
        var guidBytes = new byte[16];
        Buffer.BlockCopy(hash, 0, guidBytes, 0, 16);
        
        // Set version to 5 (SHA-1)
        guidBytes[6] = (byte)((guidBytes[6] & 0x0f) | 0x50);
        
        // Set variant to RFC 4122
        guidBytes[8] = (byte)((guidBytes[8] & 0x3f) | 0x80);
        
        // Return as string representation
        return new Guid(guidBytes).ToString();
    }

    public static DiscDocument ToDocument(RedumpDisc disc)
    {
        return new DiscDocument
        {
            Id = GenerateUuidV5(disc.Id ?? ""), // Generate deterministic UUID v5 from disc ID
            DiscId = disc.Id ?? string.Empty,
            Title = disc.Title ?? string.Empty,
            System = disc.System ?? string.Empty,
            Media = disc.Media ?? string.Empty,
            Category = disc.Category ?? string.Empty,
            Serial = disc.Serial ?? string.Empty,
            Region = disc.Region ?? string.Empty,
            Edition = disc.Edition ?? string.Empty,
            Version = disc.Version ?? string.Empty,
            Languages = disc.Languages ?? new List<string>(),
            ExeDate = disc.ExeDate ?? string.Empty,
            Edc = disc.Edc ?? string.Empty,
            AntiModchip = disc.AntiModchip ?? string.Empty,
            LibCrypt = disc.LibCrypt ?? string.Empty,
            ErrorsCount = disc.ErrorsCount ?? string.Empty,
            NumberOfTracks = disc.NumberOfTracks ?? string.Empty,
            WriteOffset = disc.WriteOffset ?? string.Empty,
            Barcode = disc.Barcode ?? string.Empty,
            Comments = disc.Comments ?? string.Empty,
            TrackStatus = disc.TrackStatus ?? string.Empty,
            CuesheetStatus = disc.CuesheetStatus ?? string.Empty,
            PvdStatus = disc.PvdStatus ?? string.Empty,
            AddedDate = disc.AddedDate ?? string.Empty,
            LastModifiedDate = disc.LastModifiedDate ?? string.Empty,
            Tracks = disc.Tracks.Select(t => new TrackDocument
            {
                Number = t.Number ?? string.Empty,
                Type = t.Type ?? string.Empty,
                Size = t.Size ?? string.Empty,
                Crc32 = t.Crc32 ?? string.Empty,
                Md5 = t.Md5 ?? string.Empty,
                Sha1 = t.Sha1 ?? string.Empty
            }).ToList(),
            Rings = disc.Rings.Select(r => new RingDocument
            {
                Number = r.Number ?? string.Empty,
                MasteringCode = r.MasteringCode ?? string.Empty,
                MasteringSidCode = r.MasteringSidCode ?? string.Empty,
                Toolstamp = r.Toolstamp ?? string.Empty,
                MouldSidCode = r.MouldSidCode ?? string.Empty,
                Status = r.Status ?? string.Empty
            }).ToList(),
            PvdEntries = disc.PvdEntries.Select(p => new PvdRecordDocument
            {
                Entry = p.Entry ?? string.Empty,
                Contents = p.Contents ?? string.Empty,
                Date = p.Date ?? string.Empty,
                Time = p.Time ?? string.Empty,
                Gmt = p.Gmt ?? string.Empty
            }).ToList(),
            LibCryptSectors = disc.LibCryptSectors.Select(l => new LibCryptSectorDocument
            {
                Sector = l.Sector ?? string.Empty,
                Msf = l.Msf ?? string.Empty,
                Contents = l.Contents ?? string.Empty,
                Xor = l.Xor ?? string.Empty,
                Comments = l.Comments ?? string.Empty
            }).ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static RedumpDisc ToRedumpDisc(DiscDocument doc)
    {
        return new RedumpDisc
        {
            Id = doc.DiscId,
            Title = doc.Title,
            System = doc.System,
            Media = doc.Media,
            Category = doc.Category,
            Serial = doc.Serial,
            Region = doc.Region,
            Edition = doc.Edition,
            Version = doc.Version,
            Languages = doc.Languages,
            ExeDate = doc.ExeDate,
            Edc = doc.Edc,
            AntiModchip = doc.AntiModchip,
            LibCrypt = doc.LibCrypt,
            ErrorsCount = doc.ErrorsCount,
            NumberOfTracks = doc.NumberOfTracks,
            WriteOffset = doc.WriteOffset,
            Barcode = doc.Barcode,
            Comments = doc.Comments,
            TrackStatus = doc.TrackStatus,
            CuesheetStatus = doc.CuesheetStatus,
            PvdStatus = doc.PvdStatus,
            AddedDate = doc.AddedDate,
            LastModifiedDate = doc.LastModifiedDate,
            Tracks = doc.Tracks.Select(t => new DiscTrack(
                t.Number, t.Type, t.Size, t.Crc32, t.Md5, t.Sha1
            )).ToList(),
            Rings = doc.Rings.Select(r => new DiscRing(
                r.Number, r.MasteringCode, r.MasteringSidCode, r.Toolstamp, r.MouldSidCode, r.Status
            )).ToList(),
            PvdEntries = doc.PvdEntries.Select(p => new PvdRecord(
                p.Entry, p.Contents, p.Date, p.Time, p.Gmt
            )).ToList(),
            LibCryptSectors = doc.LibCryptSectors.Select(l => new LibCryptSector(
                l.Sector, l.Msf, l.Contents, l.Xor, l.Comments
            )).ToList()
        };
    }
}
