using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class Id96925Fixture
{
    public RedumpDisc Disc { get; private set; }

    public Id96925Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "id-96925.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }

        string htmlContent = File.ReadAllText(filePath);
        
        Disc = scraper.ParseRedumpHtml(htmlContent);
        Disc.Id = "96925";
    }
}
