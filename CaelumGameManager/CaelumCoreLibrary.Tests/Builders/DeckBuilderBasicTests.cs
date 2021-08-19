using Autofac;
using Autofac.Extras.Moq;
using CaelumCoreLibrary.Builders;
using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Xunit;

namespace CaelumCoreLibrary.Tests.Builders
{
    public class DeckBuilderBasicTests
    {
        [Fact]
        public void Build_NoCardsToBuild_ShouldWork()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var testDir = @"C:\Users\Example User\Example Folder";

            mock.Mock<IFileSystem>().Setup(x => x.Directory.GetFiles(testDir)).Returns(new string[] { });

            var deckBuilderBasic = mock.Create<DeckBuilderBasic>();

            // Act
            var exception = Record.Exception(() => deckBuilderBasic.Build(new List<CardModel>() { }, testDir));

            // Assert
            Assert.Null(exception);
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

        [Fact]
        public void PrepareOutputDirectory_MaxFileDeleteLimitReached_ShouldFail()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var testDir = @"C:\Users\Example User\Example Folder";
            var tooManyFiles = DeckBuilderUtilities.MaxFilesAllowedForDeleting + 1;

            mock.Mock<IFileSystem>().Setup(x => x.Directory.GetFiles(testDir)).Returns(new string[tooManyFiles + 1]);

            var deckBuilderBasic = mock.Create<DeckBuilderBasic>();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => deckBuilderBasic.PrepareOutputFolder(testDir));
        }
    }
}
