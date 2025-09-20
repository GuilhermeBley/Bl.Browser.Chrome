// See https://aka.ms/new-console-template for more information
using PuppeteerSharp;

// 1. OPTION A: Connect to an existing browser via WebSocket
// First, you must manually launch Chrome:
// "C:\Program Files\Google\Chrome\Application\chrome.exe" --remote-debugging-port=9222
Console.WriteLine("Connecting to existing browser...");
var browser = await Puppeteer.ConnectAsync(new ConnectOptions
{
    BrowserWSEndpoint = await GetBrowserWebSocketEndpoint()
});

// 1. OPTION B: Launch a new browser instance (downloads Chromium if needed)
// await new BrowserFetcher().DownloadAsync();
// var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false });

// 2. Create a new page and start controlling it
var page = await browser.NewPageAsync();
await page.GoToAsync("https://www.google.com");

// 3. Take a screenshot
await page.ScreenshotAsync("google.png");

// 4. Evaluate JavaScript in the page
var title = await page.EvaluateExpressionAsync<string>("document.title");
Console.WriteLine($"Page title: {title}");

// 5. Type into the search box (assuming you know the selector)
await page.TypeAsync("input[name='q']", "Hello from C#!");

Console.WriteLine("Done. Press any key to close.");
Console.ReadKey();
await browser.CloseAsync();

// Helper method to fetch the correct WebSocket URL from the JSON endpoint
static async Task<string> GetBrowserWebSocketEndpoint()
{
    using var httpClient = new HttpClient();
    var response = await httpClient.GetStringAsync("http://127.0.0.1:9222/json/version");
    var data = System.Text.Json.JsonDocument.Parse(response);
    return data.RootElement.GetProperty("webSocketDebuggerUrl").GetString();
}