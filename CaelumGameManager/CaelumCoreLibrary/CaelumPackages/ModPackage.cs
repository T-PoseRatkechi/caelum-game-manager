using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.CaelumPackages
{
    public class ModPackage : BasePackage
    {
        /// <summary>
        /// Initializes a new mod package.
        /// </summary>
        /// <param name="path">Path to mod package data.</param>
        public ModPackage(string path) : base(path, PackageType.Mod)
        {
        }
    }
}
