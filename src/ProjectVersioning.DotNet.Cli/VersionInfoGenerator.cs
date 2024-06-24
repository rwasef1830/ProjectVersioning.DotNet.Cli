using System;
using System.Collections.Generic;

namespace ProjectVersioning.DotNet.Cli
{
    static class VersionInfoGenerator
    {
        static readonly IDictionary<string, IVersionInfoGenerator> s_InstancesByType
            = new Dictionary<string, IVersionInfoGenerator>
            {
                [string.Empty] = new ConsoleVersionInfoGenerator(),
                ["cs"] = new CsVersionInfoGenerator()
            };

        public static IVersionInfoGenerator ForLanguage(string language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (!s_InstancesByType.TryGetValue(language, out var versionInfoGenerator))
            {
                throw new NotSupportedException($"Language '{language}' is not supported.");
            }
            return versionInfoGenerator;
        }
    }
}