using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectYServer.Character;
using ProjectYServer.Accounts;

using GTANetworkAPI;

namespace ProjectYServer.Database
{
    public static class DbOps
    {
        public async static Task<TCD> LoadCharacterData(int charId)
        {
            TCD characterData = new TCD();
            try
            {
                IMongoCollection<TCD> collection = DbManager.db.GetCollection<TCD>("characters");
                characterData = await collection.Find(_ => _.CharacterId == charId).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[LoadCharacterData] {ex.Message}");
            }
            return characterData;
        }

        public async static Task<AccountData> LoadAccountData(Player player)
        {
            AccountData accountData = new AccountData();
            try
            {
                IMongoCollection<AccountData> collection = DbManager.db.GetCollection<AccountData>("accounts");
                accountData = await collection.Find(_ => _.Name == player.SocialClubName).FirstOrDefaultAsync();

                if(accountData != null) NAPI.Util.ConsoleOutput($"[LoadAccountData] succesfully loaded account {accountData.Email}");
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[LoadAccountData] {ex.Message}");
            }
            return accountData;
        }

       public async static Task<long> GetCharactersCountFromDB()
       {
            long count = -1;
            try
            {
                var collection = DbManager.db.GetCollection<TCD>("characters");
                count = await collection.CountDocumentsAsync(new BsonDocument());
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[GetCharactersCountFromDB]  {ex.Message}");
            }
            return count;
       }

       public static async void CreateNewAccount(AccountData accountData)
       {
            try
            {
                var collection = DbManager.db.GetCollection<AccountData>("accounts");
                await collection.InsertOneAsync(accountData);
                NAPI.Util.ConsoleOutput($"[CreateNewAccount] succesfully inserted account {accountData.Email}");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[CreateNewAccount]  {ex.Message}");
            }
       }

       public static async void CreateNewCharacter(TCD characterData)
       {
            try
            {
                var collection = DbManager.db.GetCollection<TCD>("characters");
                await collection.InsertOneAsync(characterData);
                NAPI.Util.ConsoleOutput($"[CreateNewAccount] succesfully inserted character {characterData.CharacterId}");
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[CreateNewCharacter]  {ex.Message}");
            }
       }

       public static async void UpdateCharacterData(TCD characterData, string what)
       {
            try
            {
                var collection = DbManager.db.GetCollection<TCD>("characters");
                await collection.ReplaceOneAsync(x => x.CharacterId == characterData.CharacterId, characterData);
                NAPI.Util.ConsoleOutput($"[UpdateCharacterData] succesfully updated character data {what}");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[UpdateCharacterData] {ex.Message}");
            }
       }
    }
}
