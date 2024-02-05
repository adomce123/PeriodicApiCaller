using System.Text.Json;

namespace PeriodicApiCaller.ApiFetcher.Models.SerializerPolicies
{
    public static class JsonSerializerSettings
    {
        public static JsonSerializerOptions CaseInsensitive =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
    }
}
