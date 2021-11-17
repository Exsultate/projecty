using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Ui;
using ProjectYClient.UI;

namespace ProjectYClient.UI
{
    public class KeyManager : Events.Script
    {
        public KeyManager()
        {
            Input.Bind(RAGE.Ui.VirtualKeys.OEM3, false, OnPlayerPressTildeEvent);
        }

        public static void OnPlayerPressTildeEvent()
        {
            Cursor.Visible = !Cursor.Visible;
            if (!Cursor.Visible) BrowserManager.BlurElements();
        }
    }
}
