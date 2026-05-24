using System.CommandLine;

namespace KotorCLI
{
    /// <summary>
    /// System.CommandLine 2.x helpers — descriptions are set via Symbol.Description, not constructor aliases.
    /// </summary>
    internal static class Cli
    {
        internal static Option<T> Opt<T>(string name, string description)
        {
            var option = new Option<T>(name);
            option.Description = description;
            return option;
        }
    }
}
