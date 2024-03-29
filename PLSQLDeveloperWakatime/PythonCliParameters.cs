﻿using System.Collections.ObjectModel;
using System.Linq;

namespace WakaTime
{
    internal class PythonCliParameters
    {
        private static string Cli => Dependencies.CliLocation;
        public string Key { get; set; }
        public string Entity { get; set; }
        public string lineNo { get; set; }
        public string Time { get; set; }
        public string Plugin { get; set; }
        public bool IsWrite { get; set; }
        public string Project { get; set; }
        public bool HasExtraHeartbeats { get; set; }

        public string[] ToArray(bool obfuscate = false)
        {
            var parameters = new Collection<string>
            {
                Cli,
                "--key",
                obfuscate ? $"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX{Key.Substring(Key.Length - 4)}" : Key,
                "--entity",
                Entity,
                "--time",
                Time,
                "--plugin",
                Plugin
            };

            if (IsWrite)
                parameters.Add("--write");

            if (!string.IsNullOrEmpty(lineNo))
            {
                parameters.Add("--lineno");
                parameters.Add(lineNo);
            }
            if (!string.IsNullOrEmpty(Project))
            {
                parameters.Add("--project");
                parameters.Add(Project);
            }

            if (HasExtraHeartbeats)
                parameters.Add("--extra-heartbeats");

            return parameters.ToArray();
        }
    }
}