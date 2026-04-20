# RedumpScraper

A .NET tool for scraping and parsing disc information from [redump.org](http://redump.org/), extracting comprehensive data about video game discs including metadata, tracks, rings, PVD records, and LibCrypt protection information.

## Features

- **Disc Information**: Extract basic metadata (title, system, media, region, serial, etc.)
- **Languages**: Parse supported languages for each disc
- **Technical Details**: Retrieve EDC, anti-modchip, LibCrypt status, error counts, and write offsets
- **Tracks**: Extract track data including type, size, and checksums (CRC32, MD5, SHA1)
- **Disc Rings**: Parse ring information including mastering codes, SID codes, and toolstamps
- **PVD Records**: Extract Primary Volume Descriptor entries with timestamps
- **LibCrypt Protection**: Parse LibCrypt protection sectors with detailed hex contents and XOR values
- **Database Info**: Track when discs were added and last modified

## ⚠️ Responsible Usage & Legal Disclaimer

**This tool is provided for educational and personal archival purposes only.**

### Important Considerations

**Respect Redump's Infrastructure:**
- Redump is a volunteer-driven project maintained by a small team
- Do NOT use this tool for bulk/mass scraping of the entire Redump database
- Implement appropriate delays between requests to avoid overloading their servers
- Use this tool responsibly and sparingly

**Best Practices:**
- ✅ Scrape specific discs you personally own and are preserving
- ✅ Cache results locally to minimize repeated requests to Redump
- ✅ Use the MongoDB local storage feature to build your personal archive
- ✅ Add reasonable delays between requests (use `Thread.Sleep()` if scripting multiple discs)
- ❌ Do NOT perform automated bulk downloads of entire disc database
- ❌ Do NOT republish Redump data without permission
- ❌ Do NOT use this tool to create competing databases
- ❌ Do NOT overload Redump's servers with rapid-fire requests

### Legal Disclaimer

- **No Warranty**: This tool is provided as-is without any warranty
- **User Responsibility**: Users are solely responsible for ensuring their usage complies with:
  - Redump's [Terms of Service](http://redump.org/)
  - Applicable laws in their jurisdiction
  - Respectful use of volunteer-maintained infrastructure
- **Copyright**: Redump data is copyrighted by the Redump team. This tool is for personal/archival use only
- **Not Endorsed**: This tool is not affiliated with, endorsed by, or associated with Redump
- **Liability**: The author is not liable for any consequences resulting from misuse of this tool

### Ethical Usage

By using this tool, you agree to:
1. Use it only for personal archival of discs you own
2. Not perform bulk/automated scraping of the Redump database
3. Respect Redump as a volunteer community resource
4. Cache results locally rather than making repeated requests
5. Comply with all applicable laws and Redump's terms of service

**If you build a large local collection:** Host the data locally and don't make public mirrors. If you want to contribute your data back to Redump, please contact the Redump team directly.

---

## Project Structure

```
RedumpScraper/
├── RedumpApp/              # Console application for disc lookup
│   ├── Program.cs          # Main entry point with formatted output
│   └── RedumpApp.csproj
├── RedumpLib/              # Core scraping library
│   ├── Scraper.cs          # HTML parsing logic
│   ├── RedumpDisc.cs       # Main disc data model
│   ├── DiscTrack.cs        # Track information
│   ├── DiscRing.cs         # Ring information
│   ├── PvdRecord.cs        # PVD record model
│   ├── LibCryptSector.cs   # LibCrypt sector data
│   └── RedumpLib.csproj
├── RedumpDatabase/         # MongoDB database library
│   ├── Models/             # MongoDB document models
│   │   └── DiscDocument.cs # Disc, Track, Ring, PVD, LibCrypt documents
│   ├── Services/
│   │   └── RedumpMongoDbService.cs # Database operations
│   ├── Mappers/
│   │   └── DiscMapper.cs   # RedumpDisc ↔ DiscDocument conversion
│   └── RedumpDatabase.csproj
├── RedumpDbLoader/         # Database population CLI tool
│   ├── Program.cs          # Add discs, search, list by system/region
│   └── RedumpDbLoader.csproj
└── RedumpLib.Tests/        # Unit tests (69 tests total)
    ├── ID17031Fixture.cs           # Fixture for Sheep, Dog 'n' Wolf disc
    ├── ID17031ScraperTests.cs      # 36 tests for basic disc parsing
    ├── ID27824Fixture.cs           # Fixture for Disney Tarzan disc
    ├── ID27824LibCryptTests.cs     # 33 tests for LibCrypt validation
    ├── TestData/                   # Sample HTML files for testing
    │   ├── ID_17031.html           # Sheep, Dog 'n' Wolf (no LibCrypt)
    │   └── ID_27824.html           # Disney Tarzan (32 LibCrypt sectors)
    └── RedumpLib.Tests.csproj
```

## Requirements

- **.NET 10.0** or later
- **HtmlAgilityPack** (for HTML parsing)
- **MongoDB.Driver** (for database operations)
- **MongoDB 4.0+** (for local database - optional, only for RedumpDbLoader)

## Installation

Clone the repository and build the solution:

```bash
dotnet build RedumpScraper.slnx
```

## Usage

### Console Application

Run the application with a Redump disc ID, serial number, or hash:

```bash
# Look up by disc ID
dotnet run --project RedumpApp -- <disc-id>

# Search by serial number
dotnet run --project RedumpApp -- --serial <serial-number>

# Search by hash (CRC32, MD5, SHA1)
dotnet run --project RedumpApp -- --hash <hash-value>
```

Examples:

```bash
# By disc ID
dotnet run --project RedumpApp -- 27824

# By serial number
dotnet run --project RedumpApp -- --serial SCES-01518

# By hash
dotnet run --project RedumpApp -- --hash f45c579064568e96f6a01f16fc7b726f
```

This will output comprehensive information about the disc formatted in sections:

- **BASIC INFORMATION**: Title, system, media, category, serial, region, edition
- **LANGUAGES**: Supported languages
- **TECHNICAL DETAILS**: EXE date, EDC, anti-modchip, LibCrypt, error count, write offset
- **DATABASE INFORMATION**: Added and last modified dates
- **BARCODE**: Disc barcode(s)
- **COMMENTS**: Additional comments (if any)
- **TRACKS**: Track information with checksums
- **RINGS**: Ring data including mastering codes
- **PRIMARY VOLUME DESCRIPTOR (PVD)**: Timestamp information
- **LIBCRYPT PROTECTION**: Protected sectors (if any)

### Library Usage

Use `RedumpLib` in your own projects:

```csharp
using RedumpLib;
using System.Threading.Tasks;

var scraper = new Scraper();

// Parse from URL
var disc = scraper.ParseRedumpPage("http://redump.org/disc/27824/");

// Or parse from HTML string
string htmlContent = File.ReadAllText("disc.html");
var disc = scraper.ParseRedumpHtml(htmlContent);

// Search by serial number, hash, or any search term
var searchDisc = await scraper.SearchRedumpByQuickSearchAsync("SCES-01518");
var hashDisc = await scraper.SearchRedumpByQuickSearchAsync("f45c579064568e96f6a01f16fc7b726f");

// Access disc data
Console.WriteLine($"Title: {disc.Title}");
Console.WriteLine($"System: {disc.System}");
foreach (var track in disc.Tracks)
{
    Console.WriteLine($"Track {track.Number}: {track.Type}");
}
foreach (var sector in disc.LibCryptSectors)
{
    Console.WriteLine($"Sector {sector.Sector}: {sector.Msf}");
}
```

### MongoDB Database Usage

Store and query disc data in a local MongoDB database:

```bash
# Make sure MongoDB is running locally (mongodb://localhost:27017)

# Add a disc to the database
dotnet run --project RedumpDbLoader -- add 27824

# Search discs by title
dotnet run --project RedumpDbLoader -- search "Disney"

# Search discs by serial number
dotnet run --project RedumpDbLoader -- serial "SCES-01518"

# Search discs by CRC32 hash
dotnet run --project RedumpDbLoader -- crc32 "985c5153"

# Search discs by MD5 hash
dotnet run --project RedumpDbLoader -- md5 "d4f95f37c5e91494fbeaf83b2fad23c3"

# Search discs by SHA1 hash
dotnet run --project RedumpDbLoader -- sha1 "2e5d4c8f9e7c8b6a5d4c8b6a5d4c8b6a"

# List all discs for a specific system
dotnet run --project RedumpDbLoader -- system "Sony PlayStation"

# List all discs for a specific region
dotnet run --project RedumpDbLoader -- region "Europe"

# List all discs with LibCrypt protection
dotnet run --project RedumpDbLoader -- libcrypt

# Advanced filter with multiple criteria
dotnet run --project RedumpDbLoader -- filter title "Disney" system "Sony PlayStation"

# Filter by serial
dotnet run --project RedumpDbLoader -- filter serial "SCES"

# Filter by serial and system
dotnet run --project RedumpDbLoader -- filter serial "SCES-01518" system "Sony PlayStation"

# Filter by CRC32 hash
dotnet run --project RedumpDbLoader -- filter crc32 "985c5153"

# Filter by MD5 hash
dotnet run --project RedumpDbLoader -- filter md5 "d4f95f37c5e91494fbeaf83b2fad23c3"

# Filter by SHA1 hash
dotnet run --project RedumpDbLoader -- filter sha1 "2e5d4c8f9e7c8b6a5d4c8b6a5d4c8b6a"

# Filter by hash and system
dotnet run --project RedumpDbLoader -- filter crc32 "985c" system "Sony PlayStation"

# Filter by region and LibCrypt
dotnet run --project RedumpDbLoader -- filter region "Italy" libcrypt

# Filter by title and CRC32
dotnet run --project RedumpDbLoader -- filter title "Crash" crc32 "985c"

# Any combination of filters
dotnet run --project RedumpDbLoader -- filter title "Crash" system "Sony PlayStation" region "USA"

# Show database statistics
dotnet run --project RedumpDbLoader -- stats
```

Use `RedumpDatabase` library in your projects:

```csharp
using RedumpDatabase.Services;
using RedumpDatabase.Mappers;

var dbService = new RedumpMongoDbService("mongodb://localhost:27017", "redump");

// Get a disc by ID
var disc = await dbService.GetDiscByIdAsync("27824");

// Search discs by title
var results = await dbService.SearchDiscsByTitleAsync("Tarzan");

// Search discs by serial number
var serialResults = await dbService.SearchBySerialAsync("SCES-01518");

// Search discs by CRC32 hash
var crc32Results = await dbService.SearchByCrc32Async("985c5153");

// Search discs by MD5 hash
var md5Results = await dbService.SearchByMd5Async("d4f95f37c5e91494fbeaf83b2fad23c3");

// Search discs by SHA1 hash
var sha1Results = await dbService.SearchBySha1Async("2e5d4c8f9e7c8b6a5d4c8b6a5d4c8b6a");

// Get discs by system
var psDiscs = await dbService.GetDiscsBySystemAsync("Sony PlayStation");

// Get discs with LibCrypt
var libcryptDiscs = await dbService.GetDiscsWithLibCryptAsync();

// Advanced filtering with multiple criteria
var filteredDiscs = await dbService.GetDiscsByMultipleFiltersAsync(
    title: "Disney",
    serial: null,
    crc32: null,
    md5: null,
    sha1: null,
    system: "Sony PlayStation",
    region: null,
    hasLibCrypt: null
);

// Filter by serial and system
var serialSystemDiscs = await dbService.GetDiscsByMultipleFiltersAsync(
    serial: "SCES",
    system: "Sony PlayStation"
);

// Filter by hash and system
var hashDiscs = await dbService.GetDiscsByMultipleFiltersAsync(
    crc32: "985c5153",
    system: "Sony PlayStation"
);

// Filter by system and LibCrypt
var protectedDiscs = await dbService.GetDiscsByMultipleFiltersAsync(
    system: "Sony PlayStation",
    hasLibCrypt: true
);

// Add/update a disc
var scraper = new Scraper();
var redumpDisc = scraper.ParseRedumpPage("http://redump.org/disc/27824/");
var document = DiscMapper.ToDocument(redumpDisc);
await dbService.UpsertDiscAsync(document);
```

## Data Models

### RedumpDisc

Main class containing all disc information:

```csharp
public class RedumpDisc
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string System { get; set; }
    public string Media { get; set; }
    public string Category { get; set; }
    public string Region { get; set; }
    public List<string> Languages { get; set; }
    public string Serial { get; set; }
    public string ExeDate { get; set; }
    public string Edition { get; set; }
    public string Edc { get; set; }
    public string AntiModchip { get; set; }
    public string LibCrypt { get; set; }
    public string ErrorsCount { get; set; }
    public string NumberOfTracks { get; set; }
    public string WriteOffset { get; set; }
    public string AddedDate { get; set; }
    public string LastModifiedDate { get; set; }
    public string Barcode { get; set; }
    public string Comments { get; set; }
    public List<DiscTrack> Tracks { get; set; }
    public List<DiscRing> Rings { get; set; }
    public List<PvdRecord> PvdEntries { get; set; }
    public List<LibCryptSector> LibCryptSectors { get; set; }
}
```

### DiscTrack

```csharp
public record DiscTrack(
    string Number,
    string Type,
    string Size,
    string Crc32,
    string Md5,
    string Sha1
);
```

### DiscRing

```csharp
public record DiscRing(
    string Number,
    string MasteringCode,
    string MasteringSidCode,
    string Toolstamp,
    string MouldSidCode,
    string Status
);
```

### PvdRecord

```csharp
public record PvdRecord(
    string Entry,
    string Contents,
    string Date,
    string Time,
    string Gmt
);
```

### LibCryptSector

```csharp
public record LibCryptSector(
    string Sector,
    string Msf,
    string Contents,
    string Xor,
    string Comments
);
```

### MongoDB Document Models

MongoDB documents are automatically indexed for optimal query performance:

**Indexes:**
- `disc_id` (Unique) - Fast disc lookup by ID
- `system` - Filter discs by system
- `region` - Filter discs by region
- `title` (Text) - Full-text search support

**DiscDocument** - Main disc storage with all metadata, tracks, rings, PVD entries, and LibCrypt sectors embedded.

All nested objects (tracks, rings, PVD entries, LibCrypt sectors) are stored as arrays within the main document for efficient querying and atomic operations.

**Fields include:**
- Timestamps: `created_at`, `updated_at` (automatic)
- All RedumpDisc properties normalized to snake_case (e.g., `disc_id`, `anti_modchip`)
- Nested arrays for tracks, rings, PVD entries, and LibCrypt sectors

## Testing

The project includes a comprehensive test suite with 69 tests organized by disc ID using fixtures for test data management.

### Test Structure

Tests are organized using ID-based naming conventions for better organization and reusability:

**ID 17031 - Sheep, Dog 'n' Wolf (No LibCrypt)**
- **Fixture**: `ID17031Fixture.cs` - Loads and parses `TestData/ID_17031.html`
- **Test Class**: `ID17031ScraperTests.cs` - 36 tests covering:
  - Disc metadata (title, system, region, serial, etc.)
  - Languages (6 languages supported)
  - Technical details (EDC, anti-modchip, write offset)
  - Tracks and ring information
  - PVD records with timestamps
  - Empty LibCrypt validation (disc has no protection)

**ID 27824 - Disney Tarzan (32 LibCrypt Sectors)**
- **Fixture**: `ID27824Fixture.cs` - Loads and parses `TestData/ID_27824.html`
- **Test Class**: `ID27824LibCryptTests.cs` - 33 tests covering comprehensive LibCrypt validation:
  - Sector collection integrity (verify exactly 32 sectors)
  - MSF timestamp format validation (MM:SS:FF format)
  - Sector data completeness (all required fields populated)
  - Specific sector validation (first, last, middle sectors)
  - XOR value variation between sectors
  - Hex contents and comment validation
  - Sector numbering gaps and ordering

### Running Tests

```bash
# Run all tests
dotnet test RedumpScraper.slnx

# Run with verbose output
dotnet test RedumpScraper.slnx -v normal

# Run specific test class
dotnet test --filter "ClassName=RedumpLib.Tests.ID17031ScraperTests"
```

**Test Results**: All 69 tests pass ✅

## Example Output

```
======================================================================
BASIC INFORMATION
======================================================================
ID:          27824
Title:       Disney Tarzan
System:      Sony PlayStation
Media:       CD
Category:    Games
Serial:      SCES-01518
Region:      Italy
Edition:     Original, Platinum

======================================================================
TECHNICAL DETAILS
======================================================================
EXE Date:        1999-10-04
EDC:             Yes
Anti-modchip:    Yes
LibCrypt:        Yes
Errors Count:    0
Write Offset:    -647
Number of Tracks: 2

======================================================================
LIBCRYPT PROTECTION (32)
======================================================================

Sector 14105:
  MSF:      03:08:05
  Contents: 41 01 01 07 06 05 00 23 08 05 38 39
  XOR:      8001 c701
  Comments: LC1 sector, no errors in data &CRC-16
...
```

## License

This project is for educational and archival purposes, respecting the work of the Redump Team.

## References

- [Redump.org](http://redump.org/) - Disc image information database
- [HtmlAgilityPack](https://html-agility-pack.net/) - HTML parsing library
