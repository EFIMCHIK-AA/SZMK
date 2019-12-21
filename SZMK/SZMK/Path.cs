using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZMK
{
    public class Path
    {
        private readonly String _ConnectDBPath = $@"Connect\DataBase\connect.conf";
        private readonly String _ConnectApplicationPath = $@"Connect\Application\connect.conf";
        private readonly String _ArchivePath = $@"Path\Archive.df";
        private readonly String _RegistryPath = $@"Path\Registry.df";
        private readonly String _LogPath = $@"Log";

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
