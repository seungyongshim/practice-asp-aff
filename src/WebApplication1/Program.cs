using LanguageExt;
using WebApplication1.Dto;
using WebApplication1.Effect;
using WebApplication1.Effect.Traits;
using WebApplication1.Filters;
using static LanguageExt.Prelude;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<RequestBodyTypeFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapPost("/Todo1", async (HttpContext context, CancellationToken token) =>
{
    var q = from ___ in unitEff
            from __1 in Http<RT>.RequestBodyToSha512Aff()
            from __2 in Http<RT>.RequestBodyToAff<TodoDto>()
            from __3 in Http<RT>.ResponseAff(__2)
            select unit;

    using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
    await q.RunUnit(new RT(context, cts));
}).Accepts<TodoDto>("application/json");

await app.RunAsync();

readonly record struct RT(HttpContext HttpContext, CancellationTokenSource CancellationTokenSource) :
    HasHttp<RT>
{
    public RT LocalCancel => this;
    public CancellationToken CancellationToken => CancellationTokenSource.Token;
}
