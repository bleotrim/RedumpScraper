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
            GameInfo = disc.GameInfo != null ? new GameInfoDocument
            {
                System = disc.GameInfo.System ?? null,
                Media = disc.GameInfo.Media ?? null,
                Category = disc.GameInfo.Category ?? null,
                Region = disc.GameInfo.Region ?? null,
                Languages = disc.GameInfo.Languages ?? new List<string>(),
                Serial = disc.GameInfo.Serial ?? null,
                BuildDate = disc.GameInfo.BuildDate ?? null,
                Version = disc.GameInfo.Version ?? null,
                Edition = disc.GameInfo.Edition ?? null,
                ErrorsCount = disc.GameInfo.ErrorsCount ?? null,
                NumberOfTracks = disc.GameInfo.NumberOfTracks ?? null,
                WriteOffset = disc.GameInfo.WriteOffset ?? null,
                AddedDate = disc.GameInfo.AddedDate ?? null,
                LastModifiedDate = disc.GameInfo.LastModifiedDate ?? null,
                ExeDate = disc.GameInfo.ExeDate ?? null,
                Edc = disc.GameInfo.Edc ?? null,
                AntiModchip = disc.GameInfo.AntiModchip ?? null,
                LibCrypt = disc.GameInfo.LibCrypt ?? null
            } : null,
            GameComments = disc.GameComments != null ? new GameCommentsDocument
            {
                Metadata = disc.GameComments.Metadata ?? null,
                Comments = disc.GameComments.Comments ?? null,
                Contents = disc.GameComments.Contents ?? null,
                Barcode = disc.GameComments.Barcode ?? null
            } : null,
            TrackStatus = disc.TrackStatus ?? null,
            CuesheetStatus = disc.CuesheetStatus ?? null,
            PvdStatus = disc.PvdStatus ?? null,
            Tracks = (disc.Tracks != null && disc.Tracks.Any())
            ? disc.Tracks.Select(t => new TrackDocument
            {
                Number = t.Number ?? string.Empty,
                Type = t.Type ?? string.Empty,
                Pregap = t.Pregap ?? string.Empty,
                Length = t.Length ?? string.Empty,
                Sectors = t.Sectors ?? string.Empty,
                Size = t.Size ?? string.Empty,
                Crc32 = t.Crc32 ?? string.Empty,
                Md5 = t.Md5 ?? string.Empty,
                Sha1 = t.Sha1 ?? string.Empty
            }).ToList()
            : null,
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
            HeaderEntries = disc.HeaderEntries.Select(h => new HeaderEntryDocument
            {
                Row = h.Row ?? string.Empty,
                Contents = h.Contents ?? string.Empty,
                Ascii = h.Ascii ?? string.Empty
            }).ToList(),
            HeaderStatus = disc.HeaderStatus ?? null,
            SecuritySectorRanges = disc.SecuritySectorRanges.Select(s => new SecuritySectorRangeDocument
            {
                Number = s.Number,
                Start = s.Start,
                End = s.End,
                Note = s.Note
            }).ToList(),
            Metadata = disc.Metadata != null ? new MetadataDocument
            {
                DiscKey = disc.Metadata.DiscKey ?? string.Empty,
                DiscId = disc.Metadata.DiscId ?? string.Empty,
                Pic = disc.Metadata.Pic ?? string.Empty
            } : null,
            HtmlSource = disc.HtmlSource ?? null,
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
            GameInfo = doc.GameInfo != null ? new RedumpLib.GameInfo(
                doc.GameInfo.System,
                doc.GameInfo.Media,
                doc.GameInfo.Category,
                doc.GameInfo.Region,
                doc.GameInfo.Languages,
                doc.GameInfo.Serial,
                doc.GameInfo.BuildDate,
                doc.GameInfo.Version,
                doc.GameInfo.Edition,
                doc.GameInfo.ErrorsCount,
                doc.GameInfo.NumberOfTracks,
                doc.GameInfo.WriteOffset,
                doc.GameInfo.AddedDate,
                doc.GameInfo.LastModifiedDate,
                doc.GameInfo.ExeDate,
                doc.GameInfo.Edc,
                doc.GameInfo.AntiModchip,
                doc.GameInfo.LibCrypt
            ) : null,
            GameComments = doc.GameComments != null ? new RedumpLib.GameComments(
                doc.GameComments.Metadata,
                doc.GameComments.Comments,
                doc.GameComments.Contents,
                doc.GameComments.Barcode
            ) : null,
            TrackStatus = doc.TrackStatus,
            CuesheetStatus = doc.CuesheetStatus,
            PvdStatus = doc.PvdStatus,
            Tracks = doc.Tracks.Select(t => new DiscTrack(
                t.Number, t.Type, t.Pregap, t.Length, t.Sectors, t.Size, t.Crc32, t.Md5, t.Sha1
            )).ToList(),
            Rings = doc.Rings.Select(r => new DiscRing(
                r.Number, r.MasteringCode, r.MasteringSidCode, r.Toolstamp, r.MouldSidCode, r.Status
            )).ToList(),
            PvdEntries = doc.PvdEntries.Select(p => new PvdRecord(
                p.Entry, p.Contents, p.Date, p.Time, p.Gmt
            )).ToList(),
            LibCryptSectors = doc.LibCryptSectors.Select(l => new LibCryptSector(
                l.Sector, l.Msf, l.Contents, l.Xor, l.Comments
            )).ToList(),
            HeaderEntries = doc.HeaderEntries.Select(h => new HeaderEntry(
                h.Row, h.Contents, h.Ascii
            )).ToList(),
            HeaderStatus = doc.HeaderStatus,
            SecuritySectorRanges = doc.SecuritySectorRanges.Select(s => new SecuritySectorRange(
                s.Number, s.Start, s.End, s.Note
            )).ToList(),
            Metadata = doc.Metadata != null ? new Metadata(
                doc.Metadata.DiscKey, doc.Metadata.DiscId, doc.Metadata.Pic
            ) : null,
            HtmlSource = doc.HtmlSource ?? null
        };
    }
}
