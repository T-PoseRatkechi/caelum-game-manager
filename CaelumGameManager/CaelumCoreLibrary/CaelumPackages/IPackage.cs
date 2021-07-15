using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Packages
{
    /// <summary>
    /// Interface that all package types implement.
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// Name of package.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name(s) of authors that created the package.
        /// </summary>
        public string[] Authors { get; set; }

        /// <summary>
        /// Name of game package is for.
        /// </summary>
        public string Game { get; set; }

        /// <summary>
        /// Version of package.
        /// </summary>
        public string Version { get; set; }
    }
}
