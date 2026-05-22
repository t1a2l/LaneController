using System;
using System.Collections.Generic;
using System.Linq;

namespace LaneController.ModsCommon.Utilities
{
    public static class VersionExtension
    {
        public static Version Build(this Version version)
        {
            return new Version(version.Major, version.Minor, version.Build);
        }

        public static Version Minor(this Version version)
        {
            return new Version(version.Major, version.Minor);
        }

        public static Version PrevMinor(this Version version, List<Version> versions)
        {
            Version version2 = version.Build();
            bool isMinor = version2.IsMinor();
            Version toFind = version2.Minor();
            int num = versions.FindIndex((v) => v == toFind);
            if (num != -1)
            {
                Version version3 = versions.Skip(num + 1).FirstOrDefault((v) => isMinor || v.IsMinor());
                if (version3 is not null)
                {
                    return version3;
                }
            }
            return versions.LastOrDefault() ?? new Version(0, 0);
        }

        public static bool IsMinor(this Version version)
        {
            if (version.Build <= 0)
            {
                return version.Revision <= 0;
            }
            return false;
        }

        public static string GetString(this Version version)
        {
            if (version.Revision > 0)
            {
                return version.ToString(4);
            }
            if (version.Build > 0)
            {
                return version.ToString(3);
            }
            return version.ToString(2);
        }

        public static string GetStringGameFormat(this Version version, BuildConfig.ReleaseType releaseType = BuildConfig.ReleaseType.Final)
        {
            return BuildConfig.VersionToString(BuildConfig.MakeVersionNumber((uint)version.Major, (uint)version.Minor, (uint)version.Build, releaseType, (uint)version.Revision, BuildConfig.BuildType.Unknown), full: false);
        }
    }
}
