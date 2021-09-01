// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Common
{
    using CaelumGameManagerGUI.Resources.Localization;
    using Microsoft.Win32;

    /// <summary>
    /// Functions for opening various file/folder select windows.
    /// </summary>
    public static class SelectWindow
    {
        /// <summary>
        /// Opens a file select window.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="filter">Window filter.</param>
        /// <returns>Path of file selected, if a file was selected. Null otherise.</returns>
        public static string SelectFileWindow(string title, string filter)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Title = title ?? LocalizedStrings.Instance["WindowSelectFileTitle"];
            openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() == true)
            {
                // No file selected.
                if (string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    return null;
                }

                return openFileDialog.FileName;
            }
            else
            {
                // Selection cancelled.
                return null;
            }
        }
    }
}
