using FluentAssertions;
using spy_detection.Services;
using Xunit;

namespace spy_detection.tests
{
    public class SpyDetectorTests
    {
        [Fact]
        public void ShouldMatchExactConsecutiveSequenceTest()
        {
            // Message Code Name Spy Contains spy?
            // [1,2,4,0,0,7,5] [0,0,7] James Bond true
            var detector = new SpyDetector();

            //Execute test
            var result = detector.ContainsSpy(new[] { 1, 2, 4, 0, 0, 7, 5 }, new[] { 0, 0, 7 });

            // Verify test result
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldMatchConsecutiveSequenceTest()
        {
            // Message Code Name Spy Contains spy?
            // [0,2,2,0,4,7,0] [0,0,7] James Bond true
            var detector = new SpyDetector();

            //Execute test
            var result = detector.ContainsSpy(new[] { 0, 2, 2, 0, 4, 7, 0 }, new[] { 0, 0, 7 });

            // Verify test result
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotMatchNonConsecutiveSequenceTest()
        {
            // Message Code Name Spy Contains spy?
            // [1,2,0,7,4,4,0] [0,0,7] James Bond false
            var detector = new SpyDetector();

            //Execute test
            var result = detector.ContainsSpy(new[] { 1, 2, 0, 7, 4, 4, 0 }, new[] { 0, 0, 7 });

            // Verify test result
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldMatchConsecutiveSequenceWithDuplicateTest()
        {
            // Message Code Name Spy Contains spy?
            // [3,3,1,5,1,4,4] [3,1,4] Ethan Hunt true
            var detector = new SpyDetector();

            //Execute test
            var result = detector.ContainsSpy(new[] { 3, 3, 1, 5, 1, 4, 4 }, new[] { 3, 1, 4 });

            // Verify test result
            result.Should().BeTrue();
        }
    }
}
