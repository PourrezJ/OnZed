using Onsharp.Entities;
using OnZed.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnZed.Entities
{
    public static class ZedsManager
    {
        public static void OnTick()
        {
            lock(GameMode.Instance.Server.Players)
            {
                foreach(Player player in GameMode.Instance.Server.Players)
                {
                    Client client = player as Client;

                    if (client == null)
                        continue;

                    lock(client.Zeds)
                    {
                        var ppos = player.GetPosition();

                        if (client.Zeds.Count < GameMode.Config.MaxZombiePlayer)
                        {        
                            Zed zed = GameMode.Instance.Server.CreateNPC(ppos.Around(200), 0) as Zed;

                            client.Zeds.Add(zed);
                        }

                        foreach(var zed in client.Zeds)
                        {
                            var zpos = zed.GetPosition();

                            if (ppos.DistanceTo(zpos) < 1000f)
                            {
                                zed.Follow(player);
                            }
                        }
                    }
                }
            }
        }
    }
}
