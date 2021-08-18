﻿using Autofac.Extras.Moq;
using CaelumCoreLibrary.Cards;
using CaelumCoreLibrary.Decks;
using System;
using System.Collections.Generic;
using Xunit;

namespace CaelumCoreLibrary.Tests.Decks
{
    public class DeckTests
    {
        [Fact]
        public void AddCard_AddingCard_ShouldAddToDeck()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICardsLoader>().Setup(x => x.GetInstalledCards()).Returns(new List<CardModel>());

                // Arrange
                var deck = mock.Create<Deck>();
                var newCard = new CardModel();

                // Act
                deck.AddCard(newCard);

                // Assert
                Assert.True(deck.Cards.Count == 1);
                Assert.Contains(newCard, deck.Cards);
            }
        }

        [Fact]
        public void AddCard_AddingDuplicateCardByInstance_ShouldFail()
        {
            // Arrange
            var deck = new Deck(null, null); // TODO: Mock.
            var newCard = new CardModel();

            // Act
            deck.AddCard(newCard);

            // Assert
            Assert.Throws<ArgumentException>(() => deck.AddCard(newCard));
        }

        [Fact]
        public void AddCard_AddingDuplicateCardByCardId_ShouldFail()
        {
            // Arrange
            var deck = new Deck(null, null); // TODO: Mock.

            var firstCard = new CardModel()
            {
                CardId = "tposeratkechi_caelummusicmanager"
            };

            var secondCard = new CardModel()
            {
                CardId = "tposeratkechi_caelummusicmanager"
            };

            // Act
            deck.AddCard(firstCard);

            // Assert
            Assert.Throws<ArgumentException>(() => deck.AddCard(secondCard));
        }

        [Fact]
        public void HideCard_HidingCard_ShouldSetHidePropTrueInDeck()
        {
            // Arrange
            var deck = new Deck(null, null); // TODO: Mock.
            var newCard = new CardModel();

            // Act
            deck.AddCard(newCard);
            deck.HideCard(newCard);

            // Assert
            Assert.Contains(newCard, deck.Cards);
            Assert.True(deck.Cards.Find(x => x == newCard).IsHidden);
        }

        [Fact]
        public void HideCard_HidingAlreadyHiddenCards_ShouldFail()
        {
            // Arrange
            var deck = new Deck(null, null); // TODO: Mock.
            var newCard = new CardModel();

            // Act
            deck.AddCard(newCard);
            deck.HideCard(newCard);

            // Assert
            Assert.Throws<ArgumentException>(() => deck.HideCard(newCard));
        }
    }
}