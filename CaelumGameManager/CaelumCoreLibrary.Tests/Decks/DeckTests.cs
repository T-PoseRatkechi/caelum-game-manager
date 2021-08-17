using CaelumCoreLibrary.Cards;
using CaelumCoreLibrary.Decks;
using System;
using Xunit;

namespace CaelumCoreLibrary.Tests.Decks
{
    public class DeckTests
    {
        [Fact]
        public void AddCard_AddingCard_ShouldAddToDeck()
        {
            // Arrange
            var deck = new Deck(); // TODO: Mock.
            var newCard = new InstallableCardModel();

            // Act
            deck.AddCard(newCard);

            // Assert
            Assert.True(deck.Cards.Count == 1);
            Assert.Contains(newCard, deck.Cards);
        }

        [Fact]
        public void AddCard_AddingDuplicateCardByInstance_ShouldFail()
        {
            // Arrange
            var deck = new Deck(); // TODO: Mock.
            var newCard = new InstallableCardModel();

            // Act
            deck.AddCard(newCard);

            // Assert
            Assert.Throws<ArgumentException>(() => deck.AddCard(newCard));
        }

        [Fact]
        public void AddCard_AddingDuplicateCardByCardId_ShouldFail()
        {
            // Arrange
            var deck = new Deck(); // TODO: Mock.

            var firstCard = new InstallableCardModel()
            {
                CardId = "tposeratkechi_caelummusicmanager"
            };

            var secondCard = new InstallableCardModel()
            {
                CardId = "tposeratkechi_caelummusicmanager"
            };

            // Act
            deck.AddCard(firstCard);

            // Assert
            Assert.Throws<ArgumentException>(() => deck.AddCard(secondCard));
        }

        [Fact]
        public void HideCard_HidingCard_ShouldRemoveFromDeck()
        {
            // Arrange
            var deck = new Deck(); // TODO: Mock.
            var newCard = new InstallableCardModel();

            // Act
            deck.AddCard(newCard);
            deck.HideCard(newCard);

            // Assert
            Assert.True(deck.Cards.Count == 0);
            Assert.DoesNotContain(newCard, deck.Cards);
        }

        [Fact]
        public void DeleteCard_DeletingCard_ShouldRemoveFromDeck()
        {
            // Arrange
            var deck = new Deck(); // TODO: Mock.
            var newCard = new InstallableCardModel();

            // Act
            deck.AddCard(newCard);
            deck.DeleteCard(newCard);

            // Assert
            Assert.True(deck.Cards.Count == 0);
            Assert.DoesNotContain(newCard, deck.Cards);
        }
    }
}
