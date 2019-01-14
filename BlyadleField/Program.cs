using BlyadleField.CommandSystem;
using BlyadleField.ConsoleSystem;
using BlyadleField.Game.Models;
using BlyadleField.Game.Structs;
using BlyadleField.MemorySystem;
using BlyadleField.Overlay;
using BlyadleField.ThreadingSystem;
using SharpDX;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlyadleField
{
    internal class Program
    {
        private static MemoryScanner Memory => BlyadleField.Memory;
        private static OverlayWindow Overlay => BlyadleField.Overlay;

        private static Matrix ViewMatrix;
        private static Matrix ViewMatrixInverse;

        private static int brushWhite;
        public static int brushBlack;
        private static int enemyBrush;
        private static int enemyVisibleBrush;

        private static List<Player> Players = new List<Player>();
        private static Player LocalPlayer = null;
        private static byte[] six_nops = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

        const short SWP_NOMOVE = 0x2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0x4;
        const int SWP_SHOWWINDOW = 0x0040;
        const short HWND_NOTOPMOST = -2;
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern System.IntPtr SetWindowPos(System.IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        private static void Collector()
        {
            while(BlyadleField.IsAttached)
            {
                Thread.Sleep(1000);
                System.GC.Collect();
            }
        }


        private static void Main(string[] args)
        {
            CommandHandler.Setup();
            ThreadManager.Add(new ThreadFunction("CommandHandler", CommandHandler.Worker));
            ThreadManager.Add(new ThreadFunction("DrawingLoop", DrawingLoop));
            //ThreadManager.Add(new ThreadFunction("Collector", Collector));
            AttachToGame();
        }

        private static void AttachToGame()
        {
            Console.WriteNotification($"\n  Looking for {BlyadleField.GameName}...");
            while (!BlyadleField.IsAttached)
            {
                Thread.Sleep(100);
                try
                {
                    BlyadleField.Process = Process.GetProcessesByName(BlyadleField.ProcessName).FirstOrDefault(p => p.Threads.Count > 0);
                    if (BlyadleField.Process == null || BlyadleField.Process.MainWindowHandle == System.IntPtr.Zero) continue;
                }
                catch
                {
                    continue;
                }

                BlyadleField.Memory = new MemoryScanner(BlyadleField.Process);
                BlyadleField.IsAttached = true;
            }

            byte[] haltFunction = {
                0xC3, // ret
                0x90, // nop
                0x90, // nop
                0x90, // nop
                0x90, // nop
                0x90, // nop
            };

            // NOP screenshot functions
            Memory.WriteMemory(0x145679410, haltFunction);
            Memory.WriteMemory(0x1456e85a0, haltFunction);

            // HWID
            //Memory.WriteMemory(0x1453879F0, haltFunction);

            // SendSuspiciousKeyMessage
            //Memory.WriteMemory(0x145467E60, haltFunction);

            Console.WriteSuccess("  Found and attached to it!");
            CommandHandler.Load();
            Console.WriteCommandLine();
            ThreadManager.Run("CommandHandler");
            ThreadManager.Run("DrawingLoop");
            SetWindowPos(BlyadleField.Process.MainWindowHandle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            //ThreadManager.Run("Collector");
        }
        

        private static void DrawingLoop()
        {
            BlyadleField.Overlay = new OverlayWindow(BlyadleField.Process.MainWindowHandle, false);
            
            Overlay.Show();

            var greenColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Green);
            var blackColor = System.Drawing.Color.Black;
            

            while (BlyadleField.IsAttached)
            {
                Thread.Sleep(0);
                if (BlyadleField.Process.HasExited)
                {
                    BlyadleField.IsAttached = false;
                    System.Environment.Exit(0);
                }

                // Begin scene of direct2d device to initialize drawing
                Overlay.Graphics.BeginScene();
                Overlay.Graphics.ClearScene();
                // Clear the scene so everything what we have drawn is getting deleted
                if (Native.GetForegroundWindow() != Overlay.ParentWindow)
                {
                    Overlay.Graphics.EndScene();
                    continue;
                }
                var pointCrosshair = new Vector2(Overlay.Width / 2, Overlay.Height / 2);
                brushBlack = Overlay.Graphics.CreateBrush(blackColor);
                brushWhite = Overlay.Graphics.CreateBrush(System.Drawing.Color.White);
                enemyBrush = Overlay.Graphics.CreateBrush(System.Drawing.Color.Red);
                enemyVisibleBrush = Overlay.Graphics.CreateBrush(System.Drawing.Color.FromArgb(220, 255, 30, 0));

                var mainFont = Overlay.Graphics.CreateFont("Tahoma", 17);
                var espFont = Overlay.Graphics.CreateFont("Tahoma", 13);
                // Draw text (kek)
                Overlay.Graphics.DrawText(Console.Watermark, mainFont, brushWhite, 10, 10, false);

                if(CommandHandler.GetParameter("crosshair", "active").Value.ToBool())
                    Overlay.Graphics.DrawPlus((int)pointCrosshair.X, (int)pointCrosshair.Y, 10, 2, brushWhite);

                Update(espFont);

                Overlay.Graphics.EndScene();
                Overlay.Graphics.DeleteBrushContainer();
                Overlay.Graphics.DeleteFontContainer();
                Overlay.Graphics.DeleteLayoutContainer();
            }
        }

        private static void Update(int font)
        {
            LocalPlayer = new Player();
            var gameContext = Memory.Read<long>(Offsets.ClientGameContext.GetInstance());
            if (!Memory.IsValid(gameContext)) return;
            var playerManager = Memory.Read<long>(gameContext + Offsets.ClientGameContext.m_pPlayerManager);
            if (!Memory.IsValid(playerManager)) return;
            var lp = Memory.Read<long>(playerManager + Offsets.ClientPlayerManager.m_pLocalPlayer);
            if (!Memory.IsValid(lp)) return;
            var clientSoldierEntity = GetClientSoldierEntity(lp, LocalPlayer);
            if (!Memory.IsValid(clientSoldierEntity)) return;
            var healthComponent = Memory.Read<long>(clientSoldierEntity + Offsets.ClientSoldierEntity.m_pHealthComponent);
            if (!Memory.IsValid(healthComponent)) return;
            var predictedController = Memory.Read<long>(clientSoldierEntity + Offsets.ClientSoldierEntity.m_pPredictedController);
            if (!Memory.IsValid(predictedController)) return;
            LocalPlayer.Health = Memory.Read<float>(healthComponent + Offsets.HealthComponent.m_Health);
            LocalPlayer.MaxHealth = Memory.Read<float>(healthComponent + Offsets.HealthComponent.m_MaxHealth);
            if (LocalPlayer.Health < 0.1f) return;

            if (CommandHandler.GetParameter("bullet", "active").Value.ToBool())
            {
                var pt6 = Memory.Read<long>(clientSoldierEntity + 0x670); //ClientSoldierWeapon
                if (!Memory.IsValid(pt6)) goto cont0;
                var pt7 = Memory.Read<long>(pt6 + 0x30); //SoldierWeaponData
                if (!Memory.IsValid(pt7)) goto cont0;
                var pt8 = Memory.Read<long>(pt7 + 0x90); //WeaponFiringData
                if (!Memory.IsValid(pt8)) goto cont0;
                var pt9 = Memory.Read<long>(pt8 + 0x10); //FiringFunctionData
                if (!Memory.IsValid(pt9)) goto cont0;

                Memory.Write(pt9 + 0xE4, CommandHandler.GetParameter("bullet", "damage").Value.ToInt32());
                Memory.Write(pt9 + 0x88, CommandHandler.GetParameter("bullet", "speed").Value.ToFloat());
                var gravity = Memory.Read<long>(pt9 + 0xC0); //BulletEntityData
                if (!Memory.IsValid(gravity)) goto cont0;
                Memory.Write(gravity + 0x140, CommandHandler.GetParameter("bullet", "gravity").Value.ToFloat());
            }
            cont0:
            if(CommandHandler.GetParameter("weapon", "norecoil").Value.ToBool())
            {
                var clientSoldierWeapon = Memory.Read<long>(clientSoldierEntity + 0x670);
                if (!Memory.IsValid(clientSoldierWeapon)) goto cont;
                var clientWeapon = Memory.Read<long>(clientSoldierWeapon + 0x4A18);
                if (!Memory.IsValid(clientWeapon)) goto cont;
                var weaponFiringData = Memory.Read<long>(clientWeapon + 0x18);
                if (!Memory.IsValid(weaponFiringData)) goto cont;
                var gunSwayData = Memory.Read<long>(weaponFiringData + 0x30 + 0x8);
                if (!Memory.IsValid(gunSwayData)) goto cont;
                Memory.Write(gunSwayData + 0x03C8, 100.0f);
                Memory.Write(gunSwayData + 0x03D4, 0.0f);
            }

            cont:
            var gameRenderer = Memory.Read<long>(Offsets.GameRenderer.GetInstance());
            var renderView = Memory.Read<long>(gameRenderer + Offsets.GameRenderer.m_pRenderView);
            LocalPlayer.Fov.X = Memory.Read<float>(renderView + 0x250);
            LocalPlayer.Fov.Y = Memory.Read<float>(renderView + 0xB4);
            LocalPlayer.Origin = Memory.Read<Vector3>(predictedController + Offsets.ClientSoldierPrediction.m_Position);
            LocalPlayer.Team = Memory.Read<int>(lp + Offsets.ClientPlayer.m_teamId);
            LocalPlayer.Pose = Memory.Read<int>(clientSoldierEntity + Offsets.ClientSoldierEntity.m_poseType);
            LocalPlayer.Yaw = Memory.Read<float>(clientSoldierEntity + Offsets.ClientSoldierEntity.m_authorativeYaw);
            LocalPlayer.IsOccluded = Memory.Read<byte>(clientSoldierEntity + Offsets.ClientSoldierEntity.m_occluded);
            ViewMatrix = Memory.Read<Matrix>(renderView + Offsets.RenderView.m_ViewProj);
            ViewMatrixInverse = Memory.Read<Matrix>(renderView + Offsets.RenderView.m_ViewMatrixInverse);
            long pPlayers = Memory.Read<long>(playerManager + Offsets.ClientPlayerManager.m_ppPlayer);
            if (!Memory.IsValid(pPlayers)) return;
            long currentPlayer;
            Player player;
            long soldierEntity;
            long healthComponent1;
            long predictedController1;

            int color;
            if (!CommandHandler.GetParameter("esp", "active").Value.ToBool()) return;
            for (int i = 0; i < 70; i++)
            {
                player = new Player();
                currentPlayer = Memory.Read<long>(pPlayers + (i * 8));
                if (!Memory.IsValid(currentPlayer)) continue;
                player.IsSpectator = System.Convert.ToBoolean(Memory.Read<byte>(currentPlayer + Offsets.ClientPlayer.m_isSpectator));
                player.Name = Memory.ReadString(currentPlayer + Offsets.ClientPlayer.szName);
                soldierEntity = GetClientSoldierEntity(currentPlayer, player);
                if (!Memory.IsValid(soldierEntity)) continue;
                healthComponent1 = Memory.Read<long>(soldierEntity + Offsets.ClientSoldierEntity.m_pHealthComponent);
                if (!Memory.IsValid(healthComponent1)) continue;
                predictedController1 = Memory.Read<long>(soldierEntity + Offsets.ClientSoldierEntity.m_pPredictedController);
                if (!Memory.IsValid(predictedController1)) continue;
                player.Health = Memory.Read<float>(healthComponent1 + Offsets.HealthComponent.m_Health);
                player.MaxHealth = Memory.Read<float>(healthComponent1 + Offsets.HealthComponent.m_MaxHealth);
                if (player.Health < 0.1f) continue;
                player.Origin = Memory.Read<Vector3>(predictedController1 + Offsets.ClientSoldierPrediction.m_Position);
                player.Team = Memory.Read<int>(currentPlayer + Offsets.ClientPlayer.m_teamId);
                player.Pose = Memory.Read<int>(soldierEntity + Offsets.ClientSoldierEntity.m_poseType);
                player.Yaw = Memory.Read<float>(soldierEntity + Offsets.ClientSoldierEntity.m_authorativeYaw);
                player.IsOccluded = Memory.Read<byte>(soldierEntity + Offsets.ClientSoldierEntity.m_occluded);
                player.Distance = Vector3.Distance(LocalPlayer.Origin, player.Origin);
                if (!player.IsValid()) continue;
                if (player.Team == LocalPlayer.Team) continue;
                if(player.InVehicle) player.Origin = new Vector3(player.VehicleTranfsorm.M41, player.VehicleTranfsorm.M42, player.VehicleTranfsorm.M43);

                Vector3 w2sFoot, w2sHead;
                int offset = 0;

                if (CommandHandler.GetParameter("esp", "glow").Value.ToBool())
                {
                    if (!player.InVehicle)
                        if (player.IsVisible())
                            Memory.Write<byte>(soldierEntity + 0x2FC, 240);
                        else
                            Memory.Write<byte>(soldierEntity + 0x2FC, 242);
                    else
                        if (player.IsVisible() && player.IsDriver)
                            Memory.Write<byte>(soldierEntity + 0x4FD, 240);
                        else
                            Memory.Write<byte>(soldierEntity + 0x4FD, 242);

                    //var renderFlag = Memory.Read<long>(soldierEntity + 0x18);
                    //renderFlag &= 0xFFF7FFFF;
                    //Memory.Write(soldierEntity + 0x18, renderFlag);
                }

                if (WorldToScreen(player.Origin, out w2sFoot) &&
                    WorldToScreen(player.Origin, player.Pose, out w2sHead))
                {
                    float y = (w2sFoot.Y - w2sHead.Y) / 2f;
                    float x = w2sHead.X - y / 2f;
                    
                    color = (player.IsVisible() ? enemyVisibleBrush : enemyBrush);
                    if (CommandHandler.GetParameter("esp", "box").Value.ToBool() && !player.InVehicle)
                    {
                        Vector3 vector32 = w2sFoot;
                        Vector3 vector33 = w2sHead;
                        float single = w2sFoot.Y - vector33.Y;
                        float single1 = single / 2f;
                        x = vector33.X - single1 / 2f;
                        Overlay.Graphics.DrawRect((int)x, (int)vector33.Y, (int)single1, (int)single, color);
                    }
                    if (CommandHandler.GetParameter("esp", "distance").Value.ToBool() && !player.InVehicle)
                    {
                        Overlay.Graphics.DrawText(string.Concat((int)player.Distance, "m"), font, brushWhite, (int)x, (int)w2sFoot.Y, false);
                        offset += 13;
                    }
                    if (CommandHandler.GetParameter("esp", "name").Value.ToBool() && !player.InVehicle)
                    {
                        Overlay.Graphics.DrawText(player.Name, font, brushWhite, (int)x, (int)w2sFoot.Y + offset, false);
                        offset += 13;
                    }

                    if (CommandHandler.GetParameter("esp", "vehicle").Value.ToBool())
                    {
                        if (player.InVehicle && player.IsDriver)
                            Overlay.Graphics.DrawText(player.VehicleName, font, brushWhite, (int)x, (int)w2sFoot.Y + offset, false);

                        if (player.InVehicle)
                            Overlay.Graphics.DrawAABB(player.VehicleAABB, player.VehicleTranfsorm, SharpDX.Color.White);
                    }

                    if (CommandHandler.GetParameter("esp", "health").Value.ToBool())
                    {
                        if(player.InVehicle && player.IsDriver)
                            Overlay.Graphics.DrawHealth((int)x, (int)w2sHead.Y - 10, (int)y, 3, (int)player.VehicleHealth, (int)player.VehicleMaxHealth);
                        else
                            if(!player.InVehicle)
                                Overlay.Graphics.DrawHealth((int)x, (int)w2sHead.Y - 6, (int)y, 3, (int)player.Health, (int)player.MaxHealth);
                    }
                    if(CommandHandler.GetParameter("esp", "trace").Value.ToBool())
                    {
                        if(player.InVehicle && player.IsDriver || !player.InVehicle)
                            Overlay.Graphics.DrawLine(Overlay.Width / 2, Overlay.Height / 2, (int)w2sFoot.X, (int)w2sFoot.Y, 1, color);
                    }
                }
            }
        }

        private static Matrix GetTransform(long pEntity)
        {
            long sMartPtr;
            byte pECX;
            byte pEAX;
            long pTr_tran;
            Matrix trans;

            sMartPtr = Memory.Read<long>(pEntity + 0x38);

            pECX = Memory.Read<byte>(sMartPtr + 0x9);
            pEAX = Memory.Read<byte>(sMartPtr + 0xA);
            
            pTr_tran = (pECX + pEAX * 2) * 0x20;
            trans = Memory.Read<Matrix>(sMartPtr + pTr_tran + 0x10);

            return trans;
        }
        
        private static long GetClientSoldierEntity(long pClientPlayer, Player player)
        {
            long pAttached = Memory.Read<long>(pClientPlayer + Offsets.ClientPlayer.m_pAttachedControllable);
            if (Memory.IsValid(pAttached))
            {
                long m_ClientSoldier = Memory.Read<long>(Memory.Read<long>(pClientPlayer + Offsets.ClientPlayer.m_character)) - sizeof(long);
                if (Memory.IsValid(m_ClientSoldier))
                {
                    long pVehicleEntity = Memory.Read<long>(pClientPlayer + Offsets.ClientPlayer.m_pAttachedControllable); //vehicle
                    if (Memory.IsValid(pVehicleEntity))
                    {
                        player.InVehicle = true;
                        // Driver
                        if (Memory.Read<int>(pClientPlayer + Offsets.ClientPlayer.m_attachedEntryId) == 0)
                        {
                            // Vehicle AABB
                            if (CommandHandler.GetParameter("esp", "vehicle").Value.ToBool())
                            {
                                player.VehicleTranfsorm = GetTransform(pVehicleEntity);
                                player.VehicleAABB = Memory.Read<AxisAlignedBox>(pVehicleEntity + Offsets.ClientVehicleEntity.m_childrenAABB);
                            }
                            long _EntityData = Memory.Read<long>(pVehicleEntity + Offsets.ClientSoldierEntity.m_data);
                            if (Memory.IsValid(_EntityData))
                            {
                                long _NameSid = Memory.Read<long>(_EntityData + Offsets.VehicleEntityData.m_NameSid);

                                string strName = Memory.ReadString(_NameSid);
                                if (strName.Length > 9)
                                {
                                    long pAttachedClient = Memory.Read<long>(m_ClientSoldier + Offsets.ClientSoldierEntity.m_pPlayer);
                                    // AttachedControllable Max Health
                                    long p = Memory.Read<long>(pAttachedClient + Offsets.ClientPlayer.m_pAttachedControllable);
                                    long p2 = Memory.Read<long>(p + Offsets.ClientSoldierEntity.m_pHealthComponent);
                                    player.VehicleHealth = Memory.Read<float>(p2 + Offsets.HealthComponent.m_vehicleHealth);

                                    // AttachedControllable Health
                                    player.VehicleMaxHealth = Memory.Read<float>(_EntityData + Offsets.VehicleEntityData.m_FrontMaxHealth);

                                    // AttachedControllable Name
                                    player.VehicleName = strName.Remove(0, 9);
                                    player.IsDriver = true;
                                }
                            }
                        }
                    }
                }
                return m_ClientSoldier;
            }
            return Memory.Read<long>(pClientPlayer + Offsets.ClientPlayer.m_pControlledControllable);
        }

        public static bool WorldToScreen(Vector3 @in, int _Pose, out Vector3 @out)
        {
            bool flag;
            @out = new Vector3(0f, 0f, 0f);
            float y = @in.Y;
            if (_Pose == 0)
            {
                y = y + 1.7f;
            }
            if (_Pose == 1)
            {
                y = y + 1.15f;
            }
            if (_Pose == 2)
            {
                y = y + 0.4f;
            }
            float m14 = ViewMatrix.M14 * @in.X + ViewMatrix.M24 * y + (ViewMatrix.M34 * @in.Z + ViewMatrix.M44);
            if (m14 >= 0.0001f)
            {
                float m11 = ViewMatrix.M11 * @in.X + ViewMatrix.M21 * y + (ViewMatrix.M31 * @in.Z + ViewMatrix.M41);
                float m12 = ViewMatrix.M12 * @in.X + ViewMatrix.M22 * y + (ViewMatrix.M32 * @in.Z + ViewMatrix.M42);
                @out.X = (Overlay.Width / 2) + (Overlay.Width / 2) * m11 / m14;
                @out.Y = (Overlay.Height / 2) - (Overlay.Height / 2) * m12 / m14;
                @out.Z = m14;
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static bool WorldToScreen(Vector3 @in, out Vector3 @out)
        {
            bool flag;
            @out = new Vector3(0f, 0f, 0f);
            float m14 = ViewMatrix.M14 * @in.X + ViewMatrix.M24 * @in.Y + (ViewMatrix.M34 * @in.Z + ViewMatrix.M44);
            if (m14 >= 0.0001f)
            {
                float m11 = ViewMatrix.M11 * @in.X + ViewMatrix.M21 * @in.Y + (ViewMatrix.M31 * @in.Z + ViewMatrix.M41);
                float m12 = ViewMatrix.M12 * @in.X + ViewMatrix.M22 * @in.Y + (ViewMatrix.M32 * @in.Z + ViewMatrix.M42);
                @out.X = (Overlay.Width / 2) + (Overlay.Width / 2) * m11 / m14;
                @out.Y = (Overlay.Height / 2) - (Overlay.Height / 2) * m12 / m14;
                @out.Z = m14;
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }
    }
}
