using LanguageExt;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using WebApplication1.Effect.Traits;
using static LanguageExt.Prelude;

namespace WebApplication1.Effect;

public static class AspController<RT>
    where RT : struct, HasHttp<RT>
{
    public static Aff<RT, string> RequestBodyToSha512Aff() =>
        from controller in default(RT).AspControllerEff
        from _1 in Aff<RT, string>(rt => controller.RequestBodyToSha512Async().ToValue())
        select _1;

    public static Aff<RT, Unit> WriteResponseAff(string response) =>
        from controller in default(RT).AspControllerEff
        from _1 in Aff<RT, Unit>(rt => controller.WriteResponseAsync(response).ToValue())
        select _1;
}
