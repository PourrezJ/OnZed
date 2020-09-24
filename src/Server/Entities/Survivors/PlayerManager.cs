using MongoDB.Driver;
using Onsharp.Commands;
using Onsharp.Entities;
using Onsharp.Events;
using OnZed.Database;
using OnZed.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnZed.Entities
{
    
    public class PlayerManager
    {
        #region Fields
        public static List<PlayerData> PlayerDatas { get; private set; }
        #endregion

        #region C4TOR
        public PlayerManager()
        {
            PlayerDatas = new List<PlayerData>();
        }
        #endregion

        #region Methods
        public static void LoadAllPlayerDatabase()
        {
            PlayerDatas = MongoDb.GetCollectionSafe<PlayerData>("players").AsQueryable().ToList<PlayerData>();

            Log.Info("Mise en cache des " + PlayerDatas.Count() + " joueurs");
        }
        #endregion

        #region Server Events
        [ServerEvent(EventType.PlayerJoin)]
        public void OnPlayerJoint(Player player)
        {
            lock (PlayerDatas)
            {
                // Check if exist in database
                if (PlayerDatas.Exists(p=>p.SteamID == player.SteamID64))
                {
                    Survivor client = player as Survivor;

                    client.PlayerData = PlayerDatas.Find(p => p.SteamID == player.SteamID64);
                    Log.Info($"Loading player {player.SteamID64} {player.Name}");

                    UCoords lastPos = client.PlayerData.LastPosition;

                    client.SetSpawnLocation(lastPos.ToVector3(), lastPos.Heading);

                    client.Spawned = true;

                    player.CallRemote("ClientConnected");
                }
                else // New Player
                {
                    Log.Info($"Creating new player for {player.SteamID64} {player.Name}");

                    // Go to Charcreator
                    player.CallRemote("LaunchCharCreator");

                    
                }
            }

            Task.Run(() =>
            {
                TaskManager.Run(() =>
                {
                    var pos = player.GetPosition();

                    Console.WriteLine($"{pos.X} {pos.Y} {pos.Z}");
                });

            });
        }
        #endregion
    }
}
