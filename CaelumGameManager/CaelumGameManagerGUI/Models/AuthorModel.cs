// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using CaelumCoreLibrary.Common;
    using Caliburn.Micro;

    /// <summary>
    /// Author Model.
    /// </summary>
    public class AuthorModel : PropertyChangedBase
    {
        private Author _author;

        public Author Author
        {
            get { return _author; }
            set
            { 
                _author = value;
                this.NotifyOfPropertyChange(() => this.Author);
            }
        }

        public AuthorModel(Author author)
        {
            this.Author = author;
        }
    }
}
