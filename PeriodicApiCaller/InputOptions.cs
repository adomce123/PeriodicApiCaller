using CommandLine;

namespace PeriodicApiCaller
{
    internal class InputOptions
    {
        [Option("cities", Separator = ',', HelpText = "List of cities separated by commas.")]
        public IEnumerable<string> Cities { get; set; } = Enumerable.Empty<string>();
    }
}
