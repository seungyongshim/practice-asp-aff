using System.Security.Cryptography;
using LanguageExt;

namespace WebApplication1.Effect.Traits;

public readonly record struct HttpIO(HttpContext HttpContext)
{
    public async Task<string> RequestBodyToSha512Async()
    {
        HttpContext.Request.EnableBuffering();

        using var sha = SHA512.Create();

        var raw = await sha.ComputeHashAsync(HttpContext.Request.Body);

        return Convert.ToBase64String(raw);
    }

    public async Task<Unit> WriteResponseAsync(string response)
    {
        await HttpContext.Response.WriteAsync(response);

        return Unit.Default;
    }
}
