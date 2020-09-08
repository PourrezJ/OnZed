using Onsharp.Events;
using Onsharp.Plugins;
using System;

namespace OnZed
{
    [PluginMeta("OnZed", "OnZed", "1.0", "Djoe45", IsDebug = true)]
    public class GameMode : Plugin
    {
        public override void OnStart()
        {
            Console.WriteLine("Hello World!");


        }

        public override void OnStop()
        {
            Console.WriteLine("OnStop Called");
        }

        [ServerEvent(EventType.ClientConnectionRequest)]
        public void OnConnectionReq(string ip, int port)
        {
            Console.WriteLine("incoming request {IP}:{PORT}", ip, port);
        }
    }
}
