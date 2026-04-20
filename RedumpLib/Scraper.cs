using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        return disc;
    }

    public RedumpDisc ParseRedumpHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return ParseDocument(doc);
    }

    private RedumpDisc ParseDocument(HtmlDocument doc)
    {
        var disc = new RedumpDisc();

        disc.Title = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText.Trim() ?? "N/D";

        var gameInfoRows = doc.DocumentNode.SelectNodes("//table[@class='gameinfo']//tr");
        if (gameInfoRows != null)
        {
            foreach (var row in gameInfoRows)
            {
                var header = row.SelectSingleNode("th")?.InnerText.Trim();
                var td = row.SelectSingleNode("td");
                if (td == null) continue;
                string val = td.InnerText.Trim();

                switch (header)
                {
                    case "System": disc.System = val; break;
                    case "Media": disc.Media = val; break;
                    case "Category": disc.Category = val; break;
                    case "Serial": disc.Serial = val; break;
                    case "Region": disc.Region = td.SelectSingleNode(".//img")?.GetAttributeValue("title", "") ?? ""; break;
                    case "Languages":
                        disc.Languages = td.SelectNodes("img")?.Select(i => i.GetAttributeValue("title", "")).ToList() ?? new();
                        break;
                    case "EXE date": disc.ExeDate = val; break;
                    case "Version": disc.Version = val; break;
                    case "Edition": disc.Edition = val; break;
                    case "EDC": disc.Edc = val; break;
                    case "Anti-modchip": disc.AntiModchip = val; break;
                    case "LibCrypt": disc.LibCrypt = val; break;
                    case "Errors count": disc.ErrorsCount = val; break;
                    case "Number of tracks": disc.NumberOfTracks = val; break;
                    case "Write offset": disc.WriteOffset = val; break;
                    case "Added": disc.AddedDate = val; break;
                    case "Last modified": disc.LastModifiedDate = val; break;
                }
            }
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
                foreach (var row in trackRows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols?.Count >= 9)
                    {
                        disc.Tracks.Add(new DiscTrack(
                            cols[0].InnerText.Trim(), cols[1].InnerText.Trim(), cols[5].InnerText.Trim(),
                            cols[6].InnerText.Trim(), cols[7].InnerText.Trim(), cols[8].InnerText.Trim()
                        ));
                    }
                }
            }
        }

        var ringRows = doc.DocumentNode.SelectNodes("//table[@class='rings']//tr[td]");
        if (ringRows != null)
        {
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

                    string status = "";
                    if (cols.Count >= 7)
                    {
                        var img = cols[6].SelectSingleNode(".//img");
                        if (img != null)
                        {
                            status = img.GetAttributeValue("alt", "");
                        }
                    }

                    disc.Rings.Add(new DiscRing(
                        GetCleanText(cols[0]), GetCleanText(cols[1]), GetCleanText(cols[2]),
                        GetCleanText(cols[3]), GetCleanText(cols[4]), status
                    ));
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

        var barcodeNode = doc.DocumentNode.SelectSingleNode("//th[text()='Barcode']/../following-sibling::tr/td");
        if (barcodeNode != null) disc.Barcode = barcodeNode.InnerText.Trim();

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

        var commentsNode = doc.DocumentNode.SelectSingleNode("//th[text()='Comments']/../following-sibling::tr/td");
        if (commentsNode != null)
        {
            string cleanComments = Regex.Replace(commentsNode.InnerHtml, @"<br\s*/?>", Environment.NewLine, RegexOptions.IgnoreCase);
            disc.Comments = HtmlEntity.DeEntitize(Regex.Replace(cleanComments, @"<[^>]*>", "")).Trim();
        }

        return disc;
    }
}