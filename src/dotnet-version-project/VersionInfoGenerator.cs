using System;
using System.Collections.Generic;

namespace DotNet.Tool.Version.Project
{
    static class VersionInfoGenerator
    {
        static readonly IDictionary<string, IVersionInfoGenerator> s_InstancesByType
            = new Dictionary<string, IVersionInfoGenerator>
            {
                ["cs"] = new CsVersionInfoGenerator()
            };

        public static IVersionInfoGenerator ForLanguage(string language)
        {
            if (language == null) throw new ArgumentNullException(nameof(language));

            IVersionInfoGenerator versionInfoGenerator;
            if (!s_InstancesByType.TryGetValue(language, out versionInfoGenerator))
            {
                throw new NotSupportedException($"Language '{language}' is not supported.");
            }
            return versionInfoGenerator;
        }
    }
}