using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Ui;
using ProjectYClient.UI;
using RAGE.Elements;
using ProjectYClient.Camera;

namespace ProjectYClient.Account
{
    public class Authentication : Events.Script
    {
        public Authentication()
        {
            //from server
            Events.Add("Authentication:ShowAuthScreen", ShowAuthScreenEvent);
            Events.Add("Authentication:SuccessfulAuth", SuccessfulAuthEvent);
            Events.Add("Authentication:BadLoginPassword", ShowBadLoginPasswordEvent);

            //from vue
            Events.Add("Authentication:TryLogin", TryLoginEvent);
            Events.Add("Authentication:Register", RegisterPlayerEvent);
        }

        public static void ShowAuthScreenEvent(object[] args)
        {
            CameraController.CreateAuthCamera();
            Player.LocalPlayer.FreezePosition(true);
            RAGE.Game.Ui.DisplayRadar(false);
            RAGE.Game.Ui.DisplayHud(false);
            var state = Convert.ToBoolean(args[0]);
            if(state)
            {
                BrowserManager.InitializeBrowserAsync(true, false);
                BrowserManager.CallAsync("Authentication:InitializeLoginScreen", new[] { args[1].ToString() });
            }
            else
            {
                BrowserManager.InitializeBrowserAsync(true, false);
                BrowserManager.CallAsync("Authentication:InitializeRegisterScreen", new[] { args[1].ToString() });
            }
        }


        public static void RegisterPlayerEvent(object[] args)
        {
            Events.CallRemote("Authentication:RegisterPlayerToDB", args[0], args[1]);
        }

        public static void TryLoginEvent(object[] args)
        {
            Events.CallRemote("Authentication:TryLoginToDB", args[0]);
        }

        public static void ShowBadLoginPasswordEvent(object[] args)
        {
            BrowserManager.CallAsync("Authentication:BadPassword");
        }
        public static void SuccessfulAuthEvent(object[] args)
        {
            BrowserManager.ToggleMouse();
            BrowserManager.ToggleChat(true);
            BrowserManager.CallAsync("Authentication:InitHud");
            CameraController.DestroyCustomCamera();
            Player.LocalPlayer.FreezePosition(false);
        }
    }
}
