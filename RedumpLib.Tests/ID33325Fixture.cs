using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class ID33325Fixture
{
    public RedumpDisc Disc { get; private set; }

    public ID33325Fixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "ID_33325.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }

        string htmlContent = File.ReadAllText(filePath);
        
        Disc = scraper.ParseRedumpHtml(htmlContent);
        Disc.Id = "33325";
    }
}
