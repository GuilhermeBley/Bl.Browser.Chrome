// See https://aka.ms/new-console-template for more information
using PuppeteerSharp;
using System.Net;

await new BrowserFetcher().DownloadAsync();
var browser = await Puppeteer.LaunchAsync(new LaunchOptions 
{ 
    Headless = false,
    Args = new[] { "--no-first-run", "--no-default-browser-check" }
});

var page = await browser.NewPageAsync();

//await page.SetRequestInterceptionAsync(true);
page.Request += async (sender, e) =>
{
    var req = e.Request;
    Console.WriteLine($"Requesting to ({req.Method}) {req.Url}.");
    if (req.HasPostData)
    {
        var preview = await req.FetchPostDataAsync();
        Console.WriteLine($"    Response Body: {string.Concat(preview.Take(100))}");
    }
};
page.Response += async (sender, e) =>
{
    Console.WriteLine($"Response to ({e.Response.Request.Method}) {e.Response.Request.Url}");

    var acceptableMethods = new string[]
    {
        "get", "post", "patch", "put"
    };
    if (acceptableMethods.Contains(e.Response.Request.Method.Method, StringComparer.OrdinalIgnoreCase))
        try
        {
            var preview = await e.Response.TextAsync();
            Console.WriteLine($"    Response Body: {string.Concat(preview.Take(100))}");
        }
        catch { } // ignoring
};

var title = await page.EvaluateExpressionAsync<string>("document.title");
Console.WriteLine($"Page title: {title}");

await page.GoToAsync("https://portalservicos.senatran.serpro.gov.br/#/infracoes/orgaos-autuadores-sne");
await Task.Delay(1000*60);

Console.WriteLine("Done. Press any key to close.");
Console.ReadKey();
await browser.CloseAsync();