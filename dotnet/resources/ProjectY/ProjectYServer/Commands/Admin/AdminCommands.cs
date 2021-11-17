using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

using ProjectYServer.Commands;
using ProjectYServer.ServerOperations;
using static ProjectYServer.Data.Constants;

namespace ProjectYServer.Commands.Admin
{
    public static class AdminCommands
    {
        public static void RegisterCommands()
        {
            NAPI.Command.Register(Type.GetType("ProjectYServer.Commands.Admin.AdminCommands").GetMethod("CreateVehicleCommand"), Commandhandler.GetGreedyCommand("veh", "/veh [name]"));
            NAPI.Command.Register(Type.GetType("ProjectYServer.Commands.Admin.AdminCommands").GetMethod("SetCharacterMoneyCommand"), Commandhandler.GetGreedyCommand("setmoney", "/setmoney [id] [bank/cash] [amount]"));
        }

        [Command]
        public static void CreateVehicleCommand(Player player, string name)
        {
            VehicleHash busModel = NAPI.Util.VehicleNameToModel(name); //Gets the hash of bus model
            NAPI.Vehicle.CreateVehicle(busModel, player.Position, 0, 0, 0); //Creates a bus on the player's position
        }

        [Command]
        public static void SetCharacterMoneyCommand(Player player, string targetId, string type, string amount)
        {
            Player target = NAPI.Pools.GetAllPlayers().Find(x => x.Id == int.Parse(targetId));
            try
            {
                Money.SetPlayerMoney(target, type == "cash" ? 0 : 1, int.Parse(amount));
                player.SendChatMessage($"{msgSuccess} {target.Name} {type} set to {amount}");
                return;
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[SetCharacterMoneyCommand] {ex.Message}");
                player.SendChatMessage($"{msgError} couldn't set {target.Name} {type} to {amount}");
            }
        }
    }
}
