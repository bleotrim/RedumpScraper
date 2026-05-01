using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID192Fixture
{
    public RedumpDisc Disc { get; }

    public ID192Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "192.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }
        
        var html = File.ReadAllText(filePath);
        Disc = scraper.ParseRedumpHtml(html);
        Disc.Id = "192";
    }
}