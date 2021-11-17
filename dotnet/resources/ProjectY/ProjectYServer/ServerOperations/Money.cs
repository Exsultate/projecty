using GTANetworkAPI;
using ProjectYServer.Character;
using ProjectYServer.Database;
using System;
using static ProjectYServer.Data.Enumerators;

namespace ProjectYServer.ServerOperations
{
    public class Money : Script
    {
        public static void SetPlayerMoney(Player player, int which, int amount)
        {
            try
            {
                TCD characterData = player.GetExternalData<TCD>((int)ExternalData.Ingame);
                if (which == 0)
                {
                    characterData.MoneyCash = amount;
                }
                else
                {
                    characterData.MoneyBank = amount;
                }
                player.TriggerEvent("HUD:UpdateMoney", characterData.MoneyBank, characterData.MoneyCash);
                DbOps.UpdateCharacterData(characterData, which == 0 ? "Cash money" : "Bank money");
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[SetPlayerMoney] {ex.Message}");
            }
            
        }
    }
}
