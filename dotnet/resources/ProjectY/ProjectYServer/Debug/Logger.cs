using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectYServer.Debug
{
    public class Logger : Script
    {
        public static Dictionary<string, int> logs = new Dictionary<string, int>();
        public static bool recentlyAdded = false;
        [RemoteEvent("Debug:LogToServerConsole")]
        public static void LogToServerConsole(Player player, string str)
        {
            NAPI.Task.Run(() =>
            {
                if(logs.ContainsKey(str))
                {
                    logs[str] = logs[str] + 1;
                    recentlyAdded = true;
                }
                else
                {
                    logs.Add(str, 1);
                    recentlyAdded = true;
                    NAPI.Task.Run(() =>
                    {
                        recentlyAdded = false;
                    }, 1000);
                    PrintOut(str, logs[str], player);
                }
            }, 0);
        }

        public async static void PrintOut(string msg, int c, Player player)
        {
            while(recentlyAdded)
            {
                await Task.Delay(100);
            }
            NAPI.Task.Run(() => {
                string dbg = logs.Keys.First(x => x == msg);
                int count = logs[dbg];
                NAPI.Util.ConsoleOutput($"[{player.Name}] {dbg} [{count}]");
                logs.Remove(msg);
            }, 0);
            
        }
    }
}
