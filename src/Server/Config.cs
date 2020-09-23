using Onsharp.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed
{
    [Config("Config")]
    public class OnZedConfig
    {
        #region MongoDB
        public string Host { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        #endregion

        #region Zombies

        public int MaxZombieWorld = 500;
        public int MaxZombiePlayer = 20;
        #endregion
    }
}
