// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    /// <summary>
    /// Interface for cards that are buildable.
    /// </summary>
    public interface IBuildableCard : IInstallableCard
    {
        /// <summary>
        /// Build card with the given <paramref name="builder"/> at <paramref name="outputDir"/>.
        /// </summary>
        /// <param name="builder"><seealso cref="ICardBuilder"/> to build card with.</param>
        /// <param name="outputDir">Output directory to build of card build.</param>
        void BuildCard(ICardBuilder builder, string outputDir);
    }
}
