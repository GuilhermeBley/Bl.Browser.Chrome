// See https://aka.ms/new-console-template for more information
using PuppeteerSharp;

await new BrowserFetcher().DownloadAsync();
var browser = await Puppeteer.LaunchAsync(new LaunchOptions 
{ 
    Headless = false,
    Args = new[] { "--no-first-run", "--no-default-browser-check" }
});

var page = await browser.NewPageAsync();
await page.GoToAsync("https://www.google.com");

await page.ScreenshotAsync("google.png");

var title = await page.EvaluateExpressionAsync<string>("document.title");
Console.WriteLine($"Page title: {title}");

await page.TypeAsync("input[name='q']", "Hello from C#!");

Console.WriteLine("Done. Press any key to close.");
Console.ReadKey();
await browser.CloseAsync();