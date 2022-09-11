using LanguageExt;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using static LanguageExt.Prelude;

namespace WebApplication1.Effect.Traits;

[Typeclass("*")]
public interface HasHttp<RT> : HasCancel<RT>
    where RT : struct, HasHttp<RT>
{
    HttpContext HttpContext { get; }

    Eff<RT, HttpIO> HttpEff => Eff<RT, HttpIO>(rt => new(rt.HttpContext));
}
