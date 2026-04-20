using System;
using RedumpLib;
using RedumpDatabase.Services;
using RedumpDatabase.Mappers;

Console.WriteLine("========================================");
Console.WriteLine("Redump MongoDB Database Loader");
Console.WriteLine("========================================");
Console.WriteLine("\n⚠️  REMINDER: Use this tool responsibly!");
Console.WriteLine("   - Scrape only discs you personally own");
Console.WriteLine("   - Add delays between requests to respect Redump's servers");
Console.WriteLine("   - See README.md for responsible usage guidelines\n");

if (args.Length == 0)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  Add disc:              dotnet run -- add <disc-id>");
    Console.WriteLine("  Search by title:       dotnet run -- search <query>");
    Console.WriteLine("  Search by serial:      dotnet run -- serial <serial-number>");
    Console.WriteLine("  Search by CRC32:       dotnet run -- crc32 <hash>");
    Console.WriteLine("  Search by MD5:         dotnet run -- md5 <hash>");
    Console.WriteLine("  Search by SHA1:        dotnet run -- sha1 <hash>");
    Console.WriteLine("  List by system:        dotnet run -- system <system-name>");
    Console.WriteLine("  List by region:        dotnet run -- region <region-name>");
    Console.WriteLine("  List LibCrypt:         dotnet run -- libcrypt");
    Console.WriteLine("  Advanced filter:       dotnet run -- filter [title <query>] [serial <num>] [crc32 <hash>]");
    Console.WriteLine("                                              [md5 <hash>] [sha1 <hash>] [system <name>]");
    Console.WriteLine("                                              [region <name>] [libcrypt]");
    Console.WriteLine("  Stats:                 dotnet run -- stats");
    Console.WriteLine("\nExamples:");
    Console.WriteLine("  dotnet run -- add 27824");
    Console.WriteLine("  dotnet run -- search \"Disney\"");
    Console.WriteLine("  dotnet run -- serial \"SCES-01518\"");
    Console.WriteLine("  dotnet run -- crc32 \"985c5153\"");
    Console.WriteLine("  dotnet run -- system \"Sony PlayStation\"");
    Console.WriteLine("  dotnet run -- filter title \"Disney\" system \"Sony PlayStation\"");
    Console.WriteLine("  dotnet run -- filter serial \"SCES\" crc32 \"985c\"");
    return;
}

string mongoUrl = "mongodb://localhost:27017";
var dbService = new RedumpMongoDbService(mongoUrl, "redump");
var scraper = new Scraper();

try
{
    string command = args[0].ToLower();

    switch (command)
    {
        case "add":
            await AddDisc(args.Length > 1 ? args[1] : "");
            break;

        case "search":
            await SearchDiscs(string.Join(" ", args.Skip(1)));
            break;

        case "serial":
            await SearchBySerial(string.Join(" ", args.Skip(1)));
            break;

        case "crc32":
            await SearchByCrc32(args.Length > 1 ? args[1] : "");
            break;

        case "md5":
            await SearchByMd5(args.Length > 1 ? args[1] : "");
            break;

        case "sha1":
            await SearchBySha1(args.Length > 1 ? args[1] : "");
            break;

        case "system":
            await ListBySystem(args.Length > 1 ? args[1] : "");
            break;

        case "region":
            await ListByRegion(args.Length > 1 ? args[1] : "");
            break;

        case "libcrypt":
            await ListLibCryptDiscs();
            break;

        case "filter":
            await AdvancedFilter(args.Skip(1).ToArray());
            break;

        case "stats":
            await ShowStats();
            break;

        default:
            Console.WriteLine($"Unknown command: {command}");
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Error: {ex.InnerException.Message}");
    }
}

async Task AdvancedFilter(string[] filterArgs)
{
    string? title = null;
    string? serial = null;
    string? crc32 = null;
    string? md5 = null;
    string? sha1 = null;
    string? system = null;
    string? region = null;
    bool? libcrypt = null;

    // Parse filter arguments
    for (int i = 0; i < filterArgs.Length; i++)
    {
        switch (filterArgs[i].ToLower())
        {
            case "title":
                if (i + 1 < filterArgs.Length)
                    title = filterArgs[++i];
                break;

            case "serial":
                if (i + 1 < filterArgs.Length)
                    serial = filterArgs[++i];
                break;

            case "crc32":
                if (i + 1 < filterArgs.Length)
                    crc32 = filterArgs[++i];
                break;

            case "md5":
                if (i + 1 < filterArgs.Length)
                    md5 = filterArgs[++i];
                break;

            case "sha1":
                if (i + 1 < filterArgs.Length)
                    sha1 = filterArgs[++i];
                break;

            case "system":
                if (i + 1 < filterArgs.Length)
                    system = filterArgs[++i];
                break;

            case "region":
                if (i + 1 < filterArgs.Length)
                    region = filterArgs[++i];
                break;

            case "libcrypt":
                libcrypt = true;
                break;
        }
    }

    var filters = new List<string>();
    if (!string.IsNullOrEmpty(title)) filters.Add($"title contains \"{title}\"");
    if (!string.IsNullOrEmpty(serial)) filters.Add($"serial = \"{serial}\"");
    if (!string.IsNullOrEmpty(crc32)) filters.Add($"crc32 = \"{crc32}\"");
    if (!string.IsNullOrEmpty(md5)) filters.Add($"md5 = \"{md5}\"");
    if (!string.IsNullOrEmpty(sha1)) filters.Add($"sha1 = \"{sha1}\"");
    if (!string.IsNullOrEmpty(system)) filters.Add($"system = \"{system}\"");
    if (!string.IsNullOrEmpty(region)) filters.Add($"region = \"{region}\"");
    if (libcrypt.HasValue && libcrypt.Value) filters.Add($"has LibCrypt protection");

    if (filters.Count == 0)
    {
        Console.WriteLine("Please specify at least one filter\n");
        return;
    }

    Console.WriteLine($"Filters: {string.Join(", ", filters)}\n");

    var results = await dbService.GetDiscsByMultipleFiltersAsync(title, serial, crc32, md5, sha1, system, region, libcrypt);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found matching the filters");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s):\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        if (!string.IsNullOrEmpty(disc.System)) Console.WriteLine($"    System: {disc.System}");
        if (!string.IsNullOrEmpty(disc.Region)) Console.WriteLine($"    Region: {disc.Region}");
        if (!string.IsNullOrEmpty(disc.Serial)) Console.WriteLine($"    Serial: {disc.Serial}");
        if (disc.LibCryptSectors.Count > 0) Console.WriteLine($"    LibCrypt Sectors: {disc.LibCryptSectors.Count}");
        Console.WriteLine();
    }
}

async Task AddDisc(string discId)
{
    if (string.IsNullOrWhiteSpace(discId))
    {
        Console.WriteLine("Error: Disc ID is required");
        return;
    }

    Console.WriteLine($"Fetching disc {discId}...");
    string url = $"http://redump.org/disc/{discId}/";

    try
    {
        RedumpDisc disc = scraper.ParseRedumpPage(url);
        
        if (string.IsNullOrEmpty(disc.Title))
        {
            Console.WriteLine("Error: Disc not found or could not be parsed");
            return;
        }

        var document = DiscMapper.ToDocument(disc);
        await dbService.UpsertDiscAsync(document);

        Console.WriteLine($"✓ Successfully added/updated disc {discId}: {disc.Title}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Failed to add disc: {ex.Message}");
        if (ex.StackTrace != null)
        {
            Console.WriteLine($"Stack trace:\n{ex.StackTrace}");
        }
    }
}

async Task SearchDiscs(string query)
{
    if (string.IsNullOrWhiteSpace(query))
    {
        Console.WriteLine("Error: Search query is required");
        return;
    }

    Console.WriteLine($"Searching for: {query}\n");
    var results = await dbService.SearchDiscsByTitleAsync(query);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s):\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        Console.WriteLine($"    System: {disc.System}");
        Console.WriteLine($"    Region: {disc.Region}");
        Console.WriteLine();
    }
}

async Task SearchBySerial(string serial)
{
    if (string.IsNullOrWhiteSpace(serial))
    {
        Console.WriteLine("Error: Serial number is required");
        return;
    }

    Console.WriteLine($"Searching for serial: {serial}\n");
    var results = await dbService.SearchBySerialAsync(serial);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s):\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        Console.WriteLine($"    Serial: {disc.Serial}");
        Console.WriteLine($"    System: {disc.System}");
        Console.WriteLine($"    Region: {disc.Region}");
        Console.WriteLine();
    }
}

async Task SearchByCrc32(string crc32)
{
    if (string.IsNullOrWhiteSpace(crc32))
    {
        Console.WriteLine("Error: CRC32 hash is required");
        return;
    }

    Console.WriteLine($"Searching for CRC32: {crc32}\n");
    var results = await dbService.SearchByCrc32Async(crc32);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s) with matching CRC32:\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        var matchingTracks = disc.Tracks.Where(t => t.Crc32.Contains(crc32, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var track in matchingTracks)
        {
            Console.WriteLine($"    Track {track.Number}: {track.Crc32}");
        }
        Console.WriteLine();
    }
}

async Task SearchByMd5(string md5)
{
    if (string.IsNullOrWhiteSpace(md5))
    {
        Console.WriteLine("Error: MD5 hash is required");
        return;
    }

    Console.WriteLine($"Searching for MD5: {md5}\n");
    var results = await dbService.SearchByMd5Async(md5);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s) with matching MD5:\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        var matchingTracks = disc.Tracks.Where(t => t.Md5.Contains(md5, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var track in matchingTracks)
        {
            Console.WriteLine($"    Track {track.Number}: {track.Md5}");
        }
        Console.WriteLine();
    }
}

async Task SearchBySha1(string sha1)
{
    if (string.IsNullOrWhiteSpace(sha1))
    {
        Console.WriteLine("Error: SHA1 hash is required");
        return;
    }

    Console.WriteLine($"Searching for SHA1: {sha1}\n");
    var results = await dbService.SearchBySha1Async(sha1);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s) with matching SHA1:\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        var matchingTracks = disc.Tracks.Where(t => t.Sha1.Contains(sha1, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var track in matchingTracks)
        {
            Console.WriteLine($"    Track {track.Number}: {track.Sha1}");
        }
        Console.WriteLine();
    }
}

async Task ListBySystem(string system)
{
    if (string.IsNullOrWhiteSpace(system))
    {
        Console.WriteLine("Error: System name is required");
        return;
    }

    Console.WriteLine($"Discs for system: {system}\n");
    var results = await dbService.GetDiscsBySystemAsync(system);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s):\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        Console.WriteLine($"    Region: {disc.Region}");
        Console.WriteLine($"    Serial: {disc.Serial}");
        Console.WriteLine();
    }
}

async Task ListByRegion(string region)
{
    if (string.IsNullOrWhiteSpace(region))
    {
        Console.WriteLine("Error: Region name is required");
        return;
    }

    Console.WriteLine($"Discs for region: {region}\n");
    var results = await dbService.GetDiscsByRegionAsync(region);

    if (results.Count == 0)
    {
        Console.WriteLine("No discs found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s):\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        Console.WriteLine($"    System: {disc.System}");
        Console.WriteLine();
    }
}

async Task ListLibCryptDiscs()
{
    Console.WriteLine("Discs with LibCrypt protection:\n");
    var results = await dbService.GetDiscsWithLibCryptAsync();

    if (results.Count == 0)
    {
        Console.WriteLine("No discs with LibCrypt found");
        return;
    }

    Console.WriteLine($"Found {results.Count} disc(s) with LibCrypt:\n");
    foreach (var disc in results)
    {
        Console.WriteLine($"  [{disc.DiscId}] {disc.Title}");
        Console.WriteLine($"    System: {disc.System}");
        Console.WriteLine($"    LibCrypt Sectors: {disc.LibCryptSectors.Count}");
        Console.WriteLine();
    }
}

async Task ShowStats()
{
    var count = await dbService.GetDiscCountAsync();
    
    Console.WriteLine("\nDatabase Statistics:");
    Console.WriteLine($"  Total Discs: {count}");
    
    if (count > 0)
    {
        var libcryptDiscs = await dbService.GetDiscsWithLibCryptAsync();
        Console.WriteLine($"  Discs with LibCrypt: {libcryptDiscs.Count}");
    }
}
