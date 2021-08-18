using CaelumCoreLibrary.Cards;
using System;
using Xunit;

namespace CaelumCoreLibrary.Tests.Cards
{
    public class CardParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void ParseCardFile_InvalidParam_ShouldFail(string filePath)
        {
            // Arrange
            var cardParser = new CardParser();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => cardParser.ParseCardFile(filePath));
        }
    }
}
