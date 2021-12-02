using System;
using System.Collections.Generic;
using System.Linq;

namespace VDB.Architecture.Concern.Helper
{
    public static class VersionHelpers
    {
        public static bool DoesVersionMatchRange(string version, string versionRangeStart, string versionRangeEnd, bool isStartingVersionInclusive, bool isEndingVersionInclusive, string standartVersionSeperator, string standartVersionNumber)
        {
            bool satisfiesVersionStart = String.IsNullOrWhiteSpace(versionRangeStart);
            bool satisfiesVersionEnd = String.IsNullOrWhiteSpace(versionRangeEnd);

            string[] splittedProductVersion = version.Split(standartVersionSeperator);
            string[] splittedStartingVersion = versionRangeStart?.Split(standartVersionSeperator);
            string[] splittedEndingVersion = versionRangeEnd?.Split(standartVersionSeperator);

            int maxLength = (new List<string[]> { splittedProductVersion, splittedStartingVersion, splittedEndingVersion }).Where(v => v != null).Max(v => v.Length);

            splittedProductVersion = NormalizeArray(splittedProductVersion, maxLength);
            splittedStartingVersion = NormalizeArray(splittedStartingVersion, maxLength);
            splittedEndingVersion = NormalizeArray(splittedEndingVersion, maxLength);

            bool startShouldCheckChildren = false;
            bool endShouldCheckChildren = false;
            for (int i = 0; i < splittedProductVersion.Length; i++)
            {
                int numericProductVersion = Convert.ToInt32(splittedProductVersion[i]);
                int numericStartingVersion = splittedStartingVersion == null ? 0 : Convert.ToInt32(splittedStartingVersion[i]);
                int numericEndingVersion = splittedEndingVersion == null ? 0 : Convert.ToInt32(splittedEndingVersion[i]);

                if ((!satisfiesVersionStart) || startShouldCheckChildren)
                {
                    if (numericProductVersion == numericStartingVersion)
                    {
                        startShouldCheckChildren = true;
                        if (isStartingVersionInclusive)
                        {
                            satisfiesVersionStart = true;
                        }
                    }
                    else
                    {
                        startShouldCheckChildren = false;
                        satisfiesVersionStart = numericProductVersion > numericStartingVersion;
                    }
                }

                if ((!satisfiesVersionEnd) || endShouldCheckChildren)
                {
                    if (numericProductVersion == numericEndingVersion)
                    {
                        endShouldCheckChildren = true;
                        if (isEndingVersionInclusive)
                        {
                            satisfiesVersionEnd = true;
                        }
                    }
                    else
                    {
                        endShouldCheckChildren = false;
                        satisfiesVersionEnd = numericProductVersion < numericEndingVersion;
                    }
                }

                if ((!satisfiesVersionStart && !startShouldCheckChildren) || (!satisfiesVersionEnd && !endShouldCheckChildren))
                {
                    return false;
                }
            }

            return satisfiesVersionStart && satisfiesVersionEnd;

            string[] NormalizeArray(string[] array, int desiredElementCount)
            {
                if (array != null && array.Length < maxLength)
                {
                    array = array.Concat(Enumerable.Repeat(standartVersionNumber, maxLength - array.Length)).ToArray();
                }

                return array;
            }
        }
    }
}
