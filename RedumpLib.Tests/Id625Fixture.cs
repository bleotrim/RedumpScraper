using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class Id625Fixture
{
    public RedumpDisc Disc { get; private set; }

    public Id625Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "id-625.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }

        string htmlContent = File.ReadAllText(filePath);
        
        Disc = scraper.ParseRedumpHtml(htmlContent);
        Disc.Id = "625";
    }
}
