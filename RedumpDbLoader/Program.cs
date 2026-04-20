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
    Console.WriteLine("  List by system:        dotnet run -- system <system-name>");
    Console.WriteLine("  List by region:        dotnet run -- region <region-name>");
    Console.WriteLine("  List LibCrypt:         dotnet run -- libcrypt");
    Console.WriteLine("  Advanced filter:       dotnet run -- filter [title <query>] [system <name>] [region <name>] [libcrypt]");
    Console.WriteLine("  Stats:                 dotnet run -- stats");
    Console.WriteLine("\nExamples:");
    Console.WriteLine("  dotnet run -- add 27824");
    Console.WriteLine("  dotnet run -- search \"Disney\"");
    Console.WriteLine("  dotnet run -- system \"Sony PlayStation\"");
    Console.WriteLine("  dotnet run -- filter title \"Disney\" system \"Sony PlayStation\"");
    Console.WriteLine("  dotnet run -- filter region \"Europe\" libcrypt");
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
    if (!string.IsNullOrEmpty(system)) filters.Add($"system = \"{system}\"");
    if (!string.IsNullOrEmpty(region)) filters.Add($"region = \"{region}\"");
    if (libcrypt.HasValue && libcrypt.Value) filters.Add($"has LibCrypt protection");

    if (filters.Count == 0)
    {
        Console.WriteLine("Please specify at least one filter\n");
        return;
    }

    Console.WriteLine($"Filters: {string.Join(", ", filters)}\n");

    var results = await dbService.GetDiscsByMultipleFiltersAsync(title, system, region, libcrypt);

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
