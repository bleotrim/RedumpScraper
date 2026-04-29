using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RedumpLib;

/// <summary>
/// Scraper for parsing disc information from redump.org
/// 
/// IMPORTANT: Use this tool responsibly!
/// - Scrape only discs you personally own and are archiving
/// - Implement delays between requests to avoid overloading Redump's servers
/// - Cache results locally to minimize repeated requests
/// - Do NOT perform bulk/automated scraping of the entire database
/// - See README.md for complete responsible usage guidelines
/// </summary>
public class Scraper
{
    private readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    public RedumpDisc ParseRedumpPage(string url)
    {
        var web = new HtmlWeb { UserAgent = _userAgent };
        var doc = web.Load(url);
        var disc = ParseDocument(doc);

        var idMatch = Regex.Match(url, @"/disc/(\d+)/");
        if (idMatch.Success)
        {
            disc.Id = idMatch.Groups[1].Value;
        }

        // Capture HTML source and clean it
        disc.HtmlSource = doc.DocumentNode.OuterHtml;

        return disc;
    }

    public RedumpDisc ParseRedumpHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return ParseDocument(doc);
    }

    /// <summary>
    /// Search for discs using Redump's quicksearch feature (by serial, hash, etc.)
    /// Returns search results which may contain multiple discs
    /// </summary>
    /// <param name="query">Search query (serial number, CRC32, MD5, SHA1, etc.)</param>
    /// <returns>SearchResultsContainer with list of matching discs</returns>
    public async Task<SearchResultsContainer> SearchRedumpQuickSearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Search query cannot be empty", nameof(query));

        using (var handler = new HttpClientHandler { AllowAutoRedirect = false })
        using (var client = new HttpClient(handler))
        {
            client.DefaultRequestHeaders.Add("User-Agent", _userAgent);

            // Step 1: POST to /results/ with quicksearch parameter
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("quicksearch", query)
            });

            var response = await client.PostAsync("http://redump.org/results/", content);

            // Step 2: Follow redirect to /discs/quicksearch/{query}/
            if (response.StatusCode != System.Net.HttpStatusCode.Found &&
                response.StatusCode != System.Net.HttpStatusCode.Moved)
                throw new Exception($"Unexpected response from search: {response.StatusCode}");

            var redirectUrl = response.Content.Headers.ContentLocation?.AbsoluteUri ??
                             response.Headers.Location?.AbsoluteUri;

            if (string.IsNullOrEmpty(redirectUrl))
                throw new Exception("No redirect location found");

            // Step 3: Get the search results page
            var resultsResponse = await client.GetAsync(redirectUrl);

            // Check if we got a redirect (single result case - redirects directly to disc page)
            if (resultsResponse.StatusCode == System.Net.HttpStatusCode.Found ||
                resultsResponse.StatusCode == System.Net.HttpStatusCode.Moved)
            {
                // Single result - follows redirect to disc page
                var discUrl = resultsResponse.Content.Headers.ContentLocation?.AbsoluteUri ??
                             resultsResponse.Headers.Location?.AbsoluteUri;

                if (string.IsNullOrEmpty(discUrl))
                    throw new Exception("No redirect to disc page found");

                // Return single result in container
                var results = new SearchResultsContainer { SearchQuery = query };

                // Parse the disc page to get the ID
                var discResponse = await client.GetAsync(discUrl);
                if (discResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var discHtml = await discResponse.Content.ReadAsStringAsync();
                    var discIdMatch = Regex.Match(discUrl, @"/disc/(\d+)/");

                    if (discIdMatch.Success)
                    {
                        results.Results.Add(new SearchResult
                        {
                            DiscId = discIdMatch.Groups[1].Value
                            // Other fields will be populated when disc is fully parsed
                        });
                    }
                }

                return results;
            }

            if (resultsResponse.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Could not retrieve search results: {resultsResponse.StatusCode}");

            var responseContent = await resultsResponse.Content.ReadAsStringAsync();

            // Step 4: Parse the results page
            var results2 = ParseSearchResultsPage(responseContent, query);

            return results2;
        }
    }

    /// <summary>
    /// Parse the search results HTML page and extract disc information
    /// </summary>
    private SearchResultsContainer ParseSearchResultsPage(string html, string query)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var container = new SearchResultsContainer { SearchQuery = query };

        // Find the results table
        var table = doc.DocumentNode.SelectSingleNode("//table[@class='games']");
        if (table == null)
            return container; // No results table found

        // Get all rows except the header (first tr with class 'th')
        var rows = table.SelectNodes(".//tr[not(@class='th')]");
        if (rows == null || rows.Count == 0)
            return container; // No data rows

        foreach (var row in rows)
        {
            var cols = row.SelectNodes("td");
            if (cols == null || cols.Count < 7)
                continue;

            try
            {
                var result = new SearchResult
                {
                    Region = cols[0].InnerText.Trim(),
                    System = cols[2].InnerText.Trim(),
                    Version = cols[3].InnerText.Trim(),
                    Edition = cols[4].InnerText.Trim(),
                    Serial = cols[6].InnerText.Trim()
                };

                // Extract title and disc ID from the link in column 1
                var titleLink = cols[1].SelectSingleNode(".//a");
                if (titleLink != null)
                {
                    result.Title = titleLink.InnerText.Trim();
                    var href = titleLink.GetAttributeValue("href", "");
                    var discIdMatch = Regex.Match(href, @"/disc/(\d+)/");
                    if (discIdMatch.Success)
                    {
                        result.DiscId = discIdMatch.Groups[1].Value;
                    }
                }

                // Extract languages from images in column 5
                var langImages = cols[5].SelectNodes(".//img");
                if (langImages != null)
                {
                    foreach (var img in langImages)
                    {
                        var title = img.GetAttributeValue("title", "");
                        if (!string.IsNullOrEmpty(title))
                        {
                            result.Languages.Add(title);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(result.DiscId))
                {
                    container.Results.Add(result);
                }
            }
            catch
            {
                // Skip rows that can't be parsed
                continue;
            }
        }

        return container;
    }

    /// <summary>
    /// Search for a disc using Redump's quicksearch feature (by serial, hash, etc.)
    /// If single result is found, automatically parses and returns the disc
    /// If multiple results are found, throws exception with results for user to choose
    /// </summary>
    /// <param name="query">Search query (serial number, CRC32, MD5, SHA1, etc.)</param>
    /// <returns>Parsed RedumpDisc object (if single result)</returns>
    public async Task<RedumpDisc> SearchRedumpByQuickSearchAsync(string query)
    {
        var searchResults = await SearchRedumpQuickSearchAsync(query);

        if (searchResults.Results.Count == 0)
            throw new Exception($"No discs found matching search query: {query}");

        if (searchResults.Results.Count == 1)
        {
            // Auto-select single result
            return ParseRedumpPage($"http://redump.org/disc/{searchResults.Results[0].DiscId}/");
        }

        // Multiple results - throw exception with formatted list
        var resultsList = string.Join("\n", searchResults.Results.Select((r, i) =>
            $"[{r.DiscId}] {r.Title} | {r.System} | {r.Region} | Version: {r.Version} | Edition: {r.Edition}"));

        throw new InvalidOperationException($"Multiple results found. Please specify disc ID:\n\n{resultsList}");
    }

    /// <summary>
    /// Clean HTML source by removing online and footer sections
    /// </summary>
    private string CleanHtmlSource(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;

        // Remove the online users section
        html = Regex.Replace(html, @"<div\s+id=""online""[^>]*>.*?</div>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Remove the footer section
        html = Regex.Replace(html, @"<div\s+id=""footer""[^>]*>.*?</div>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        return html;
    }

    private RedumpDisc ParseDocument(HtmlDocument doc)
    {
        var disc = new RedumpDisc();

        disc.Title = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText.Trim() ?? "N/D";

        var gameInfoRows = doc.DocumentNode.SelectNodes("//table[@class='gameinfo']//tr");
        if (gameInfoRows != null)
        {
            var gameInfo = new GameInfo();

            foreach (var row in gameInfoRows)
            {
                var header = row.SelectSingleNode("th")?.InnerText.Trim();
                var td = row.SelectSingleNode("td");
                if (td == null) continue;
                string val = td.InnerText.Trim();

                switch (header)
                {
                    case "System":
                        gameInfo.System = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Media":
                        gameInfo.Media = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Category":
                        gameInfo.Category = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Serial":
                        gameInfo.Serial = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Region":
                        gameInfo.Region = td.SelectSingleNode(".//img")?.GetAttributeValue("title", "") is string s && !string.IsNullOrWhiteSpace(s) ? s : null;
                        break;
                    case "Languages":
                        gameInfo.Languages = td.SelectNodes("img")?.Select(i => i.GetAttributeValue("title", "")).ToList() ?? new();
                        break;
                    case "Build date":
                        gameInfo.BuildDate = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "EXE date":
                        gameInfo.ExeDate = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Version":
                        gameInfo.Version = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Edition":
                        gameInfo.Edition = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "EDC":
                        gameInfo.Edc = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Anti-modchip":
                        gameInfo.AntiModchip = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "LibCrypt":
                        gameInfo.LibCrypt = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Errors count":
                        gameInfo.ErrorsCount = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Number of tracks":
                        if (int.TryParse(val, out int numTracks))
                        {
                            gameInfo.NumberOfTracks = numTracks;
                        }
                        else
                        {
                            gameInfo.NumberOfTracks = null;
                        }
                        break;
                    case "Write offset":
                        gameInfo.WriteOffset = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Added":
                        gameInfo.AddedDate = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                    case "Last modified":
                        gameInfo.LastModifiedDate = string.IsNullOrWhiteSpace(val) ? null : val;
                        break;
                }
            }

            disc.GameInfo = gameInfo;
        }

        var tracksTable = doc.DocumentNode.SelectSingleNode("//table[@class='tracks']");
        if (tracksTable != null)
        {
            var h3Node = tracksTable.SelectSingleNode(".//h3");
            if (h3Node != null)
            {
                string h3Html = h3Node.InnerHtml;

                // Extract Track status (before " | Cuesheet")
                string trackSection = h3Html.Contains(" | Cuesheet")
                    ? h3Html.Split(" | Cuesheet")[0]
                    : h3Html;

                HtmlDocument tempDoc = new HtmlDocument();
                tempDoc.LoadHtml($"<div>{trackSection}</div>");
                var trackImages = tempDoc.DocumentNode.SelectNodes(".//img");

                if (trackImages != null && trackImages.Count > 0)
                {
                    var statusParts = new List<string>();
                    foreach (var img in trackImages)
                    {
                        var alt = img.GetAttributeValue("alt", "");
                        if (!string.IsNullOrEmpty(alt))
                        {
                            statusParts.Add(alt);
                        }
                    }
                    disc.TrackStatus = string.Join(" | ", statusParts);
                }

                // Extract Cuesheet status (after " | Cuesheet")
                if (h3Html.Contains(" | Cuesheet"))
                {
                    string cuesheetSection = h3Html.Split(" | Cuesheet")[1];
                    HtmlDocument cuesheetDoc = new HtmlDocument();
                    cuesheetDoc.LoadHtml($"<div>{cuesheetSection}</div>");
                    var cuesheetImages = cuesheetDoc.DocumentNode.SelectNodes(".//img");

                    if (cuesheetImages != null && cuesheetImages.Count > 0)
                    {
                        var statusParts = new List<string>();
                        foreach (var img in cuesheetImages)
                        {
                            var alt = img.GetAttributeValue("alt", "");
                            if (!string.IsNullOrEmpty(alt))
                            {
                                statusParts.Add(alt);
                            }
                        }
                        disc.CuesheetStatus = string.Join(" | ", statusParts);
                    }
                }
            }

            var trackRows = tracksTable.SelectNodes(".//tr[td]");
            if (trackRows != null)
            {
                disc.Tracks = new List<DiscTrack>();
                foreach (var row in trackRows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols?.Count >= 6)
                    {
                        // Handle two formats:
                        // Extended (9 cols): #, Type, Pregap, Length, Sectors, Size, CRC-32, MD5, SHA-1
                        // Simple (6 cols):   #, Sectors, Size, CRC-32, MD5, SHA-1

                        string number = cols[0].InnerText.Trim();
                        string type = "";
                        string pregap = "";
                        string length = "";
                        string sectors = "";
                        string size = "";
                        string crc32 = "";
                        string md5 = "";
                        string sha1 = "";

                        if (cols.Count >= 9)
                        {
                            // Extended format
                            type = cols[1].InnerText.Trim();
                            pregap = cols[2].InnerText.Trim();
                            length = cols[3].InnerText.Trim();
                            sectors = cols[4].InnerText.Trim();
                            size = cols[5].InnerText.Trim();
                            crc32 = cols[6].InnerText.Trim();
                            md5 = cols[7].InnerText.Trim();
                            sha1 = cols[8].InnerText.Trim();
                        }
                        else if (cols.Count == 6)
                        {
                            // Simple format (# Sectors, Size, CRC-32, MD5, SHA-1)
                            type = "Data";
                            pregap = "";
                            length = "";
                            sectors = cols[1].InnerText.Trim();
                            size = cols[2].InnerText.Trim();
                            crc32 = cols[3].InnerText.Trim();
                            md5 = cols[4].InnerText.Trim();
                            sha1 = cols[5].InnerText.Trim();
                        }

                        if (!string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(size))
                        {
                            disc.Tracks.Add(new DiscTrack(number, type, pregap, length, sectors, size, crc32, md5, sha1));
                        }
                    }
                }
            }
        }

        var ringRows = doc.DocumentNode.SelectNodes("//table[@class='rings']//tr[td]");
        if (ringRows != null)
        {
            DiscRing? currentRing = null;

            foreach (var row in ringRows)
            {
                var cols = row.SelectNodes("td");
                if (cols?.Count >= 5)
                {
                    string GetCleanText(HtmlNode node)
                    {
                        var nullSpan = node.SelectSingleNode("span[@class='null']");
                        if (nullSpan != null) return "";
                        return HtmlEntity.DeEntitize(node.InnerText).Trim();
                    }

                    // Check if this row is a new ring (first column contains a number)
                    string firstCol = GetCleanText(cols[0]);
                    if (int.TryParse(firstCol, out int ringNumber))
                    {
                        // This is a new ring entry
                        string status = "";

                        // Look for status image in column 5 or beyond
                        if (cols.Count >= 6)
                        {
                            var img = cols[5].SelectSingleNode(".//img");
                            if (img != null)
                            {
                                status = img.GetAttributeValue("alt", "");
                            }
                        }
                        else if (cols.Count >= 7)
                        {
                            var img = cols[6].SelectSingleNode(".//img");
                            if (img != null)
                            {
                                status = img.GetAttributeValue("alt", "");
                            }
                        }

                        currentRing = new DiscRing(
                            firstCol, GetCleanText(cols[1]), GetCleanText(cols[2]),
                            GetCleanText(cols[3]), GetCleanText(cols[4]), status
                        );
                        disc.Rings.Add(currentRing);
                    }
                    else if (currentRing != null && cols.Count >= 4)
                    {
                        // This is a continuation row for the current ring (multiple rows per ring)
                        // Update the current ring with additional data if needed
                        // For now, we keep the first row's data and status
                    }
                }
            }
        }


        // Parse Security sector ranges
        var ssrTable = doc.DocumentNode.SelectSingleNode("//table[contains(@class,'ssranges')]");
        if (ssrTable != null)
        {
            var ssrRows = ssrTable.SelectNodes(".//tr[td]");
            if (ssrRows != null)
            {
                foreach (var row in ssrRows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols != null && cols.Count >= 3)
                    {
                        // Try to parse number, start, end
                        if (int.TryParse(cols[0].InnerText.Trim(), out int number) &&
                            int.TryParse(cols[1].InnerText.Trim(), out int start) &&
                            int.TryParse(cols[2].InnerText.Trim(), out int end))
                        {
                            disc.SecuritySectorRanges.Add(new SecuritySectorRange(number, start, end));
                        }
                    }
                }
            }
            // Check for a note row with a th element and colspan
            var noteRow = ssrTable.SelectSingleNode(".//tr[th[@colspan='3']]");
            if (noteRow != null)
            {
                var noteCell = noteRow.SelectSingleNode(".//th[@colspan='3']");
                if (noteCell != null)
                {
                    var note = noteCell.InnerText.Trim();
                    if (!string.IsNullOrEmpty(note) && disc.SecuritySectorRanges.Count > 0)
                    {
                        // Attach note to last range
                        disc.SecuritySectorRanges[disc.SecuritySectorRanges.Count - 1].Note = note;
                    }
                }
            }
        }

        var pvdTable = doc.DocumentNode.SelectSingleNode("//table[@class='pvd']");
        if (pvdTable != null)
        {
            var h3Node = pvdTable.SelectSingleNode(".//h3");
            if (h3Node != null)
            {
                var images = h3Node.SelectNodes(".//img");
                if (images != null && images.Count > 0)
                {
                    var statusParts = new List<string>();
                    foreach (var img in images)
                    {
                        var alt = img.GetAttributeValue("alt", "");
                        if (!string.IsNullOrEmpty(alt))
                        {
                            statusParts.Add(alt);
                        }
                    }
                    disc.PvdStatus = string.Join(" | ", statusParts);
                }
            }

            var pvdRows = pvdTable.SelectNodes(".//tr[td]");
            if (pvdRows != null)
            {
                foreach (var row in pvdRows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols?.Count >= 5)
                    {
                        disc.PvdEntries.Add(new PvdRecord(
                            cols[0].InnerText.Trim(), cols[1].InnerText.Trim(), cols[2].InnerText.Trim(),
                            cols[3].InnerText.Trim(), cols[4].InnerText.Trim()
                        ));
                    }
                }
            }
        }

        var libcryptTable = doc.DocumentNode.SelectSingleNode("//table[@class='libcrypt']");
        if (libcryptTable != null)
        {
            var libcryptRows = libcryptTable.SelectNodes(".//tr[td]");
            if (libcryptRows != null)
            {
                foreach (var row in libcryptRows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols?.Count >= 5)
                    {
                        string sector = cols[0].InnerText.Trim();
                        string msf = cols[1].InnerText.Trim();
                        string contents = cols[2].InnerText.Trim();
                        string xor = cols[3].InnerText.Trim();
                        string comments = cols[4].InnerText.Trim();

                        // Skip the "Total" row
                        if (sector != "" && !sector.StartsWith("Total"))
                        {
                            disc.LibCryptSectors.Add(new LibCryptSector(sector, msf, contents, xor, comments));
                        }
                    }
                }
            }
        }

        // Parse gamecomments table for Metadata, Comments, and Contents
        var gamecommentsTable = doc.DocumentNode.SelectSingleNode("//table[@class='gamecomments']");
        if (gamecommentsTable != null)
        {
            var gameComments = new GameComments();

            // Extract Metadata
            var metadataHeader = gamecommentsTable.SelectSingleNode(".//th[text()='Metadata']");
            if (metadataHeader != null)
            {
                var metadataNode = metadataHeader.SelectSingleNode("./../following-sibling::tr/td");
                if (metadataNode != null)
                {
                    gameComments.Metadata = HtmlEntity.DeEntitize(metadataNode.InnerText).Trim();
                }
            }

            // Extract Comments
            var commentsHeader = gamecommentsTable.SelectSingleNode(".//th[text()='Comments']");
            if (commentsHeader != null)
            {
                var commentsNode = commentsHeader.SelectSingleNode("./../following-sibling::tr/td");
                if (commentsNode != null)
                {
                    string cleanComments = Regex.Replace(commentsNode.InnerHtml, @"<br\s*/?>", Environment.NewLine, RegexOptions.IgnoreCase);
                    gameComments.Comments = HtmlEntity.DeEntitize(Regex.Replace(cleanComments, @"<[^>]*>", "")).Trim();
                }
            }

            // Extract Contents
            var contentsHeader = gamecommentsTable.SelectSingleNode(".//th[text()='Contents']");
            if (contentsHeader != null)
            {
                var contentsNode = contentsHeader.SelectSingleNode("./../following-sibling::tr/td");
                if (contentsNode != null)
                {
                    string cleanContents = Regex.Replace(contentsNode.InnerHtml, @"<br\s*/?>", Environment.NewLine, RegexOptions.IgnoreCase);
                    string strippedContents = Regex.Replace(cleanContents, @"<div[^>]*>", "").Replace("</div>", "").Replace("[+]", "");
                    gameComments.Contents = HtmlEntity.DeEntitize(Regex.Replace(strippedContents, @"<[^>]*>", "")).Trim();
                }
            }

            // Extract Barcode
            var barcodeNode = doc.DocumentNode.SelectSingleNode("//th[text()='Barcode']/../following-sibling::tr/td");
            if (barcodeNode != null) gameComments.Barcode = barcodeNode.InnerText.Trim();

            // Only set GameComments if at least one field has a value
            if (!string.IsNullOrWhiteSpace(gameComments.Metadata) ||
                !string.IsNullOrWhiteSpace(gameComments.Comments) ||
                !string.IsNullOrWhiteSpace(gameComments.Contents) ||
                !string.IsNullOrWhiteSpace(gameComments.Barcode))
            {
                disc.GameComments = gameComments;
            }
        }

        var headerTable = doc.DocumentNode.SelectSingleNode("//table[@class='header']");
        if (headerTable != null)
        {
            var headerRows = headerTable.SelectNodes(".//tr[td]");
            if (headerRows != null)
            {
                var h3Node = headerTable.SelectSingleNode(".//h3");
                if (h3Node != null)
                {
                    var images = h3Node.SelectNodes(".//img");
                    if (images != null && images.Count > 0)
                    {
                        var statusParts = new List<string>();
                        foreach (var img in images)
                        {
                            var alt = img.GetAttributeValue("alt", "");
                            if (!string.IsNullOrEmpty(alt))
                            {
                                statusParts.Add(alt);
                            }
                        }
                        disc.HeaderStatus = string.Join(" | ", statusParts);
                    }
                }

                foreach (var row in headerRows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols?.Count >= 3)
                    {
                        // Decode HTML entities and normalize non-breaking spaces to regular spaces
                        var firstCol = HtmlEntity.DeEntitize(cols[0].InnerText.Trim()).Replace("\u00A0", " ");
                        if (firstCol == "Row" || string.IsNullOrEmpty(firstCol))
                            continue;

                        // Skip the "Total" row
                        if (firstCol.Contains("Total"))
                            continue;

                        disc.HeaderEntries.Add(new HeaderEntry(
                            firstCol,
                            HtmlEntity.DeEntitize(cols[1].InnerText.Trim()).Replace("\u00A0", " "),
                            HtmlEntity.DeEntitize(cols[2].InnerText.Trim()).Replace("\u00A0", " ")
                        ));
                    }
                }
            }
        }

        // Parse Metadata section
        var metadataTables = doc.DocumentNode.SelectNodes("//table[@class='rings']");
        if (metadataTables != null)
        {
            foreach (var table in metadataTables)
            {
                var h3Node = table.SelectSingleNode(".//h3");
                if (h3Node != null && h3Node.InnerText.Trim() == "Metadata")
                {
                    // Find all rows that have data (tr with td, not th)
                    var allRows = table.SelectNodes(".//tr");
                    if (allRows != null && allRows.Count > 0)
                    {
                        // Skip header row(s) with th elements, find data row(s) with td
                        foreach (var row in allRows)
                        {
                            var hasTh = row.SelectSingleNode(".//th") != null;
                            var hasTd = row.SelectSingleNode(".//td") != null;

                            // Skip header rows, only process data rows
                            if (hasTh || !hasTd)
                                continue;

                            var cols = row.SelectNodes("td");
                            if (cols?.Count >= 3)
                            {
                                string discKey = cols[0].InnerText.Trim();
                                string discId = cols[1].InnerText.Trim();
                                // For PIC, replace <br> tags with newlines and clean up
                                string pic = Regex.Replace(cols[2].InnerHtml, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
                                pic = HtmlEntity.DeEntitize(Regex.Replace(pic, @"<[^>]*>", "")).Trim();

                                disc.Metadata = new Metadata(discKey, discId, pic);
                                break; // Found the data row, exit loop
                            }
                        }
                    }
                    break; // Found metadata, no need to continue
                }
            }
        }

        return disc;
    }
}