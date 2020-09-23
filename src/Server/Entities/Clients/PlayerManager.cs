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

namespace OnZed.Entities
{
    /*
    public class PlayerManager : GameMode
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

            Instance.Logger.Info("Mise en cache des " + PlayerDatas.Count() + " joueurs");
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
                    Client client = player as Client;

                    client.PlayerData = PlayerDatas.Find(p => p.SteamID == player.SteamID64);
                    Instance.Logger.Info($"Loading player {player.SteamID64} {player.Name}");

                    UCoords lastPos = client.PlayerData.LastPosition;

                    client.SetSpawnLocation(lastPos.ToVector3(), lastPos.Heading);

                    player.CallRemote("ClientConnected");
                }
                else // New Player
                {
                    Instance.Logger.Info($"Creating new player for {player.SteamID64} {player.Name}");

                    // Go to Charcreator
                    player.CallRemote("LaunchCharCreator");

                    
                }
            }
        }


        public void OnConnectionReq(string ip, int port)
        {
            Instance.Logger.Info("incoming request {IP}:{PORT}", ip, port);
        }

        [ServerEvent(EventType.PlayerServerAuth)]
        public void OnPlayerServerAuth(Player player)
        {

        }

        [ServerEvent(EventType.PlayerSteamAuth)]
        public void OnPlayerSteamAuth(Player player)
        {

        }

        [ServerEvent(EventType.PlayerSpawn)]
        public void OnPlayerSpawn(Player player)
        {

        }
        #endregion
    }*/
}
