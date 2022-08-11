using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LaunchwaresSubprocess.Helpers
{
    /// <summary>
    /// Custom StartupArgs Array
    /// </summary>
    public class StartupArgs
    {
        public List<string> _Args { get; set; }

        /// <summary>
        /// Ctx
        /// </summary>
        /// <param name="Args">Arg array</param>
        public StartupArgs(string[] Args) => _Args = Args.ToList();

        public string GetValue(string key)
        {
            if (!_Args.Contains(key)) return "Input could not found.";

            string raw = "";
            _Args.ForEach(x =>
            {
                raw += $"{x} ";
            });
            var matched = Regex.Match(raw, $"(?<={key} ).*?(?= -)").Value ?? "Input could not found.";
            return matched;
        }

        public int Length => _Args.Count;
    }
}
