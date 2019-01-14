using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlyadleField
{
    internal struct Offsets
    {
        public static long OFFSET_DXRENDERER = 0x143605698;
        public static long OFFSET_GAMECONTEXT = 0x14341b650;
        public static long OFFSET_GAMERENDERER = 0x14360b120;
        public static long OFFSET_VIEWANGLES = 0x1421CAEE0;
        public static long OFFSET_CURRENT_WEAPONFIRING = OFFSET_VIEWANGLES + 0x8;
        public static long OFFSET_BORDERINPUTNODE = 0x143089CD0;
        public static long OFFSET_SHOTSTATS = 0x142572950;
        public static long OFFSET_FFHWND1 = 0x145604240;
        public static long OFFSET_FFHWND2 = 0x145648af0;
        public static long OFFSET_FFDX = 5451133584L;

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct AimAssist
        {
            public static long m_yaw = 0x14;
            public static long m_pitch = 0x18;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct AxisAlignedBox
        {
            public static long m_Min = 0;
            public static long m_Max = 0x10;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct BorderInputNode
        {
            public static long GetInstance()
            {
                return OFFSET_BORDERINPUTNODE;
            }

            public static long m_pMouse = 0x58;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct BreathControlHandler
        {
            public static long m_breathControlTimer = 0x38;
            public static long m_breathControlMultiplier = 0x3C;
            public static long m_breathControlPenaltyTimer = 0x40;
            public static long m_breathControlpenaltyMultiplier = 0x44;
            public static long m_breathControlActive = 0x48;
            public static long m_breathControlInput = 0x4C;
            public static long m_breathActive = 0x50;
            public static long m_Enabled = 0x58;
        }

        // Token: 0x02000027 RID: 39
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct BulletEntityData
        {
            // Token: 0x040000CB RID: 203
            public static long m_Gravity = 320L;

            // Token: 0x040000CC RID: 204
            public static long m_StartDamage = 368L;

            // Token: 0x040000CD RID: 205
            public static long m_EndDamage = 372L;
        }

        // Token: 0x02000028 RID: 40
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientActiveWeaponHandler
        {
            // Token: 0x040000CE RID: 206
            public static long m_activeWeapon = 56L;
        }

        // Token: 0x02000037 RID: 55
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientCameraComponent
        {
            // Token: 0x040000FB RID: 251
            public static long pChaseorStaticCamera = 184L;
        }

        // Token: 0x0200001B RID: 27
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientChassisComponent
        {
            // Token: 0x0400009F RID: 159
            public static long m_Velocity = 448L;
        }

        // Token: 0x02000012 RID: 18
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientGameContext
        {
            // Token: 0x0600007A RID: 122 RVA: 0x00006364 File Offset: 0x00004564
            public static long GetInstance()
            {
                return OFFSET_GAMECONTEXT;
            }

            // Token: 0x04000085 RID: 133
            public static long m_pPhysicsManager = 0x28;

            // Token: 0x04000086 RID: 134
            public static long m_pPlayerManager = 0x68;
        }

        // Token: 0x02000015 RID: 21
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientPlayer
        {
            // Token: 0x0400008A RID: 138
            public static long szName = 64L;

            // Token: 0x0400008B RID: 139
            public static long m_isSpectator = 5065L;

            // Token: 0x0400008C RID: 140
            public static long m_teamId = 7220L;

            // Token: 0x0400008D RID: 141
            public static long m_character = 7464L;

            // Token: 0x0400008E RID: 142
            public static long m_ownPlayerView = 5392L;

            // Token: 0x0400008F RID: 143
            public static long m_PlayerView = 5408L;

            // Token: 0x04000090 RID: 144
            public static long m_pAttachedControllable = 0x1D38;

            // Token: 0x04000091 RID: 145
            public static long m_pControlledControllable = 7496L;

            // Token: 0x04000092 RID: 146
            public static long m_attachedEntryId = 7504L;
        }

        // Token: 0x02000013 RID: 19
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientPlayerManager
        {
            // Token: 0x04000087 RID: 135
            public static long m_pLocalPlayer = 1400L;

            // Token: 0x04000088 RID: 136
            public static long m_ppPlayer = 256L;
        }

        // Token: 0x02000014 RID: 20
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientPlayerView
        {
            // Token: 0x04000089 RID: 137
            public static long m_Owner = 248L;
        }

        // Token: 0x02000021 RID: 33
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientRagDollComponent
        {
            // Token: 0x040000BA RID: 186
            public static long m_ragdollTransforms = 136L;

            // Token: 0x040000BB RID: 187
            public static long m_Transform = 1488L;
        }

        // Token: 0x0200002A RID: 42
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientSoldierAimingSimulation
        {
            // Token: 0x040000D0 RID: 208
            public static long m_fpsAimer = 16L;

            // Token: 0x040000D1 RID: 209
            public static long m_yaw = 24L;

            // Token: 0x040000D2 RID: 210
            public static long m_pitch = 28L;

            // Token: 0x040000D3 RID: 211
            public static long m_sway = 40L;

            // Token: 0x040000D4 RID: 212
            public static long m_zoomLevel = 104L;
        }

        // Token: 0x0200001C RID: 28
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientSoldierEntity
        {
            // Token: 0x040000A0 RID: 160
            public static long m_data = 48L;

            // Token: 0x040000A1 RID: 161
            public static long m_pPlayer = 608L;

            // Token: 0x040000A2 RID: 162
            public static long m_pHealthComponent = 448L;

            // Token: 0x040000A3 RID: 163
            public static long m_authorativeYaw = 1428L;

            // Token: 0x040000A4 RID: 164
            public static long m_authorativePitch = 1476L;

            // Token: 0x040000A5 RID: 165
            public static long m_poseType = 0x5E8;

            // Token: 0x040000A6 RID: 166
            public static long m_RenderFlags = 1484L;

            // Token: 0x040000A7 RID: 167
            public static long m_pPhysicsEntity = 568L;

            // Token: 0x040000A8 RID: 168
            public static long m_pPredictedController = 0x0598;

            // Token: 0x040000A9 RID: 169
            public static long m_soldierWeaponsComponent = 0x0648;

            // Token: 0x040000AA RID: 170
            public static long m_ragdollComponent = 1400L;

            // Token: 0x040000AB RID: 171
            public static long m_breathControlHandler = 1416L;

            // Token: 0x040000AC RID: 172
            public static long m_sprinting = 1656L;

            // Token: 0x040000AD RID: 173
            public static long m_occluded = 0x69B;

            // Token: 0x040000AE RID: 174
            public static long m_pClientAimEntity = 1608L;

            // Token: 0x040000AF RID: 175
            public static long m_pActiveSoldierWeapon = 1616L;
        }

        // Token: 0x0200001E RID: 30
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientSoldierPrediction
        {
            // Token: 0x040000B3 RID: 179
            public static long m_Position = 0x40;

            // Token: 0x040000B4 RID: 180
            public static long m_Velocity = 0x0060;
        }

        // Token: 0x02000023 RID: 35
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientSoldierWeapon
        {
            // Token: 0x040000BE RID: 190
            public static long m_data = 48L;

            // Token: 0x040000BF RID: 191
            public static long m_authorativeAiming = 18824L;

            // Token: 0x040000C0 RID: 192
            public static long m_pWeapon = 18960L;

            // Token: 0x040000C1 RID: 193
            public static long m_pPrimary = 18984L;

            // Token: 0x040000C2 RID: 194
            public static long m_pSoldierWeaponData = 48L;
        }

        // Token: 0x0200001F RID: 31
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientSoldierWeaponsComponent
        {
            // Token: 0x040000B5 RID: 181
            public static long m_handler = 2192L;

            // Token: 0x040000B6 RID: 182
            public static long m_activeSlot = 2712L;

            // Token: 0x040000B7 RID: 183
            public static long m_activeHandler = 2256L;

            // Token: 0x0200003F RID: 63
            public enum WeaponSlot
            {
                // Token: 0x04000100 RID: 256
                M_PRIMARY,
                // Token: 0x04000101 RID: 257
                M_SECONDARY,
                // Token: 0x04000102 RID: 258
                M_GADGET,
                // Token: 0x04000103 RID: 259
                M_GRENADE = 6,
                // Token: 0x04000104 RID: 260
                M_KNIFE
            }
        }

        // Token: 0x02000016 RID: 22
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientVehicleEntity
        {
            // Token: 0x04000093 RID: 147
            public static long m_data = 48L;

            // Token: 0x04000094 RID: 148
            public static long m_pPhysicsEntity = 568L;

            // Token: 0x04000095 RID: 149
            public static long m_Velocity = 640L;

            // Token: 0x04000096 RID: 150
            public static long m_prevVelocity = 656L;

            // Token: 0x04000097 RID: 151
            public static long m_Chassis = 992L;

            // Token: 0x04000098 RID: 152
            public static long m_childrenAABB = 0x2d0;
        }

        // Token: 0x0200002B RID: 43
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ClientWeapon
        {
            // Token: 0x040000D5 RID: 213
            public static long m_pModifier = 32L;

            // Token: 0x040000D6 RID: 214
            public static long m_shootSpace = 64L;
        }

        // Token: 0x02000018 RID: 24
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct DynamicPhysicsEntity
        {
            // Token: 0x0400009B RID: 155
            public static long m_EntityTransform = 160L;
        }

        // Token: 0x02000026 RID: 38
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct FiringFunctionData
        {
            // Token: 0x040000C5 RID: 197
            public static long m_initialSpeed = 136L;

            // Token: 0x040000C6 RID: 198
            public static long m_pBulletEntityData = 192L;

            // Token: 0x040000C7 RID: 199
            public static long m_NumberOfBulletsPerShot = 228L;

            // Token: 0x040000C8 RID: 200
            public static long m_NumberOfBulletsPerShell = 224L;

            // Token: 0x040000C9 RID: 201
            public static long m_NumberOfBulletsPerBurst = 232L;

            // Token: 0x040000CA RID: 202
            public static long m_RateOfFire = 504L;
        }

        // Token: 0x02000031 RID: 49
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct GameRenderer
        {
            // Token: 0x0600009A RID: 154 RVA: 0x00006834 File Offset: 0x00004A34
            public static long GetInstance()
            {
                return Offsets.OFFSET_GAMERENDERER;
            }

            // Token: 0x040000F0 RID: 240
            public static long m_pRenderView = 0x60;
        }

        // Token: 0x0200002E RID: 46
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct GunSwayData
        {
            // Token: 0x040000E0 RID: 224
            public static long m_DeviationScaleFactorZoom = 864L;

            // Token: 0x040000E1 RID: 225
            public static long m_GameplayDeviationScaleFactorZoom = 868L;

            // Token: 0x040000E2 RID: 226
            public static long m_DeviationScaleFactorNoZoom = 872L;

            // Token: 0x040000E3 RID: 227
            public static long m_GameplayDeviationScaleFactorNoZoom = 876L;

            // Token: 0x040000E4 RID: 228
            public static long m_ShootingRecoilDecreaseScale = 880L;

            // Token: 0x040000E5 RID: 229
            public static long m_FirstShotRecoilMultiplier = 884L;
        }

        // Token: 0x0200001D RID: 29
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct HealthComponent
        {
            // Token: 0x040000B0 RID: 176
            public static long m_Health = 32L;

            // Token: 0x040000B1 RID: 177
            public static long m_MaxHealth = 36L;

            // Token: 0x040000B2 RID: 178
            public static long m_vehicleHealth = 56L;
        }

        // Token: 0x02000034 RID: 52
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct Mouse
        {
            // Token: 0x040000F8 RID: 248
            public static long m_pDevice = 16L;
        }

        // Token: 0x02000035 RID: 53
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct MouseDevice
        {
            // Token: 0x040000F9 RID: 249
            public static long m_Buffer = 260L;
        }

        // Token: 0x02000019 RID: 25
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct PhysicsEntityTransform
        {
            // Token: 0x0400009C RID: 156
            public static long m_Transform = 0L;
        }

        // Token: 0x02000022 RID: 34
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct QuatTransform
        {
            // Token: 0x040000BC RID: 188
            public static long m_TransAndScale = 0L;

            // Token: 0x040000BD RID: 189
            public static long m_Rotation = 16L;
        }

        // Token: 0x02000032 RID: 50
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct RenderView
        {
            // Token: 0x040000F1 RID: 241
            public static long m_Transform = 64L;

            // Token: 0x040000F2 RID: 242
            public static long m_FovY = 180L;

            // Token: 0x040000F3 RID: 243
            public static long m_fovX = 592L;

            // Token: 0x040000F4 RID: 244
            public static long m_ViewProj = 1120L;

            // Token: 0x040000F5 RID: 245
            public static long m_ViewMatrixInverse = 800L;

            // Token: 0x040000F6 RID: 246
            public static long m_ViewProjInverse = 1248L;
        }

        // Token: 0x02000024 RID: 36
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct SoldierWeaponData
        {
            // Token: 0x040000C3 RID: 195
            public static long m_pWeaponFiringData = 144L;
        }

        // Token: 0x02000038 RID: 56
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct StaticCamera
        {
            // Token: 0x040000FC RID: 252
            public static long m_PreCrossMatrix = 16L;

            // Token: 0x040000FD RID: 253
            public static long m_CrossMatrix = 80L;

            // Token: 0x040000FE RID: 254
            public static long m_ForwardOffset = 464L;
        }

        // Token: 0x02000020 RID: 32
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct UpdatePoseResultData
        {
            // Token: 0x040000B8 RID: 184
            public static long m_ActiveWorldTransforms = 40L;

            // Token: 0x040000B9 RID: 185
            public static long m_ValidTransforms = 64L;

            // Token: 0x02000040 RID: 64
            public enum BONES
            {
                // Token: 0x04000106 RID: 262
                BONE_HEAD = 35,
                // Token: 0x04000107 RID: 263
                BONE_NECK = 33,
                // Token: 0x04000108 RID: 264
                BONE_SPINE2 = 7,
                // Token: 0x04000109 RID: 265
                BONE_SPINE1 = 6,
                // Token: 0x0400010A RID: 266
                BONE_SPINE = 5,
                // Token: 0x0400010B RID: 267
                BONE_LEFTSHOULDER = 8,
                // Token: 0x0400010C RID: 268
                BONE_RIGHTSHOULDER = 163,
                // Token: 0x0400010D RID: 269
                BONE_LEFTELBOWROLL = 14,
                // Token: 0x0400010E RID: 270
                BONE_RIGHTELBOWROLL = 169,
                // Token: 0x0400010F RID: 271
                BONE_LEFTHAND = 10,
                // Token: 0x04000110 RID: 272
                BONE_RIGHTHAND = 171,
                // Token: 0x04000111 RID: 273
                BONE_LEFTKNEEROLL = 287,
                // Token: 0x04000112 RID: 274
                BONE_RIGHTKNEEROLL = 301,
                // Token: 0x04000113 RID: 275
                BONE_LEFTFOOT = 115,
                // Token: 0x04000114 RID: 276
                BONE_RIGHTFOOT = 123
            }
        }

        // Token: 0x0200001A RID: 26
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct VehicleEntityData
        {
            // Token: 0x0400009D RID: 157
            public static long m_FrontMaxHealth = 328L;

            // Token: 0x0400009E RID: 158
            public static long m_NameSid = 0x130;
        }

        // Token: 0x02000036 RID: 54
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct VehicleWeapon
        {
            // Token: 0x060000A1 RID: 161 RVA: 0x000068DC File Offset: 0x00004ADC
            public static long GetInstance()
            {
                return Offsets.OFFSET_CURRENT_WEAPONFIRING;
            }

            // Token: 0x040000FA RID: 250
            public static long m_pClientCameraComponent = 16L;
        }

        // Token: 0x02000029 RID: 41
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct WeaponEntityData
        {
            // Token: 0x040000CF RID: 207
            public static long m_name = 304L;
        }

        // Token: 0x0200002C RID: 44
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct WeaponFiring
        {
            // Token: 0x040000D7 RID: 215
            public static long m_pSway = 120L;

            // Token: 0x040000D8 RID: 216
            public static long m_pPrimaryFire = 296L;

            // Token: 0x040000D9 RID: 217
            public static long m_projectilesLoaded = 416L;

            // Token: 0x040000DA RID: 218
            public static long m_projectilesInMagazines = 420L;

            // Token: 0x040000DB RID: 219
            public static long m_overheatPenaltyTimer = 432L;

            // Token: 0x040000DC RID: 220
            public static long m_RecoilTimer = 360L;
        }

        // Token: 0x02000025 RID: 37
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct WeaponFiringData
        {
            // Token: 0x040000C4 RID: 196
            public static long m_FiringFunctionData = 16L;
        }

        // Token: 0x0200002D RID: 45
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct WeaponSway
        {
            // Token: 0x040000DD RID: 221
            public static long m_pSwayData = 8L;

            // Token: 0x040000DE RID: 222
            public static long m_deviationPitch = 304L;

            // Token: 0x040000DF RID: 223
            public static long m_deviationYaw = 308L;
        }
    }
}
