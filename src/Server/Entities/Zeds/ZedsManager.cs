using Onsharp.Entities;
using Onsharp.Events;
using OnZed.Utils;
using OnZed.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnZed.Entities
{
    public class ZedsManager
    {
        public ZedsManager()
        {

        }

        [ServerEvent(EventType.NPCDeath)]
        public void OnNpcDeath(NPC npc)
        {
            if (npc is Zed)
            {
                Zed zed = npc as Zed;

                if (zed.SurvivorFollowed != null)
                {
                    Survivor survivor = zed.SurvivorFollowed;

                    if (survivor.Zeds.Contains(zed))
                    {
                        survivor.Zeds.Remove(zed);
                        zed.SetRagdoll(true);
                        //zed.Destroy();
                    }
                }
            }
        }

        private static DateTime nextLoop = new DateTime();

        [ServerEvent(EventType.GameTick)]
        public void OnTick(double delta)
        {
            if (nextLoop > DateTime.Now)
                return;

            nextLoop = DateTime.Now.AddMilliseconds(15000);

            lock (GameMode.Instance.Server.Players)
            {
                foreach (Player player in GameMode.Instance.Server.Players)
                {
                    Survivor client = player as Survivor;

                    if (client == null)
                        continue;

                    if (!client.Spawned)
                        continue;

                    var ppos = player.GetPosition();

                    if (client.Zeds.Count < GameMode.Config.MaxZombiePlayer)
                    {
                        Zed zed = GameMode.Instance.Server.CreateNPC(ppos.Around(1000), 0) as Zed;
                        client.Zeds.Add(zed);
                    }

                    lock (client.Zeds)
                    {
                        foreach (var zed in client.Zeds.ToList()) // Création d'une liste temporaire pour check les zeds à supprimer
                        {
                            var zpos = zed.GetPosition();

                            if (zed.SurvivorFollowed == null && ppos.DistanceTo(zpos) < 1000f)
                            {
                                zed.SurvivorFollowed = client;
                            }

                            else if (!zed.IsStreamedFor(player))
                            {
                                // Recherche d'un autre joueur ou despawn.

                                //zed.Destroy();
                                //client.Zeds.Remove(zed);
                            }
                        }
                    }
                }
            }
        }
    }
}
