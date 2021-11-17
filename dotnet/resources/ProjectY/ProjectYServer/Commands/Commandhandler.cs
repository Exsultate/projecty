using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

using ProjectYServer.Commands.Util;
using ProjectYServer.Commands.Admin;

namespace ProjectYServer.Commands
{
    public class Commandhandler
    {
        public static void RegisterCommands()
        {
            UtilCommands.RegisterCommands();
            AdminCommands.RegisterCommands();
        }

        public static RuntimeCommandInfo GetGreedyCommand(string name, string help)
        {
            // Add the GreedyArg argument to the command
            return new RuntimeCommandInfo(name, help) { GreedyArg = true };
        }
    }
}
