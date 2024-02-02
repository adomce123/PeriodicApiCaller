using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher.JsonPolicies
{
    internal class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}
