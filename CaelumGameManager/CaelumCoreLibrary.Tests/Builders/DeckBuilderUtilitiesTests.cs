using CaelumCoreLibrary.Builders;
using System;
using Xunit;

namespace CaelumCoreLibrary.Tests.Builders
{
    public class DeckBuilderUtilitiesTests
    {
        [Theory]
        [InlineData(@"C:\")]
        [InlineData(@"C:\Users")]
        [InlineData(@"C:\Program Files\")]
        [InlineData(@"C:\Program Files (x86)\")]
        [InlineData(@"C:\Program data")]
        [InlineData(@"C:\Users\Example User")]
        [InlineData(@"C:\users\Example user\")]
        public void IsValidOutputDirectory_ValidPathsButDisallowed_ShouldReturnFalse(string directory)
        {
            // Arrange

            // Act
            var actual = DeckBuilderUtilities.IsValidOutputDirectory(directory);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("        ")]
        public void IsValidOutputDirectory_InvalidPaths_ShouldFail(string directory)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => DeckBuilderUtilities.IsValidOutputDirectory(directory));
        }

    }
}
