using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace ProjectYServer.Commands.Util
{
    public static class UtilCommands
    {
        public static void RegisterCommands()
        {
            NAPI.Command.Register(Type.GetType("ProjectYServer.Commands.Util.UtilCommands").GetMethod("CoordsCommand"), new RuntimeCommandInfo("coords", "Get your current position"));
        }

        [Command]
        public static void CoordsCommand(Player player)
        {
            NAPI.Util.ConsoleOutput($"{player.SocialClubName} {player.Position}");
        }
    }
}
