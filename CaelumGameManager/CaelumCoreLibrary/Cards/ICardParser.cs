namespace CaelumCoreLibrary.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for parsing InstallableCards.
    /// </summary>
    public interface ICardParser
    {
        /// <summary>
        /// Parses and returns <paramref name="cardFile"/> as an <see cref="ICardModel"/>.
        /// </summary>
        /// <param name="cardFile">Card file to parse.</param>
        /// <returns><paramref name="cardFile"/> parsed as <see cref="ICardModel"/>.</returns>
        /// <typeparam name="T">Card type.</typeparam>
        ICardModel ParseCard<T>(string cardFile)
            where T : ICardModel;
    }
}
