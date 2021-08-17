using CaelumCoreLibrary.Cards;
using System;
using Xunit;

namespace CaelumCoreLibrary.Tests.Cards
{
    public class CardValidationTests
    {
        [Fact]
        public void GetValidCardId_BasicIdShouldWork()
        {
            // Arrange
            var expected = "author_card";

            // Act
            var actual = CardValidation.GetValidCardId("author", "card");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("T-Pose Ratkechi", "Caelum Music Manager", "tposeratkechi_caelummusicmanager")]
        [InlineData("T-Pose(Ráatkechi", "Caelum!Music+Manager", "tposeratkechi_caelummusicmanager")]
        [InlineData("T-Pose Ratkechi", "Caelum Music Manager2", "tposeratkechi_caelummusicmanager2")]
        public void GetValidCardId_SymbolsRemovedShouldWork(string authorName, string cardName, string expected)
        {
            // Arrange

            // Act
            var actual = CardValidation.GetValidCardId(authorName, cardName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Caelum Music Manager", "authorName")]
        [InlineData("T-Pose Ratkechi", null, "cardName")]
        [InlineData("     ", "Caelum Music Manager", "authorName")]
        public void GetValidCardId_EmptyParamShouldFail(string authorName, string cardName, string param)
        {
            // Assert
            Assert.Throws<ArgumentException>(param, () => CardValidation.GetValidCardId(authorName, cardName));
        }

        [Theory]
        [InlineData("!!!", "Caelum Music Manager")]
        [InlineData("T-Pose Ratkechi", "ááá")]
        [InlineData("???", "Caelum Music Manager")]
        public void GetValidCardId_CouldNotGetValidIdShouldFail(string authorName, string cardName)
        {
            // Arrange
            string expected = null;

            // Act
            var actual = CardValidation.GetValidCardId(authorName, cardName);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
