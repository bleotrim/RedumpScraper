using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class Id277Fixture
{
    public RedumpDisc Disc { get; private set; }

    public Id277Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "id-277.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }

        string htmlContent = File.ReadAllText(filePath);
        
        Disc = scraper.ParseRedumpHtml(htmlContent);
        Disc.Id = "277";
    }
}
