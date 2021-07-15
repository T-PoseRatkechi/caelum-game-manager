using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaelumCoreLibrary.CaelumPackages;

namespace CaelumGameManagerGUI.Models
{
    public class PackageModel
    {
        private IPackage package;
        public PackageModel(IPackage package)
        {
            this.package = package;
        }

        public string Name => this.package.Name;
        public string Authors => string.Join(", ", package.Authors);
        public string Game => this.package.Game;
        public string PackagePath => this.package.Data.Path;
        public string PackageType => this.package.Data.Type.ToString();
    }
}
