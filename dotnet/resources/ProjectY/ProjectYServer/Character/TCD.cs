using System;
using MongoDB.Bson;
using ProjectYServer.Accounts;
using GTANetworkAPI;
using MongoDB.Bson.Serialization.Attributes;
using static ProjectYServer.Data.Enumerators;
using ProjectYServer.Database;

namespace ProjectYServer.Character
{
    [BsonIgnoreExtraElements]
    public class TCD : Script
    {
        public static readonly string CharacterDataIndentifier = "characterData";
        public int CharacterId { get; set; }
        public string Account { get; set; }
        public int MoneyBank { get; set; }
        public int MoneyCash { get; set; }
        public Vector3 Position { get; set; }

        public TCD(BsonDocument bsonElements)
        {
            this.CharacterId = (int)bsonElements.GetValue(0);
            this.MoneyBank = (int)bsonElements.GetValue(1);
            this.MoneyCash = (int)bsonElements.GetValue(2);
            this.Position = NAPI.Util.FromJson<Vector3>(bsonElements.GetValue(3));
        }

        public TCD(AccountData accountData)
        {
            this.CharacterId = 0;
            this.Account = accountData.Name;
            this.MoneyBank = int.MaxValue;
            this.MoneyCash = int.MaxValue;
        }

        public TCD() { }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void TCD_OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            TCD characterData = player.GetExternalData<TCD>((int)ExternalData.Ingame);
            characterData.Position = player.Position;
            DbOps.UpdateCharacterData(characterData, "OnDisconnect");
        }
    }
}
