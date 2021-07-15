﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.CaelumPackages
{
    public class FolderPackage : BasePackage
    {
        public FolderPackage(string path) : base(path, PackageType.Folder)
        {
        }
    }
}
