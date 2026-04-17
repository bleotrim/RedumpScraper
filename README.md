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

## Project Structure

```
RedumpScraper/
├── RedumpApp/              # Console application
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

## Installation

Clone the repository and build the solution:

```bash
dotnet build RedumpScraper.slnx
```

## Usage

### Console Application

Run the application with a Redump disc ID:

```bash
dotnet run --project RedumpApp -- <disc-id>
```

Example:

```bash
dotnet run --project RedumpApp -- 27824
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

var scraper = new Scraper();

// Parse from URL
var disc = scraper.ParseRedumpPage("http://redump.org/disc/27824/");

// Or parse from HTML string
string htmlContent = File.ReadAllText("disc.html");
var disc = scraper.ParseRedumpHtml(htmlContent);

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
