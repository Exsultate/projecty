using System;
using GTANetworkAPI;
using ProjectYServer.Database;
using ProjectYServer.Commands;

namespace ProjectYServer
{
    public class MainController : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            //Register commands
            Commandhandler.RegisterCommands();
            //Initialize db
            DbManager dbManager = new DbManager();
        }
    }
}
