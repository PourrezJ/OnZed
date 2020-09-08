using Onsharp.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities
{
    public class PlayerManager
    {
        [ServerEvent(EventType.ClientConnectionRequest)]
        public void OnConnectionReq(string ip, int port)
        {
            Console.WriteLine("incoming request {IP}:{PORT}", ip, port);
        }
    }
}
