using Autofac.Extras.Moq;
using CaelumCoreLibrary.Builders;
using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using Xunit;

namespace CaelumCoreLibrary.Tests.Builders
{
    public class DeckBuilderBasicTests
    {
        [Fact]
        public void Build_Simple_ShouldWork()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var deckBuilderBasic = mock.Create<DeckBuilderBasic>();

            // Act
            deckBuilderBasic.Build(new CardModel[] { }, "./");

            // Assert

        }

        [Theory]
        [InlineData(@"C:\")]
        [InlineData(@"C:\Users")]
        [InlineData(@"C:\Program Files\")]
        [InlineData(@"C:\Program Files (x86)\")]
        [InlineData(@"C:\Program data")]
        [InlineData(@"C:\Users\Example User")]
        [InlineData(@"C:\users\Example user\")]
        [InlineData("")]
        public void Build_InvalidOutputDirs_ShouldFail(string outputDir) // Possibly redundant tests but better safe than sorry.
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var deckBuilderBasic = mock.Create<DeckBuilderBasic>();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => deckBuilderBasic.PrepareOutputFolder(outputDir));
        }
    }
}
