using LanguageExt;
using LanguageExt.Attributes;
using LanguageExt.Common;
using LanguageExt.Effects.Traits;
using WebApplication1.Effect.Traits;
using static LanguageExt.Prelude;

namespace WebApplication1.Effect;

public static class Http<RT>
    where RT : struct, HasHttp<RT>
{
    public static Aff<RT, string> RequestBodyToSha512Aff() =>
        from http in default(RT).HttpEff
        from _1 in Aff<RT, string>(rt => http.RequestBodyToSha512Async())
        select _1;

    public static Aff<RT, T> RequestBodyToAff<T>() =>
        from http in default(RT).HttpEff
        from _1 in Aff<RT, T>(rt => http.RequestBodyToAsync<T>())
        select _1;
    
    public static Aff<RT, Unit> ResponseOkAff(string response) =>
        from http in default(RT).HttpEff
        from _1 in Aff<RT, Unit>(rt => http.ResponseOkAsync(response))
        select unit;

    public static Aff<RT, T> CatchResponseAff<T>(Aff<RT, T> runAff) =>
        from __1 in runAff
        | @catch<RT, T>((Error err) =>
            from __ in unitEff
            from _1 in ResponseAff(err)
            from _2 in FailAff<RT, T>(err)
            select _2
        )
        select __1;

    public static Aff<RT, T> ResponseAff<T>(T response) =>
        from http in default(RT).HttpEff
        from _1 in Aff<RT, Unit>(rt => http.ResponseAsync(response))
        select response;
}
