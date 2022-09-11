using System.Security.Cryptography;
using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Effect.Traits;

public readonly record struct HttpIO(HttpContext HttpContext)
{
    public async ValueTask<string> RequestBodyToSha512Async()
    {
        HttpContext.Request.EnableBuffering();

        using var sha = SHA512.Create();

        var raw = await sha.ComputeHashAsync(HttpContext.Request.Body);

        HttpContext.Request.Body.Position = 0;

        return Convert.ToBase64String(raw);
    }

    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public ValueTask<T> RequestBodyToAsync<T>()
    {
        return JsonSerializer.DeserializeAsync<T>(HttpContext.Request.Body, JsonSerializerOptions);
    }
    

    public async ValueTask<Unit> ResponseOkAsync(string response)
    {
        await HttpContext.Response.WriteAsync(response);

        return Unit.Default;
    }

    public async ValueTask<Unit> ResponseAsync<T>(T response)
    {
        var context = HttpContext;
        await (response switch
        {
            Error err => Task.Run(() =>
            {
                context.Response.StatusCode = err.Code;
                context.
            }),
            object msg => context.Response.WriteAsJsonAsync(response),
            _ => Task.CompletedTask,
        });

        return Unit.Default;
    }
}
