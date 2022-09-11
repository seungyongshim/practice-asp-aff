using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Effect;
using WebApplication1.Effect.Traits;
using static LanguageExt.Prelude;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    public TodosController(ILogger<TodosController> logger)
    {
        Logger = logger;
    }

    public ILogger<TodosController> Logger { get; }

    ISet<string> Todos { get; } = new System.Collections.Generic.HashSet<string>()
    {
        "a", "b", "c"
    };

    [HttpPost]
    [Consumes(typeof(TodoDto), "application/json")]
    public async Task PostTodos(CancellationToken token)
    {
        var q = from ___ in unitEff
                from __1 in AspController<RT>.RequestBodyToSha512Aff()
                from __2 in AspController<RT>.WriteResponseAff(__1)
                select unit;

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        await q.Run(new RT(HttpContext, cts));
    }

    public readonly record struct RT(HttpContext HttpContext, CancellationTokenSource CancellationTokenSource) : HasHttp<RT>
    {
        public RT LocalCancel => this;
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
