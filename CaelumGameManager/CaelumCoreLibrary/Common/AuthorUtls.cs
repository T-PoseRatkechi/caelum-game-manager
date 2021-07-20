// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Common
{
    using System.IO;
    using System.Text.Json;

    /// <summary>
    /// Utility functinos related to <seealso cref="Author"/>.
    /// </summary>
    public class AuthorUtls
    {
        private static readonly string AuthorsDirectory = Path.Join(Directory.GetCurrentDirectory(), "authors");

        /// <summary>
        /// Parses and returns all available in authors in the authors directory.
        /// </summary>
        /// <returns>List of authors.</returns>
        public static Author[] AvailableAuthors()
        {
            var authorFiles = Directory.GetFiles(AuthorsDirectory, "*.author", SearchOption.TopDirectoryOnly);

            Author[] authors = new Author[authorFiles.Length];
            for (int i = 0, total = authorFiles.Length; i < total; i++)
            {
                authors[i] = ParseAuthor(authorFiles[i]);
            }

            return authors;
        }

        /// <summary>
        /// Parses the file given by <paramref name="authorFile"/> as <seealso cref="Author"/> and returns it.
        /// </summary>
        /// <param name="authorFile">Author file to parse.</param>
        /// <returns><paramref name="authorFile"/> parsed as <seealso cref="Author"/>.</returns>
        public static Author ParseAuthor(string authorFile)
        {
            var authorText = File.ReadAllText(authorFile);
            var author = JsonSerializer.Deserialize<Author>(authorText);
            return author;
        }

        /// <summary>
        /// Writes <paramref name="author"/> to file at <see cref="AuthorsDirectory"/> as a <c>.author</c> JSON.
        /// </summary>
        /// <param name="author"><seealso cref="Author"/> to write.</param>
        public static void WriteAuthor(Author author)
        {
            var authorText = JsonSerializer.Serialize(author, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Join(AuthorsDirectory, author.Name), authorText);
        }
    }
}
