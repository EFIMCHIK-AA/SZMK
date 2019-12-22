using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Path
    {
        private readonly String _ConnectDBPath;
        private readonly String _ConnectApplicationPath;
        private readonly String _ArchivePath;
        private readonly String _RegistryPath;
        private readonly String _LogPath;

        public Path() 
        {
             _ConnectDBPath = $@"Connect\DataBase\connect.conf";
             _ConnectApplicationPath = $@"Connect\Application\connect.conf";
             _ArchivePath = $@"Path\Archive.df";
             _RegistryPath = $@"Path\Registry.df";
             _LogPath = $@"Log";
        }

        public String ConnectDBPath
        {
            get
            {
                return _ConnectDBPath;
            }
        }

        public String LogPath
        {
            get
            {
                return _LogPath;
            }
        }

        public String ConnectApplicationPath
        {
            get
            {
                return _ConnectApplicationPath;
            }
        }

        public String ArchivePath
        {
            get
            {
                return _ArchivePath;
            }
        }

        public String RegistryPath
        {
            get
            {
                return _RegistryPath;
            }
        }
    }
}
