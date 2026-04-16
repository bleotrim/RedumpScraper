using System;
using System.IO;
using RedumpLib;

namespace RedumpLib.Tests;

public class RedumpFixture
{
    public RedumpDisc Disc { get; private set; }

    public RedumpFixture()
    {
        var scraper = new Scraper();
        
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "ID_17031.html");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Unable to find test file at: {filePath}");
        }

        string htmlContent = File.ReadAllText(filePath);
        
        Disc = scraper.ParseRedumpHtml(htmlContent);
        Disc.Id = "17031";
    }
}