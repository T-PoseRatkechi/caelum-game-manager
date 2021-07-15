using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.CaelumPackages
{
    public abstract class BasePackage : IPackage
    {
        private PackageData data;

        public BasePackage(string path, PackageType type)
        {
            Data = new() { Path = path, Type = type };
        }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string[] Authors { get; set; }

        /// <inheritdoc/>
        public string Game { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public PackageData Data
        {
            get { return data; }
            private init { data = value; }
        }
    }
}
