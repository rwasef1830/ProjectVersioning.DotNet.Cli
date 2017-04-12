using FluentAssertions;
using NUnit.Framework;

namespace ProjectVersioning.DotNet.Cli.Tests
{
    [TestFixture]

    class WorkingCopyVersionTests
    {
        [Test]
        public void CleanWorkingCopy()
        {
            var wc = new WorkingCopyVersion(131071, "abcdefabcdef", false);

            var numeric = wc.ToVersion(1, 2);
            numeric.ToString().Should().Be("1.2.1.65535");

            var informational = wc.ToVersionString(1, 2, 3, "alpha");
            informational.Should().Be("1.2.3.131071-alpha+abcdefabcdef");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void EmptyVersionSuffix(string suffix)
        {
            var wc = new WorkingCopyVersion(131071, "abcdefabcdef", false);
            var informational = wc.ToVersionString(1, 2, 3, suffix);
            informational.Should().Be("1.2.3.131071+abcdefabcdef");
        }

        [Test]
        public void DirtyWorkingCopy()
        {
            var wc = new WorkingCopyVersion(131071, "abcdefabcdef", true);

            var numeric = wc.ToVersion(1, 2);
            numeric.ToString().Should().Be("1.2.32769.65535");

            var informational = wc.ToVersionString(1, 2, 3, "alpha");
            informational.Should().Be("1.2.3.131071-alpha+abcdefabcdef-dirty");
        }
    }
}