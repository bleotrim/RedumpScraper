using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID201Fixture
{
    public RedumpDisc Disc { get; }

    public ID201Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "ID_201.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }
        
        var html = File.ReadAllText(filePath);
        Disc = scraper.ParseRedumpHtml(html);
        Disc.Id = "201";
    }
}