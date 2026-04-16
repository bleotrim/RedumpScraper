using System;
using RedumpLib;

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

    Console.WriteLine("\n" + new string('=', 70));
    Console.WriteLine("BASIC INFORMATION".PadRight(70));
    Console.WriteLine(new string('=', 70));
    Console.WriteLine($"ID:          {disc.Id}");
    Console.WriteLine($"Title:       {disc.Title}");
    Console.WriteLine($"System:      {disc.System}");
    Console.WriteLine($"Media:       {disc.Media}");
    Console.WriteLine($"Category:    {disc.Category}");
    Console.WriteLine($"Serial:      {disc.Serial}");
    Console.WriteLine($"Region:      {disc.Region}");
    Console.WriteLine($"Edition:     {disc.Edition}");
    if (!string.IsNullOrWhiteSpace(disc.Version))
    {
        Console.WriteLine($"Version:     {disc.Version}");
    }
    Console.WriteLine("\n" + new string('=', 70));
    Console.WriteLine("LANGUAGES".PadRight(70));
    Console.WriteLine(new string('=', 70));
    if (disc.Languages.Count > 0)
    {
        foreach (var lang in disc.Languages)
        {
            Console.WriteLine($"  • {lang}");
        }
    }

    Console.WriteLine("\n" + new string('=', 70));
    Console.WriteLine("TECHNICAL DETAILS".PadRight(70));
    Console.WriteLine(new string('=', 70));
    Console.WriteLine($"EXE Date:        {disc.ExeDate}");    if (!string.IsNullOrWhiteSpace(disc.Version))
    {
        Console.WriteLine($"Version:        {disc.Version}");
    }    Console.WriteLine($"EDC:             {disc.Edc}");
    Console.WriteLine($"Anti-modchip:    {disc.AntiModchip}");
    Console.WriteLine($"LibCrypt:        {disc.LibCrypt}");
    Console.WriteLine($"Errors Count:    {disc.ErrorsCount}");
    Console.WriteLine($"Write Offset:    {disc.WriteOffset}");
    Console.WriteLine($"Number of Tracks: {disc.NumberOfTracks}");

    Console.WriteLine("\n" + new string('=', 70));
    Console.WriteLine("DATABASE INFORMATION".PadRight(70));
    Console.WriteLine(new string('=', 70));
    Console.WriteLine($"Added:          {disc.AddedDate}");
    Console.WriteLine($"Last Modified:  {disc.LastModifiedDate}");

    Console.WriteLine("\n" + new string('=', 70));
    Console.WriteLine("BARCODE".PadRight(70));
    Console.WriteLine(new string('=', 70));
    Console.WriteLine($"{disc.Barcode}");

    if (!string.IsNullOrEmpty(disc.Comments))
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("COMMENTS".PadRight(70));
        Console.WriteLine(new string('=', 70));
        Console.WriteLine(disc.Comments);
    }

    if (disc.Tracks.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"TRACKS ({disc.Tracks.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (!string.IsNullOrWhiteSpace(disc.TrackStatus))
        {
            Console.WriteLine($"Tracks Status:  {disc.TrackStatus}");
        }
        if (!string.IsNullOrWhiteSpace(disc.CuesheetStatus))
        {
            Console.WriteLine($"Cuesheet Status: {disc.CuesheetStatus}");
        }
        if (!string.IsNullOrWhiteSpace(disc.TrackStatus) || !string.IsNullOrWhiteSpace(disc.CuesheetStatus))
        {
            Console.WriteLine();
        }
        foreach (var track in disc.Tracks)
        {
            Console.WriteLine($"\nTrack {track.Number}:");
            Console.WriteLine($"  Type:    {track.Type}");
            Console.WriteLine($"  Size:    {track.Size}");
            Console.WriteLine($"  CRC32:   {track.Crc32}");
            Console.WriteLine($"  MD5:     {track.Md5}");
            Console.WriteLine($"  SHA1:    {track.Sha1}");
        }
    }

    if (disc.Rings.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"RINGS ({disc.Rings.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        foreach (var ring in disc.Rings)
        {
            Console.WriteLine($"\nRing {ring.Number}:");
            Console.WriteLine($"  Mastering Code:       {(string.IsNullOrEmpty(ring.MasteringCode) ? "N/A" : ring.MasteringCode)}");
            Console.WriteLine($"  Mastering SID Code:   {(string.IsNullOrEmpty(ring.MasteringSidCode) ? "N/A" : ring.MasteringSidCode)}");
            Console.WriteLine($"  Toolstamp:            {(string.IsNullOrEmpty(ring.Toolstamp) ? "N/A" : ring.Toolstamp)}");
            Console.WriteLine($"  Mould SID Code:       {(string.IsNullOrEmpty(ring.MouldSidCode) ? "N/A" : ring.MouldSidCode)}");
            if (!string.IsNullOrWhiteSpace(ring.Status))
            {
                Console.WriteLine($"  Status:               {ring.Status}");
            }
        }
    }

    if (disc.PvdEntries.Count > 0)
    {
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine($"PRIMARY VOLUME DESCRIPTOR (PVD) ({disc.PvdEntries.Count})".PadRight(70));
        Console.WriteLine(new string('=', 70));
        if (!string.IsNullOrWhiteSpace(disc.PvdStatus))
        {
            Console.WriteLine($"Status: {disc.PvdStatus}\n");
        }
        foreach (var pvd in disc.PvdEntries)
        {
            Console.WriteLine($"\n{pvd.Entry}:");
            Console.WriteLine($"  Contents: {pvd.Contents}");
            Console.WriteLine($"  Date:     {pvd.Date}");
            Console.WriteLine($"  Time:     {pvd.Time}");
            Console.WriteLine($"  GMT:      {pvd.Gmt}");
        }
    }

    Console.WriteLine("\n" + new string('=', 70));
}
catch (Exception ex)
{
    Console.WriteLine($"Error during scraping: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}