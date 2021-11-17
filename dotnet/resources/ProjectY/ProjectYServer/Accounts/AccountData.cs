using GTANetworkAPI;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectYServer.Accounts
{
    [BsonIgnoreExtraElements]
    public class AccountData
    {
        public static readonly string AccountDataIndentifier = "AccountData";
        public string HWID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IPAdress { get; set; }
        public int AccountAge { get; set; }
        public int LastCharacter { get; set; }
        public int AccountState { get; set; }

        public AccountData(BsonDocument bsonElements)
        {
            this.HWID = (string)bsonElements.GetValue(0);
            this.Name = (string) bsonElements.GetValue(1);
            this.Email = (string)bsonElements.GetValue(2);
            this.Password = (string)bsonElements.GetValue(3);
        }

        public AccountData(Player player, string password, string email)
        {
            this.HWID = player.Serial;
            this.Name = player.SocialClubName;
            this.Email = email;
            this.Password = password;
            this.IPAdress = player.Address;
            this.AccountAge = 0;
            this.LastCharacter = -1;
            this.AccountState = (int)AccountStates.CLEAN;
        }

        public AccountData() { }

        public enum AccountStates { 
            CLEAN,
            BANNED,
            SUSPENDED,
            TEMPBAN,
        }
    }
}
