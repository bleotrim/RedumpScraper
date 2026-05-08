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
            Id = GenerateUuidV5(disc.Id), // Generate deterministic UUID v5 from disc ID
            DiscId = long.Parse(disc.Id),
            Title = disc.Title,
            GameInfo = disc.GameInfo != null ? new GameInfoDocument
            {
                System = disc.GameInfo.System,
                Media = disc.GameInfo.Media,
                Category = disc.GameInfo.Category,
                Region = disc.GameInfo.Region,
                Languages = disc.GameInfo.Languages ?? new List<string>(),
                Serial = disc.GameInfo.Serial,
                BuildDate = disc.GameInfo.BuildDate,
                Version = disc.GameInfo.Version,
                Edition = disc.GameInfo.Edition,
                ErrorsCount = disc.GameInfo.ErrorsCount,
                NumberOfTracks = disc.GameInfo.NumberOfTracks,
                WriteOffset = disc.GameInfo.WriteOffset,
                AddedDate = disc.GameInfo.AddedDate,
                LastModifiedDate = disc.GameInfo.LastModifiedDate,
                ExeDate = disc.GameInfo.ExeDate,
                Edc = disc.GameInfo.Edc,
                AntiModchip = disc.GameInfo.AntiModchip,
                LibCrypt = disc.GameInfo.LibCrypt,
                Layerbreak = disc.GameInfo.Layerbreak
            } : null,
            GameComments = disc.GameComments != null ? new GameCommentsDocument
            {
                Metadata = disc.GameComments.Metadata,
                Comments = disc.GameComments.Comments,
                Contents = disc.GameComments.Contents,
                Barcode = disc.GameComments.Barcode
            } : null,
            TrackStatus = disc.TrackStatus,
            CuesheetStatus = disc.CuesheetStatus,
            PvdStatus = disc.PvdStatus,
            Tracks = (disc.Tracks != null && disc.Tracks.Any())
            ? disc.Tracks.Select(t => new TrackDocument
            {
                Number = t.Number,
                Type = t.Type,
                Pregap = t.Pregap,
                Length = t.Length,
                Sectors = t.Sectors,
                Size = t.Size,
                Crc32 = t.Crc32,
                Md5 = t.Md5,
                Sha1 = t.Sha1
            }).ToList()
            : null,
            Rings = (disc.Rings != null && disc.Rings.Any())
            ? disc.Rings.Select(r => new RingDocument
            {
                Number = r.Number,
                MasteringCode = r.MasteringCode,
                MasteringSidCode = r.MasteringSidCode,
                Toolstamp = r.Toolstamp,
                MouldSidCode = r.MouldSidCode,
                Status = r.Status,
                AdditionalMouldText = r.AdditionalMouldText,
                WriteOffset = r.WriteOffset
            }).ToList()
            : null,
            PvdEntries = (disc.PvdEntries != null && disc.PvdEntries.Any())
            ? disc.PvdEntries.Select(p => new PvdRecordDocument
            {
                Entry = p.Entry ?? string.Empty,
                Contents = p.Contents ?? string.Empty,
                Date = p.Date ?? string.Empty,
                Time = p.Time ?? string.Empty,
                Gmt = p.Gmt ?? string.Empty
            }).ToList()
            : null,
            LibCryptSectors = (disc.LibCryptSectors != null && disc.LibCryptSectors.Any()) 
            ? disc.LibCryptSectors.Select(l => new LibCryptSectorDocument
                {
                    Sector = l.Sector ?? string.Empty,
                    Msf = l.Msf ?? string.Empty,
                    Contents = l.Contents ?? string.Empty,
                    Xor = l.Xor ?? string.Empty,
                    Comments = l.Comments ?? string.Empty
                }).ToList() 
            : null,
            HeaderEntries = (disc.HeaderEntries != null && disc.HeaderEntries.Any()) 
            ? disc.HeaderEntries.Select(h => new HeaderEntryDocument
                {
                    Row = h.Row ?? string.Empty,
                    Contents = h.Contents ?? string.Empty,
                    Ascii = h.Ascii ?? string.Empty
                }).ToList() 
            : null,
            HeaderStatus = disc.HeaderStatus,
            SecuritySectorRanges = (disc.SecuritySectorRanges != null && disc.SecuritySectorRanges.Any()) 
            ? disc.SecuritySectorRanges.Select(s => new SecuritySectorRangeDocument
                {
                    Number = s.Number,
                    Start = s.Start,
                    End = s.End,
                    Note = s.Note ?? string.Empty
                }).ToList() 
            : null,
            Metadata = disc.Metadata != null ? new MetadataDocument
            {
                DiscKey = disc.Metadata.DiscKey ?? string.Empty,
                DiscId = disc.Metadata.DiscId ?? string.Empty,
                Pic = disc.Metadata.Pic ?? string.Empty
            } : null,
            HtmlSource = disc.HtmlSource
        };
    }

    public static RedumpDisc ToRedumpDisc(DiscDocument doc)
    {
        return new RedumpDisc
        {
            Id = doc.DiscId.ToString(),
            Title = doc.Title,
            GameInfo = doc.GameInfo != null ? new RedumpLib.GameInfo(
                doc.GameInfo.System ?? string.Empty,
                doc.GameInfo.Media ?? string.Empty,
                doc.GameInfo.Category ?? string.Empty,
                doc.GameInfo.Region ?? string.Empty,
                doc.GameInfo.Languages ?? new List<string>(),
                doc.GameInfo.Serial ?? string.Empty,
                doc.GameInfo.BuildDate ?? string.Empty,
                doc.GameInfo.Version ?? string.Empty,
                doc.GameInfo.Edition ?? string.Empty,
                doc.GameInfo.ErrorsCount ?? string.Empty,
                doc.GameInfo.NumberOfTracks,
                doc.GameInfo.WriteOffset ?? string.Empty,
                doc.GameInfo.AddedDate.GetValueOrDefault(),
                doc.GameInfo.LastModifiedDate.GetValueOrDefault(),
                doc.GameInfo.ExeDate ?? string.Empty,
                doc.GameInfo.Edc ?? string.Empty,
                doc.GameInfo.AntiModchip ?? string.Empty,
                doc.GameInfo.LibCrypt ?? string.Empty,
                doc.GameInfo.Layerbreak ?? string.Empty
            ) : null,
            GameComments = doc.GameComments != null ? new RedumpLib.GameComments(
                doc.GameComments.Metadata ?? string.Empty,
                doc.GameComments.Comments ?? string.Empty,
                doc.GameComments.Contents ?? string.Empty,
                doc.GameComments.Barcode ?? string.Empty
            ) : null,
            TrackStatus = doc.TrackStatus,
            CuesheetStatus = doc.CuesheetStatus,
            PvdStatus = doc.PvdStatus,
            Tracks = doc.Tracks?.Select(t => new DiscTrack(
                t.Number, t.Type, t.Pregap, t.Length, t.Sectors, t.Size, t.Crc32, t.Md5, t.Sha1
            )).ToList(),
            Rings = doc.Rings?.Select(r => new DiscRing(
                r.Number, r.MasteringCode, r.MasteringSidCode, r.Toolstamp, r.MouldSidCode, r.Status
            )).ToList(),
            PvdEntries = doc.PvdEntries?.Select(p => new PvdRecord(
                p.Entry ?? string.Empty, 
                p.Contents ?? string.Empty, 
                p.Date ?? string.Empty, 
                p.Time ?? string.Empty, 
                p.Gmt ?? string.Empty
            )).ToList(),
            LibCryptSectors = doc.LibCryptSectors?.Select(l => new LibCryptSector(
                l.Sector ?? string.Empty, 
                l.Msf ?? string.Empty, 
                l.Contents ?? string.Empty, 
                l.Xor ?? string.Empty, 
                l.Comments ?? string.Empty
            )).ToList(),
            HeaderEntries = doc.HeaderEntries?.Select(h => new HeaderEntry(
                h.Row ?? string.Empty, 
                h.Contents ?? string.Empty, 
                h.Ascii ?? string.Empty
            )).ToList(),
            HeaderStatus = doc.HeaderStatus,
            SecuritySectorRanges = doc.SecuritySectorRanges?.Select(s => new SecuritySectorRange(
                s.Number, s.Start, s.End, s.Note ?? string.Empty
            )).ToList(),
            Metadata = doc.Metadata != null ? new Metadata(
                doc.Metadata.DiscKey ?? string.Empty, 
                doc.Metadata.DiscId ?? string.Empty, 
                doc.Metadata.Pic ?? string.Empty
            ) : null,
            HtmlSource = doc.HtmlSource
        };
    }
}