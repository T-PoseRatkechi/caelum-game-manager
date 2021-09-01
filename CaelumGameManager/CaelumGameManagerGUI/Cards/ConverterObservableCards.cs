// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Cards
{
    using System.Collections.Generic;
    using System.Linq;
    using CaelumCoreLibrary.Cards;
    using CaelumGameManagerGUI.Models;

    /// <summary>
    /// Converts cards to their observable counterpart.
    /// </summary>
    public static class ConverterObservableCards
    {
        public static IEnumerable<ICardModel> ToObservableCards(this IEnumerable<ICardModel> cards)
        {
            var observableCards = cards.Select(x =>
            {
                if (x.Type == CardType.Launcher)
                {
                    return new ObservableLauncherCard(x);
                }
                else
                {
                    return new ObservableCard(x);
                }
            });

            return observableCards;
        }
    }
}
