using System;
using RedumpLib;

// Helper method to check if a value should be displayed
static bool HasValue(string? value) => !string.IsNullOrWhiteSpace(value);

if (args.Length == 0)
{
    Console.WriteLine("Usage: RedumpApp <disc-id>");
    Console.WriteLine("Example: RedumpApp 1041");
    return;
}

string discId = args[0];

if (string.IsNullOrWhiteSpace(discId))
{
    Console.WriteLine("Error: Disc ID cannot be empty.");
    return;
}

string url = $"http://redump.org/disc/{discId}/";
var scraper = new Scraper();

try
{
    Console.WriteLine($"[DEBUG] Connecting to: {url}");
    RedumpDisc disc = scraper.ParseRedumpPage(url);

    // BASIC INFORMATION
    if (HasValue(disc.Id) || HasValue(disc.Title) || HasValue(disc.System) || HasValue(disc.Media) || 
        HasValue(disc.Category) || HasValue(disc.Serial) || HasValue(disc.Region) || HasValue(disc.Edition) || 
        HasValue(disc.Version))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("BASIC INFORMATION".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.Id)) Console.WriteLine($"ID:          {disc.Id}");
        if (HasValue(disc.Title)) Console.WriteLine($"Title:       {disc.Title}");
        if (HasValue(disc.System)) Console.WriteLine($"System:      {disc.System}");
        if (HasValue(disc.Media)) Console.WriteLine($"Media:       {disc.Media}");
        if (HasValue(disc.Category)) Console.WriteLine($"Category:    {disc.Category}");
        if (HasValue(disc.Serial)) Console.WriteLine($"Serial:      {disc.Serial}");
        if (HasValue(disc.Region)) Console.WriteLine($"Region:      {disc.Region}");
        if (HasValue(disc.Edition)) Console.WriteLine($"Edition:     {disc.Edition}");
        if (HasValue(disc.Version)) Console.WriteLine($"Version:     {disc.Version}");
    }

    // LANGUAGES
    if (disc.Languages.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("LANGUAGES".PadRight(70));
        Console.WriteLine(new string('=', 70));
        foreach (var lang in disc.Languages)
        {
            Console.WriteLine($"  • {lang}");
        }
    }

    // TECHNICAL DETAILS
    if (HasValue(disc.ExeDate) || HasValue(disc.Edc) || HasValue(disc.AntiModchip) || 
        HasValue(disc.LibCrypt) || HasValue(disc.ErrorsCount) || HasValue(disc.WriteOffset) || 
        HasValue(disc.NumberOfTracks))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("TECHNICAL DETAILS".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.ExeDate)) Console.WriteLine($"EXE Date:        {disc.ExeDate}");
        if (HasValue(disc.Edc)) Console.WriteLine($"EDC:             {disc.Edc}");
        if (HasValue(disc.AntiModchip)) Console.WriteLine($"Anti-modchip:    {disc.AntiModchip}");
        if (HasValue(disc.LibCrypt)) Console.WriteLine($"LibCrypt:        {disc.LibCrypt}");
        if (HasValue(disc.ErrorsCount)) Console.WriteLine($"Errors Count:    {disc.ErrorsCount}");
        if (HasValue(disc.WriteOffset)) Console.WriteLine($"Write Offset:    {disc.WriteOffset}");
        if (HasValue(disc.NumberOfTracks)) Console.WriteLine($"Number of Tracks: {disc.NumberOfTracks}");
    }

    // DATABASE INFORMATION
    if (HasValue(disc.AddedDate) || HasValue(disc.LastModifiedDate))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("DATABASE INFORMATION".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (HasValue(disc.AddedDate)) Console.WriteLine($"Added:          {disc.AddedDate}");
        if (HasValue(disc.LastModifiedDate)) Console.WriteLine($"Last Modified:  {disc.LastModifiedDate}");
    }

    // BARCODE
    if (HasValue(disc.Barcode))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("BARCODE".PadRight(70));
        Console.WriteLine(new string('=', 70));
        Console.WriteLine(disc.Barcode);
    }

    // COMMENTS
    if (HasValue(disc.Comments))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("COMMENTS".PadRight(70));
        Console.WriteLine(new string('=', 70));
        Console.WriteLine(disc.Comments);
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

    Console.WriteLine("\n" + new string('=', 70));
}
catch (Exception ex)
{
    Console.WriteLine($"Error during scraping: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}