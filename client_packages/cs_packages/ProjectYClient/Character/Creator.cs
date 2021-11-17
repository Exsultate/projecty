using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Ui;
using RAGE.Elements;
using ProjectYClient.UI;

namespace ProjectYClient.Character
{
    public class Creator : Events.Script
    {
        public static bool isInCreator = false;
        public Creator()
        {
            Events.Add("Creator:show", onCreatorShowEvent);

            Events.Add("client:creator.preview", onCreatorPreviewEvent);
            Events.Add("client:creator.finish", onCreatorFinishEvent);
            Events.Add("client:creator.reload", onCreatorReloadEvent);

            Events.Tick += Tick;
        }

        public void Tick(List<Events.TickNametagData> nametags)
        {
            if (!isInCreator) return;
            if(RAGE.Game.Pad.IsControlPressed(0, (int)RAGE.Game.Control.MoveLeftOnly))
            {
                var heading = Player.LocalPlayer.GetHeading();
                Player.LocalPlayer.SetHeading(heading - 1);
            }

            if (RAGE.Game.Pad.IsControlPressed(0, (int)RAGE.Game.Control.MoveRightOnly))
            {
                var heading = Player.LocalPlayer.GetHeading();
                Player.LocalPlayer.SetHeading(heading + 1);
            }
        }

        public static void onCreatorPreviewEvent(object[] args)
        {
            string type = args[0].ToString();
            Events.CallRemote("Debug:LogToServerConsole", "Type = " + type);
            string[] parse = null;
            switch (type)
            {
                case "hair":
                    {
                        parse = RAGE.Util.Json.Deserialize<string[]>(args[1].ToString());
                        Events.CallRemote("Debug:LogToServerConsole", "changing hair");
                        var hair = int.Parse(parse[0].ToString());
                        if (hair == 23 || hair == 24) return;
                        Player.LocalPlayer.SetComponentVariation(2, hair, 0, 0);
                        Player.LocalPlayer.SetHairColor(int.Parse(parse[1].ToString()), int.Parse(parse[2].ToString()));
                        Player.LocalPlayer.SetHeadOverlayColor(2, 1, int.Parse(parse[3].ToString()), 0);
                        break;
                    }
                case "faceFeatures":
                    {
                        parse = RAGE.Util.Json.Deserialize<string[]>(args[1].ToString());
                        Events.CallRemote("Debug:LogToServerConsole", "changing face features");
                        Events.CallLocal("Creator:CameraSet", 2);
                        Player.LocalPlayer.SetFaceFeature(int.Parse(parse[0].ToString()), float.Parse(parse[1].ToString()));
                        break;
                    }
                case "gender":
                    {
                        var gender = int.Parse(args[1].ToString());
                        Events.CallRemote("Debug:LogToServerConsole", "Koks gender? " + gender);
                        Player.LocalPlayer.Model = gender == 1 ? RAGE.Util.Joaat.Hash("mp_f_freemode_01") : RAGE.Util.Joaat.Hash("mp_m_freemode_01");
                        break;
                    }
                case "beard":
                    {
                        parse = RAGE.Util.Json.Deserialize<string[]>(args[1].ToString());
                        Events.CallRemote("Debug:LogToServerConsole", "beard array = " + parse.ToString().ToString());
                        Events.CallRemote("Debug:LogToServerConsole", "changing beard id = " + int.Parse(parse[0].ToString()) + " overlay = " + int.Parse(parse[1].ToString()));
                        Events.CallLocal("Creator:CameraSet", 2);
                        int beard = int.Parse(parse[0].ToString());
                        Player.LocalPlayer.SetHeadOverlay(1, beard == -1 ? 255 : beard, 1.0f);
                        Player.LocalPlayer.SetHeadOverlayColor(1, 1, int.Parse(parse[1].ToString()), 0);
                        break;
                    }
                case "blendData":
                    {
                        parse = RAGE.Util.Json.Deserialize<string[]>(args[1].ToString());
                        Events.CallLocal("Creator:CameraSet", 1);
                        Player.LocalPlayer.SetHeadBlendData(int.Parse(parse[0].ToString()), int.Parse(parse[1].ToString()), 0,
                                                            int.Parse(parse[2].ToString()), int.Parse(parse[3].ToString()), 0,
                                                            float.Parse(parse[4].ToString()), float.Parse(parse[5].ToString()), 0, false);
                        break;
                    }
                case "clothing":
                    {
                        string[][] clothes = RAGE.Util.Json.Deserialize<string[][]>(args[1].ToString());
                        Events.CallLocal("Creator:CameraSet", 0);
                        Player.LocalPlayer.SetComponentVariation(11, int.Parse(clothes[0][0].ToString()), int.Parse(clothes[0][1].ToString()), 0);
                        Player.LocalPlayer.SetComponentVariation(8, int.Parse(clothes[1][0].ToString()), int.Parse(clothes[1][1].ToString()), 0);
                        Player.LocalPlayer.SetComponentVariation(4, int.Parse(clothes[2][0].ToString()), int.Parse(clothes[2][1].ToString()), 0);
                        Player.LocalPlayer.SetComponentVariation(6, int.Parse(clothes[3][0].ToString()), int.Parse(clothes[3][1].ToString()), 0);
                        break;
                    }
                case "headOverlays":
                    {
                        string[][] overlays = RAGE.Util.Json.Deserialize<string[][]>(args[1].ToString());                    
                        Events.CallLocal("Creator:CameraSet", 2);
                        for(int i = 0; i < overlays.Length; i++)
                        {
                            if (i == 1) continue;
                            int data = int.Parse(overlays[i][0]);
                            int opacity = int.Parse(overlays[i][1]);
                            Player.LocalPlayer.SetHeadOverlay(i, data == -1 ? 255 : data, opacity);
                        }
                        /*
                        int blemishes = int.Parse(overlays[0][0]);
                        int eyebrows = int.Parse(overlays[1][0]);
                        int ageing = int.Parse(overlays[2][0]);
                        int makeup = int.Parse(overlays[3][0]);
                        int blush = int.Parse(overlays[4][0]);
                        int complexion = int.Parse(overlays[5][0]);
                        int sundmg = int.Parse(overlays[6][0]);
                        int lipstick = int.Parse(overlays[7][0]);
                        int moles = int.Parse(overlays[8][0]);
                        
                        Player.LocalPlayer.SetHeadOverlay(2, eyebrows == -1 ? 255 : eyebrows, float.Parse(overlays[1][1]));
                        Player.LocalPlayer.SetHeadOverlay(3, ageing == -1 ? 255 : ageing, float.Parse(overlays[2][1]));
                        Player.LocalPlayer.SetHeadOverlay(4, makeup == -1 ? 255 : makeup, float.Parse(overlays[3][1]));
                        Player.LocalPlayer.SetHeadOverlay(5, blush == -1 ? 255 : blush, float.Parse(overlays[4][1]));
                        Player.LocalPlayer.SetHeadOverlay(6, complexion == -1 ? 255 : complexion, float.Parse(overlays[5][1]));
                        Player.LocalPlayer.SetHeadOverlay(7, sundmg == -1 ? 255 : sundmg, float.Parse(overlays[6][1]));
                        Player.LocalPlayer.SetHeadOverlay(8, lipstick == -1 ? 255 : lipstick, float.Parse(overlays[7][1]));
                        Player.LocalPlayer.SetHeadOverlay(9, moles == -1 ? 255 : moles, float.Parse(overlays[8][1]));
                        */
                        break;
                    }
                case "headOverlaysColors":
                    {
                        string[] makeup = RAGE.Util.Json.Deserialize<string[]>(args[1].ToString());
                        int blush = int.Parse(makeup[4].ToString());
                        int lipstick = int.Parse(makeup[7].ToString());
                        Player.LocalPlayer.SetHeadOverlayColor(5, 1, blush == -1 ? 255 : blush, 0);
                        Player.LocalPlayer.SetHeadOverlayColor(8, 1, lipstick == -1 ? 255 : lipstick, 0);
                        break;
                    }
            }
        }

        public static void onCreatorFinishEvent(object[] args)
        {
            isInCreator = false;
            var character = RAGE.Util.Json.Deserialize<string>(args[0].ToString());
            Camera.CameraController.DestroyCustomCamera();
            BrowserManager.CallAsync("Creator:InitHud");
            Events.CallRemote("CharacterCreator:SaveCharacter", character);
            BrowserManager.ToggleMouse();
            Player.LocalPlayer.FreezePosition(false);
            BrowserManager.ToggleChat(true);
            BrowserManager.CallAsync("Creator:InitHud");
        }

        public static void onCreatorReloadEvent(object[] args)
        {
            Player.LocalPlayer.SetComponentVariation(11, 15, 0, 0);
            Player.LocalPlayer.SetComponentVariation(3, 15, 0, 0);
            Player.LocalPlayer.SetComponentVariation(8, 15, 0, 0);

            var shirt = Player.LocalPlayer.GetNumberOfDrawableVariations(11);
            var bottom = Player.LocalPlayer.GetNumberOfDrawableVariations(4);
            var shoes = Player.LocalPlayer.GetNumberOfDrawableVariations(6);

            BrowserManager.Browser.ExecuteJs($"max('{shirt}','{ bottom}','{ shoes}')");
        }

        public static void onCreatorShowEvent(object[] args)
        {   
            isInCreator = true;
            Camera.CameraController.CreateCreatorCam();
            Player.LocalPlayer.Position = new Vector3(-419.0495f, 1050.091f, 321.6926f + 50.0f);
            Player.LocalPlayer.FreezePosition(true);
            BrowserManager.CallAsync("Authentication:InitCreator");
            Player.LocalPlayer.SetComponentVariation(11, 15, 0, 0);
            Player.LocalPlayer.SetComponentVariation(3, 15, 0, 0);
            Player.LocalPlayer.SetComponentVariation(8, 15, 0, 0);
        }
    }
}
