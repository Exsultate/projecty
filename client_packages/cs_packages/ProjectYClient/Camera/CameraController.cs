using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;

namespace ProjectYClient.Camera
{
    public class CameraController : Events.Script
    {
        public static int cam;
        public static Vector3 bodyCamStart = null;
        public CameraController()
        {
            Events.Add("Creator:CameraSet", onCameraSetEvent);
        }

        public static void onCameraSetEvent(object[] args)
        {
            var Angle = 0;
            var Dist = 1f;
            var Height = 0.2f;
            switch (int.Parse(args[0].ToString()))
            {
                case 0: // Torso
                    {
                        Events.CallRemote("Debug:LogToServerConsole", "torso");
                        Angle = 0; Dist = 2.6f; Height = 0.2f;
                        break;
                    }
                case 1: // Head
                    {
                        Events.CallRemote("Debug:LogToServerConsole", "head");
                        Angle = 0; Dist = 0.8f; Height = 0.8f;
                        break;
                    }
                case 2: // Hair / Bear / Eyebrows
                    {
                        Events.CallRemote("Debug:LogToServerConsole", "hair brows beard");
                        Angle = 0; Dist = 0.5f; Height = 0.8f;
                        break;
                    }
                case 3: // chesthair
                    {
                        Events.CallRemote("Debug:LogToServerConsole", "chest");
                        Angle = 0; Dist = 2.6f; Height = 0.2f;
                        break;
                    }
            }
            bodyCamStart = Player.LocalPlayer.Position;
            var camPos = GetCameraOffset(new Vector3(bodyCamStart.X, bodyCamStart.Y, bodyCamStart.Z + Height), Player.LocalPlayer.GetRotation(2).Z + 90 +  Angle, Dist);
            RAGE.Game.Cam.SetCamCoord(cam, camPos.X, camPos.Y, camPos.Z);
            RAGE.Game.Cam.PointCamAtCoord(cam, bodyCamStart.X, bodyCamStart.Y, bodyCamStart.Z + Height);
        }

        public static void CreateAuthCamera()
        {
            cam = RAGE.Game.Cam.CreateCamera(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), true);
            RAGE.Game.Cam.SetCamCoord(cam, -419.0495f, 1050.091f, 321.6926f + 50.0f);
            RAGE.Game.Cam.PointCamAtCoord(cam, -145.48727f, -586.4273f, 211.77533f);

            // Enable the camera
            RAGE.Game.Cam.SetCamActive(cam, true);
            RAGE.Game.Cam.RenderScriptCams(true, false, 0, true, false, 0);

            ToggleHud(false);

            RAGE.Game.Streaming.RequestAnimDict("amb@world_human_statue@idle_a");//preload the animation
            Player.LocalPlayer.TaskPlayAnim("amb@world_human_statue@idle_a", "idle_b", 8.0f, 1.0f, -1, 1, 1.0f, false, false, false);
        }

        public static void CreateCreatorCam()
        {
            cam = RAGE.Game.Cam.CreateCamera(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), true);

            var bodyCamStart = Player.LocalPlayer.Position;
            var angle = Player.LocalPlayer.GetRotation(2).Z + 90;
            var dist = 2.6f;
            var height = 0.2f;
            var pos = GetCameraOffset(new Vector3(bodyCamStart.X, bodyCamStart.Y, bodyCamStart.Z + height), angle, dist);
            RAGE.Game.Cam.SetCamCoord(cam, pos.X, pos.Y, pos.Z);
            RAGE.Game.Cam.PointCamAtCoord(cam, bodyCamStart.X, bodyCamStart.X, bodyCamStart.X + height);
            RAGE.Game.Cam.SetCamActive(cam, true);
            RAGE.Game.Cam.RenderScriptCams(true, false, 0, true, false, 0);
        }

        public static void ToggleHud(bool toggle)
        {
            RAGE.Game.Ui.DisplayHud(toggle);
            RAGE.Game.Ui.DisplayRadar(toggle);
        }

        public static void DestroyCustomCamera()
        {
            ToggleHud(true);
            RAGE.Game.Cam.DestroyCam(cam, true);
            RAGE.Game.Cam.RenderScriptCams(false, false, 0, true, false, 0);
        }

        public static Vector3 GetCameraOffset(Vector3 pos, float angle, double dist)
        {
            angle *= 0.0174533f;
            pos.Y = (float)(pos.Y + dist * Math.Sin(angle));
            pos.X = (float)(pos.X + dist * Math.Sin(angle));
            return pos;
        }
    }
}
