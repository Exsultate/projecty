using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Ui;
using Newtonsoft.Json;

namespace ProjectYClient.UI
{
    public class BrowserManager : Events.Script
    {
        public static HtmlWindow Browser;
        public static bool BrowserReady;
        public static string domain = "package://dist/index.html";

        public BrowserManager()
        {
            Events.OnBrowserDomReady += OnBrowserDomReady;
            Events.OnBrowserCreated += OnBrowserCreated;
        }

        public async static void InitializeBrowserAsync(bool showCursor, bool showChat)
        {
            Browser = new HtmlWindow(domain);
            while(!BrowserReady)
            {
                Events.CallRemote("Debug:LogToServerConsole", $"[BrowserInit] Browser not ready");
                await RAGE.Game.Invoker.WaitAsync();
            }
            Cursor.Visible = showCursor;
            Chat.Show(showChat);
        }

        public async static void CallAsync(string func, object[] args = null)
        {
            while(!BrowserReady)
            {
                await RAGE.Game.Invoker.WaitAsync();
            }
            Browser.Call(func, args);
        }

        public async static void Execute(string func)
        {
            while(!BrowserReady)
            {
                Events.CallRemote("Debug:LogToServerConsole", $"[BrowserInit] Browser not ready");
                await RAGE.Game.Invoker.WaitAsync();
            }
            Browser.ExecuteJs(func);
        }

        public static void DestroyBrowser()
        {
            if(Browser != null) Browser.Destroy();
            Cursor.Visible = false;
            Chat.Show(true);
        }

        public static void ToggleMouse()
        {
            Cursor.Visible = !Cursor.Visible;
        }

        public static void ToggleChat(bool toggle)
        {
            Chat.Show(toggle);
        }


        public void OnBrowserDomReady(HtmlWindow window)
        {
            if (window.Id == Browser.Id) BrowserReady = true;
        }

        public void OnBrowserCreated(HtmlWindow htmlWindow)
        {
            htmlWindow.Active = true;
        }

        public static void BlurElements()
        {
            CallAsync("MoneyHud:BlurAllMoneyElements");
        }
    }
}
