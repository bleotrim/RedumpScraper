using System.Collections.Generic;

namespace RedumpLib;

/// <summary>
/// Represents a single search result from Redump quicksearch
/// </summary>
public class SearchResult
{
    public string DiscId { get; set; } = "";
    public string Title { get; set; } = "";
    public string System { get; set; } = "";
    public string Region { get; set; } = "";
    public string Serial { get; set; } = "";
    public string Version { get; set; } = "";
    public string Edition { get; set; } = "";
    public List<string> Languages { get; set; } = new();
}

/// <summary>
/// Container for multiple search results
/// </summary>
public class SearchResultsContainer
{
    public List<SearchResult> Results { get; set; } = new();
    public string SearchQuery { get; set; } = "";
}
