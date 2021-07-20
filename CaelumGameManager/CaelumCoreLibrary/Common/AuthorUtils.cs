// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Common
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.Json;

    /// <summary>
    /// Utility functinos related to <seealso cref="Author"/>.
    /// </summary>
    public class AuthorUtils
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
        /// Parses and returns all available in authors in <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">Directory to check for author files.</param>
        /// <returns>List of authors in <paramref name="directory"/>.</returns>
        public static Author[] GetAllAuthors(string directory)
        {
            var authorFiles = Directory.GetFiles(directory, "*.author", SearchOption.TopDirectoryOnly);

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
            using (BinaryReader reader = new(new FileStream(authorFile, FileMode.Open)))
            {
                uint header = reader.ReadUInt32();
                if (header != 0x41425553U)
                {
                    throw new ArgumentException("Author file invalid!");
                }

                uint size = reader.ReadUInt32();
                uint authorSize = reader.ReadUInt32();
                byte[] authorBytes = reader.ReadBytes((int)authorSize);
                uint imageSize = reader.ReadUInt32();
                byte[] imageBytes = reader.ReadBytes((int)imageSize);

                Author author = JsonSerializer.Deserialize<Author>(authorBytes);
                author.AvatarBytes = imageBytes;
                return author;
            }
        }

        /// <summary>
        /// Writes <paramref name="author"/> to file at <see cref="AuthorsDirectory"/> as a <c>.author</c> JSON.
        /// </summary>
        /// <param name="author"><seealso cref="Author"/> to write.</param>
        /// <param name="output">Output file of author, if null then defaults to app authors folder.</param>
        public static void WriteAuthor(Author author, string output = null)
        {
            var authorFilePath = output == null ? Path.Join(AuthorsDirectory, $"{author.Name.GetHashCode()}.author") : output;

            // Create parent directory if missing.
            if (!Directory.Exists(Path.GetDirectoryName(authorFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(authorFilePath));
            }

            var authorText = JsonSerializer.Serialize(author);
            var avatarBytes = author.AvatarBytes;

            using (BinaryWriter writer = new(new FileStream(authorFilePath, FileMode.Create)))
            {
                writer.Write(0x41425553U);

                var authorTextBytes = Encoding.UTF8.GetBytes(authorText);

                writer.Write((uint)(authorTextBytes.Length + avatarBytes.Length + 8));

                writer.Write((uint)authorTextBytes.Length);
                writer.Write(authorTextBytes);

                writer.Write((uint)avatarBytes.Length);
                writer.Write(avatarBytes);
            }
        }
    }
}
