using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies
{
    internal class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}
