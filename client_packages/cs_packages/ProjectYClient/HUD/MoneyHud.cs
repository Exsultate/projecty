using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using ProjectYClient.UI;

namespace ProjectYClient.HUD
{
    public class MoneyHud : Events.Script
    {
        public MoneyHud()
        {
            //from server
            Events.Add("HUD:DisplayMoney", HudDisplayMoneyEvent);
            Events.Add("HUD:UpdateMoney", HudUpdateMoneyEvent);
        }

        public static void HudDisplayMoneyEvent(object[] args)
        {
            BrowserManager.CallAsync("HUD:DisplayMoney", new[] { args[0], args[1] });
        }

        public static void HudUpdateMoneyEvent(object[] args)
        {
            BrowserManager.CallAsync("HUD:UpdateMoney", new[] { args[0], args[1] });
        }
    }
}
