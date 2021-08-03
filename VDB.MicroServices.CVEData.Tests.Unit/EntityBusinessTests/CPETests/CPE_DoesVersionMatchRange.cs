using NUnit.Framework;
using VDB.Architecture.Concern.Helper;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Tests.Unit.EntityBusinessTests.CPETests
{
    [TestFixture]
    public class CPE_DoesVersionMatchRange
    {
        private readonly string versionToCompare = "5.5.5";

        private readonly string lesserVersion = "5.5.4";
        private readonly string lesserVersionMiddle = "5.3.7";
        private readonly string lesserVersionMajor = "2.8.9";
        
        private readonly string greaterVersion = "5.5.6";
        private readonly string greaterVersionMiddle = "5.8.2";
        private readonly string greaterVersionMajor = "7.1.1";

        private string[] greaterVersions;
        private string[] lesserVersions;

        private readonly string standartVersionSeperator = ".";
        private readonly string standartVersionNumber = "0";

        [SetUp]
        public void SetUp()
        {
            greaterVersions = new string[] { greaterVersion, greaterVersionMiddle, greaterVersionMajor };
            lesserVersions = new string[] { lesserVersion, lesserVersionMiddle, lesserVersionMajor };
        }

        [Test]
        public void StartIncludingNull_EndIncludingGreater_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = currentGreaterVersion, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingLesser_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = currentLesserVersion, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingEqual_StartExcludingNull_EndExcludingNull()
        {
            CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = versionToCompare, VersionStartExcluding = null, VersionEndExcluding = null };
            Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
        }

        [Test]
        public void StartIncludingLesser_EndIncludingNull_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentLesserVersion, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingLesser_EndIncludingLesser_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentLesserVersion, VersionEndIncluding = currentLesserVersion, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingLesser_EndIncludingGreater_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                foreach (string currentGreaterVersion in greaterVersions)
                {
                    CPE cpeToTest = new CPE() { VersionStartIncluding = currentLesserVersion, VersionEndIncluding = currentGreaterVersion, VersionStartExcluding = null, VersionEndExcluding = null };
                    Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
                }
            }
        }

        [Test]
        public void StartIncludingLesser_EndIncludingEqual_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentLesserVersion, VersionEndIncluding = versionToCompare, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingGreater_EndIncludingNull_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentGreaterVersion, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingGreater_EndIncludingGreater_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentGreaterVersion, VersionEndIncluding = currentGreaterVersion, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingEqual_EndIncludingEqual_StartExcludingNull_EndExcludingNull()
        {
            CPE cpeToTest = new CPE() { VersionStartIncluding = versionToCompare, VersionEndIncluding = versionToCompare, VersionStartExcluding = null, VersionEndExcluding = null };
            Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
        }

        [Test]
        public void StartIncludingEqual_EndIncludingNull_StartExcludingNull_EndExcludingNull()
        {
            CPE cpeToTest = new CPE() { VersionStartIncluding = versionToCompare, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = null };
            Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
        }

        [Test]
        public void StartIncludingEqual_EndIncludingGreater_StartExcludingNull_EndExcludingNull()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = versionToCompare, VersionEndIncluding = currentGreaterVersion, VersionStartExcluding = null, VersionEndExcluding = null };
                Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingNull_EndExcludingGreater()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = currentGreaterVersion };
                Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingNull_EndExcludingLesser()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = currentLesserVersion };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingNull_EndExcludingEqual()
        {
            CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = versionToCompare };
            Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingLesser_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = currentLesserVersion, VersionEndExcluding = null };
                Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingLesser_EndExcludingLesser()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = currentLesserVersion, VersionEndExcluding = currentLesserVersion };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingLesser_EndExcludingGreater()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                foreach (string currentGreaterVersion in greaterVersions)
                {
                    CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = currentLesserVersion, VersionEndExcluding = currentGreaterVersion };
                    Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
                }
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingLesser_EndExcludingEqual()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = currentLesserVersion, VersionEndExcluding = versionToCompare };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingGreater_EndExcludingNull()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = currentGreaterVersion, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingGreater_EndExcludingGreater()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = currentGreaterVersion, VersionEndExcluding = currentGreaterVersion };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingEqual_EndExcludingNull()
        {
            CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = versionToCompare, VersionEndExcluding = null };
            Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
        }

        [Test]
        public void StartIncludingNull_EndIncludingNull_StartExcludingEqual_EndExcludingGreater()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = null, VersionStartExcluding = versionToCompare, VersionEndExcluding = currentGreaterVersion };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }



        [Test]
        public void StartIncludingLesser_EndIncludingNull_StartExcludingNull_EndExcludingLesser()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentLesserVersion, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = currentLesserVersion };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingGreater_EndIncludingNull_StartExcludingNull_EndExcludingGreater()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = currentGreaterVersion, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = currentGreaterVersion };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingLesser_EndIncludingNull_StartExcludingNull_EndExcludingGreater()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                foreach (string currentGreaterVersion in greaterVersions)
                {
                    CPE cpeToTest = new CPE() { VersionStartIncluding = currentLesserVersion, VersionEndIncluding = null, VersionStartExcluding = null, VersionEndExcluding = currentGreaterVersion };
                    Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
                }
            }
        }



        [Test]
        public void StartIncludingNull_EndIncludingLesser_StartExcludingLesser_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = currentLesserVersion, VersionStartExcluding = currentLesserVersion, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingGreater_StartExcludingGreater_EndExcludingNull()
        {
            foreach (string currentGreaterVersion in greaterVersions)
            {
                CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = currentGreaterVersion, VersionStartExcluding = currentGreaterVersion, VersionEndExcluding = null };
                Assert.IsFalse(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
            }
        }

        [Test]
        public void StartIncludingNull_EndIncludingGreater_StartExcludingLesser_EndExcludingNull()
        {
            foreach (string currentLesserVersion in lesserVersions)
            {
                foreach (string currentGreaterVersion in greaterVersions)
                {
                    CPE cpeToTest = new CPE() { VersionStartIncluding = null, VersionEndIncluding = currentGreaterVersion, VersionStartExcluding = currentLesserVersion, VersionEndExcluding = null };
                    Assert.IsTrue(VersionHelpers.DoesVersionMatchRange(versionToCompare, cpeToTest.VersionRangeStart, cpeToTest.VersionRangeEnd, cpeToTest.IsStartingVersionInclusive, cpeToTest.IsEndingVersionInclusive, standartVersionSeperator, standartVersionNumber));
                }
            }
        }
    }
}
