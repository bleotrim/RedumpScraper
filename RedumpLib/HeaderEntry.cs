namespace RedumpLib;

/// <summary>
/// Represents a header entry containing hex data and ASCII representation
/// </summary>
public record HeaderEntry(
    string Row,      // Row offset (e.g., "0000", "0010")
    string Contents, // Hex contents (e.g., "53 45 47 41 20 53 45 47 41 4B 41 54 41 4E 41 20")
    string Ascii     // ASCII representation (e.g., "SEGA SEGAKATANA ")
);
