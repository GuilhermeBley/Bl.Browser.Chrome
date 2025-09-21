# Bl.Browser.Chrome

Project to show the use of `Puppeteer` library to scrap data.

## Fuctionalities
These are a set of methods/functionalities to use with `Puppeteer` that will help you to extract data.

### IPage.Request
This is an Event handler function provided by the library, here you can track all the requests:

```csharp
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
```

### IPage.Response
This is an Event handler function provided by the library, here you can track all the responses of the browser, being very useful for collecting internal JS API responses:

```csharp
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
```
