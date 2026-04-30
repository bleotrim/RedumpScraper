using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class Id2509Fixture
{
    public RedumpDisc Disc { get; private set; }

    public Id2509Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "id-2509.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }

        string htmlContent = File.ReadAllText(filePath);
        
        Disc = scraper.ParseRedumpHtml(htmlContent);
        Disc.Id = "2509";
    }
}
