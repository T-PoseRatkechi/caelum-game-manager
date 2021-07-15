// using CaelumGameManagerGUI.Models;
using Caliburn.Micro;
using PhosLibrary.Common.MusicData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumGameManagerGUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        private MusicData _currentMusic = new() { songs = new Song[] { new Song { name = "test" } }};
        private string _status = "Waiting...";

        public string Status
        {
            get { return _status; }
            set 
            { 
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }


        public MusicData CurrentMusic
        {
            get => _currentMusic;
            set 
            { 
                _currentMusic = value;
                NotifyOfPropertyChange(() => CurrentMusic);
                NotifyOfPropertyChange(() => Songs);
            }
        }

        public void LoadMusic()
        {
            CurrentMusic = MusicDataParser.Parse($@"{Directory.GetCurrentDirectory()}\current-music-data.json");
            Status = "Loaded Music";
            var builder = new PhosLibrary.Games.MusicP4G();
            builder.Build(CurrentMusic, "./music-build", false);
        }

        public Song[] Songs => CurrentMusic.songs; 
    }
}
