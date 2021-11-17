using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using GTANetworkAPI;
using ProjectYServer.Database;
using static ProjectYServer.Data.Enumerators;
using ProjectYServer.Character;

namespace ProjectYServer.Accounts
{
    public class Authentication : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        public async void OnPlayerConnect(Player player)
        {
            try
            {
                player.Position = new Vector3(-419.0495f, 1050.091f, 321.6926f + 50.0f);
                NAPI.Util.ConsoleOutput($"Player {player.SocialClubName} connected {player.Address}");
                AccountData accountData = await DbOps.LoadAccountData(player);
                await NAPI.Task.WaitForMainThread();
                player.Transparency = 0;
                if (accountData == null)
                {
                    player.TriggerEvent("Authentication:ShowAuthScreen", false, player.SocialClubName);
                }
                else
                {
                    switch (accountData.AccountState)
                    {
                        case (int)AccountData.AccountStates.CLEAN:
                            {
                                player.SetExternalData((int)ExternalData.Database, accountData);
                                NAPI.Util.ConsoleOutput($"Account {accountData.Name} clean");
                                player.TriggerEvent("Authentication:ShowAuthScreen", true, accountData.Name);
                                break;
                            }
                        case (int)AccountData.AccountStates.BANNED:
                            {
                                //Show banned page
                                break;
                            }
                        case (int)AccountData.AccountStates.SUSPENDED:
                            {
                                //Show suspended page
                                break;
                            }
                        case (int)AccountData.AccountStates.TEMPBAN:
                            {
                                //Show tempban page
                                break;
                            }
                    }
                }
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput("[Authentication:OnPlayerConnect]" + ex.ToString());
            }      
        }

        [RemoteEvent("Authentication:RegisterPlayerToDB")]
        public async static void RegisterPlayerToDb(Player player, string password, string email)
        {
            AccountData accountData = new AccountData(player, password, email);
            try
            {
                TCD characterData = new TCD(accountData) { CharacterId = (int)await DbOps.GetCharactersCountFromDB() };
                player.SetExternalData((int)ExternalData.Ingame, characterData);
                accountData.LastCharacter = characterData.CharacterId;
                DbOps.CreateNewAccount(accountData);
                DbOps.CreateNewCharacter(characterData);
                await NAPI.Task.WaitForMainThread();

                player.TriggerEvent("Creator:show");
                player.Transparency = 255;
                /*

                player.TriggerEvent("HUD:DisplayMoney", characterData.MoneyBank, characterData.MoneyCash);

                NAPI.Util.ConsoleOutput($"[RegisterPlayerToDb] Succesfully registered player to DB {accountData.Name} {accountData.Email}");
                
                player.TriggerEvent("Authentication:SuccessfulAuth");
                player.SendChatMessage($"Welcome to ProjectY {accountData.Name}");
                
                player.Transparency = 255;

                */
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[RegisterPlayerToDb] Couldn't register player to DB {accountData.Name} {accountData.Email}");
                NAPI.Util.ConsoleOutput($"[RegisterPlayerToDb] {ex.Message}");
            }
        }

        [RemoteEvent("Authentication:TryLoginToDB")]
        public async static void TryLoginToDB(Player player, string password)
        {
            AccountData accountData = player.GetExternalData<AccountData>((int)ExternalData.Database);
            NAPI.Util.ConsoleOutput($"Password used = {password} db pass = {accountData.Password}");
            try
            {
                if (password == accountData.Password)
                {
                    player.TriggerEvent("Authentication:SuccessfulAuth");
                    player.SendChatMessage($"Welcome back to ProjectY {accountData.Name}");

                    TCD characterData = await DbOps.LoadCharacterData(accountData.LastCharacter);
                    await NAPI.Task.WaitForMainThread();
                    player.SetExternalData((int)ExternalData.Ingame, characterData);

                    player.TriggerEvent("HUD:DisplayMoney", characterData.MoneyBank, characterData.MoneyCash);
                    player.Transparency = 255;
                }
                else
                {
                    player.TriggerEvent("Authentication:BadLoginPassword");
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[TryLoginToDB] Couldn't login player to DB {accountData.Name} {accountData.Email}");
                NAPI.Util.ConsoleOutput($"[TryLoginToDB] {ex.Message}");
            }
        }
    }
}
