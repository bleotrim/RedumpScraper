using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RedumpLib;

// Helper method to check if a value should be displayed
static bool HasValue(string? value) => !string.IsNullOrWhiteSpace(value);

Console.WriteLine("⚠️  Redump Disc Scraper - Responsible Use Reminder:");
Console.WriteLine("   • Use only for discs you personally own");
Console.WriteLine("   • See README.md for ethical usage guidelines\n");

if (args.Length == 0)
{
    Console.WriteLine("Usage: RedumpApp <disc-id>");
    Console.WriteLine("       RedumpApp --serial <serial-number>");
    Console.WriteLine("       RedumpApp --hash <hash-value>");
    Console.WriteLine("\nExamples:");
    Console.WriteLine("  RedumpApp 1041");
    Console.WriteLine("  RedumpApp --serial SCES-01518");
    Console.WriteLine("  RedumpApp --hash f45c579064568e96f6a01f16fc7b726f");
    return;
}

var scraper = new Scraper();
RedumpDisc disc;

try
{
    // Check if it's a search by serial or hash
    if (args[0] == "--serial" || args[0] == "--hash")
    {
        if (args.Length < 2)
        {
            Console.WriteLine($"Error: {args[0]} requires a search value");
            return;
        }

        string searchQuery = args[1];
        Console.WriteLine($"[DEBUG] Searching for: {searchQuery}\n");

        try
        {
            disc = await scraper.SearchRedumpByQuickSearchAsync(searchQuery);
        }
        catch (InvalidOperationException ex)
        {
            // Multiple results found - display them and ask user to select
            Console.WriteLine(ex.Message);
            Console.WriteLine("\nUsage: RedumpApp <disc-id>");
            Console.WriteLine("Example: RedumpApp 2869");
            return;
        }
    }
    else
    {
        // Direct disc ID lookup
        string discId = args[0];

        if (string.IsNullOrWhiteSpace(discId))
        {
            Console.WriteLine("Error: Disc ID cannot be empty.");
            return;
        }

        string url = $"http://redump.org/disc/{discId}/";
        Console.WriteLine($"[DEBUG] Connecting to: {url}");
        disc = scraper.ParseRedumpPage(url);
    }

    // GAME INFO
    if (HasValue(disc.Id) || HasValue(disc.Title) ||
        (disc.GameInfo != null && (HasValue(disc.GameInfo.System) || HasValue(disc.GameInfo.Media) ||
        HasValue(disc.GameInfo.Category) || HasValue(disc.GameInfo.Serial) || HasValue(disc.GameInfo.Region) ||
        HasValue(disc.GameInfo.Edition) || HasValue(disc.GameInfo.Version))))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("GAME INFO".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.Id)) Console.WriteLine($"ID:            {disc.Id}");
        if (HasValue(disc.Title)) Console.WriteLine($"Title:         {disc.Title}");
        if (disc.GameInfo != null)
        {
            if (HasValue(disc.GameInfo.System)) Console.WriteLine($"System:        {disc.GameInfo.System}");
            if (HasValue(disc.GameInfo.Media)) Console.WriteLine($"Media:         {disc.GameInfo.Media}");
            if (HasValue(disc.GameInfo.Category)) Console.WriteLine($"Category:      {disc.GameInfo.Category}");
            if (HasValue(disc.GameInfo.Serial)) Console.WriteLine($"Serial:        {disc.GameInfo.Serial}");
            if (HasValue(disc.GameInfo.Region)) Console.WriteLine($"Region:        {disc.GameInfo.Region}");
            if (HasValue(disc.GameInfo.Edition)) Console.WriteLine($"Edition:       {disc.GameInfo.Edition}");
            if (HasValue(disc.GameInfo.Version)) Console.WriteLine($"Version:       {disc.GameInfo.Version}");
            if (disc.GameInfo != null && disc.GameInfo.Languages.Count > 0)
            {
                Console.WriteLine("Languages:");
                foreach (var lang in disc.GameInfo.Languages)
                {
                    Console.WriteLine($"               • {lang}");
                }
            }
            if (HasValue(disc.GameInfo.BuildDate)) Console.WriteLine($"Build Date:     {disc.GameInfo.BuildDate}");
            if (HasValue(disc.GameInfo.ExeDate)) Console.WriteLine($"EXE Date:      {disc.GameInfo.ExeDate}");
            if (HasValue(disc.GameInfo.Edc)) Console.WriteLine($"EDC:     {disc.GameInfo.Edc}");
            if (HasValue(disc.GameInfo.AntiModchip)) Console.WriteLine($"Anti-modchip:     {disc.GameInfo.AntiModchip}");
            if (HasValue(disc.GameInfo.LibCrypt)) Console.WriteLine($"LibCrypt:     {disc.GameInfo.LibCrypt}");
            if (HasValue(disc.GameInfo.ErrorsCount)) Console.WriteLine($"Errors Count:     {disc.GameInfo.ErrorsCount}");
            if (HasValue(disc.GameInfo.WriteOffset)) Console.WriteLine($"Write Offset:     {disc.GameInfo.WriteOffset}");
            if (HasValue(disc.GameInfo.NumberOfTracks)) Console.WriteLine($"Number of Tracks:       {disc.GameInfo.NumberOfTracks}");
            if (HasValue(disc.GameInfo.AddedDate)) Console.WriteLine($"Added:         {disc.GameInfo.AddedDate}");
            if (HasValue(disc.GameInfo.LastModifiedDate)) Console.WriteLine($"Last Modified: {disc.GameInfo.LastModifiedDate}");
        }
    }

    // BARCODE
    if (HasValue(disc.Barcode))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("BARCODE".PadRight(70));
        Console.WriteLine(new string('=', 70));
        Console.WriteLine(disc.Barcode);
    }

    // GAME COMMENTS
    if (disc.GameComments != null)
    {
        // Metadata
        if (HasValue(disc.GameComments.Metadata))
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("METADATA".PadRight(70));
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(disc.GameComments.Metadata);
        }

        // Comments
        if (HasValue(disc.GameComments.Comments))
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("COMMENTS".PadRight(70));
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(disc.GameComments.Comments);
        }

        // Contents
        if (HasValue(disc.GameComments.Contents))
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("CONTENTS".PadRight(70));
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(disc.GameComments.Contents);
        }
    }

    // TRACKS
    if (disc.Tracks.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"TRACKS ({disc.Tracks.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.TrackStatus))
        {
            Console.WriteLine($"Tracks Status:  {disc.TrackStatus}");
        }
        if (HasValue(disc.CuesheetStatus))
        {
            Console.WriteLine($"Cuesheet Status: {disc.CuesheetStatus}");
        }
        if (HasValue(disc.TrackStatus) || HasValue(disc.CuesheetStatus))
        {
            Console.WriteLine();
        }
        foreach (var track in disc.Tracks)
        {
            Console.WriteLine($"\nTrack {track.Number}:");
            if (HasValue(track.Type)) Console.WriteLine($"  Type:    {track.Type}");
            if (HasValue(track.Size)) Console.WriteLine($"  Size:    {track.Size}");
            if (HasValue(track.Crc32)) Console.WriteLine($"  CRC32:   {track.Crc32}");
            if (HasValue(track.Md5)) Console.WriteLine($"  MD5:     {track.Md5}");
            if (HasValue(track.Sha1)) Console.WriteLine($"  SHA1:    {track.Sha1}");
        }
    }

    // RINGS
    if (disc.Rings.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"RINGS ({disc.Rings.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        foreach (var ring in disc.Rings)
        {
            bool hasRingData = HasValue(ring.MasteringCode) || HasValue(ring.MasteringSidCode) ||
                              HasValue(ring.Toolstamp) || HasValue(ring.MouldSidCode) || HasValue(ring.Status);

            if (hasRingData)
            {
                Console.WriteLine($"\nRing {ring.Number}:");
                if (HasValue(ring.MasteringCode)) Console.WriteLine($"  Mastering Code:       {ring.MasteringCode}");
                if (HasValue(ring.MasteringSidCode)) Console.WriteLine($"  Mastering SID Code:   {ring.MasteringSidCode}");
                if (HasValue(ring.Toolstamp)) Console.WriteLine($"  Toolstamp:            {ring.Toolstamp}");
                if (HasValue(ring.MouldSidCode)) Console.WriteLine($"  Mould SID Code:       {ring.MouldSidCode}");
                if (HasValue(ring.Status)) Console.WriteLine($"  Status:               {ring.Status}");
            }
        }
    }

    // PRIMARY VOLUME DESCRIPTOR (PVD)
    if (disc.PvdEntries.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"PRIMARY VOLUME DESCRIPTOR (PVD) ({disc.PvdEntries.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.PvdStatus))
        {
            Console.WriteLine($"Status: {disc.PvdStatus}\n");
        }
        foreach (var pvd in disc.PvdEntries)
        {
            bool hasPvdData = HasValue(pvd.Contents) || HasValue(pvd.Date) || HasValue(pvd.Time) || HasValue(pvd.Gmt);

            if (hasPvdData)
            {
                Console.WriteLine($"\n{pvd.Entry}:");
                if (HasValue(pvd.Contents)) Console.WriteLine($"  Contents: {pvd.Contents}");
                if (HasValue(pvd.Date)) Console.WriteLine($"  Date:     {pvd.Date}");
                if (HasValue(pvd.Time)) Console.WriteLine($"  Time:     {pvd.Time}");
                if (HasValue(pvd.Gmt)) Console.WriteLine($"  GMT:      {pvd.Gmt}");
            }
        }
    }

    // LIBCRYPT PROTECTION
    if (disc.LibCryptSectors.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"LIBCRYPT PROTECTION ({disc.LibCryptSectors.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        foreach (var sector in disc.LibCryptSectors)
        {
            Console.WriteLine($"\nSector {sector.Sector}:");
            if (HasValue(sector.Msf)) Console.WriteLine($"  MSF:      {sector.Msf}");
            if (HasValue(sector.Contents)) Console.WriteLine($"  Contents: {sector.Contents}");
            if (HasValue(sector.Xor)) Console.WriteLine($"  XOR:      {sector.Xor}");
            if (HasValue(sector.Comments)) Console.WriteLine($"  Comments: {sector.Comments}");
        }
    }

    // HEADER ENTRIES
    if (disc.HeaderEntries.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"HEADER ENTRIES ({disc.HeaderEntries.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.HeaderStatus))
        {
            Console.WriteLine($"Status: {disc.HeaderStatus}\n");
        }
        foreach (var entry in disc.HeaderEntries)
        {
            Console.WriteLine($"\n");
            if (HasValue(entry.Row)) Console.WriteLine($"  Row:      {entry.Row}");
            if (HasValue(entry.Contents)) Console.WriteLine($"  Contents: {entry.Contents}");
            if (HasValue(entry.Ascii)) Console.WriteLine($"  ASCII:    {entry.Ascii}");
        }
    }

    // SECURITY SECTOR RANGES
    if (disc.SecuritySectorRanges.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"SECURITY SECTOR RANGES ({disc.SecuritySectorRanges.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        foreach (var ssr in disc.SecuritySectorRanges)
        {
            Console.WriteLine($"Range {ssr.Number}: Start={ssr.Start}, End={ssr.End}");
            if (!string.IsNullOrWhiteSpace(ssr.Note))
                Console.WriteLine($"  Note: {ssr.Note}");
        }
    }

    // METADATA
    if (disc.Metadata != null)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("METADATA".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.Metadata.DiscKey))
            Console.WriteLine($"Disc Key: {disc.Metadata.DiscKey}");
        if (HasValue(disc.Metadata.DiscId))
            Console.WriteLine($"Disc ID:  {disc.Metadata.DiscId}");
        if (HasValue(disc.Metadata.Pic))
        {
            Console.WriteLine($"Permanent Information & Control (PIC):");
            Console.WriteLine(disc.Metadata.Pic);
        }
    }

    Console.WriteLine("\n" + new string('=', 70));
}
catch (Exception ex)
{
    Console.WriteLine($"Error during scraping: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}