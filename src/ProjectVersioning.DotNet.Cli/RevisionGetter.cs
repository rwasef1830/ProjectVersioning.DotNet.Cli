using System;
using System.Collections.Generic;

namespace ProjectVersioning.DotNet.Cli
{
    static class RevisionGetter
    {
        static readonly IDictionary<string, IRevisionGetter> s_InstancesByType = new Dictionary<string, IRevisionGetter>
        {
            ["hg"] = new HgRevisionGetter(),
            ["git"] = new GitRevisionGetter()
        };

        public static IRevisionGetter ForSourceControl(string sourceControlType)
        {
            if (sourceControlType == null) throw new ArgumentNullException(nameof(sourceControlType));

            IRevisionGetter revGetter;
            if (!s_InstancesByType.TryGetValue(sourceControlType, out revGetter))
            {
                throw new NotSupportedException($"Source control '{sourceControlType}' is not supported.");
            }
            return revGetter;
        }
    }
}