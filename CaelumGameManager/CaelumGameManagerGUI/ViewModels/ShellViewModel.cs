// using CaelumGameManagerGUI.Models;
using CaelumCoreLibrary.CaelumPackages;
using CaelumGameManagerGUI.Models;
using Caliburn.Micro;
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
        private BindableCollection<PackageModel> packages = new();

        public ShellViewModel()
        {

        }

        public BindableCollection<PackageModel> Packages
        {
            get { return packages; }
            set { packages = value; }
        }
    }
}
