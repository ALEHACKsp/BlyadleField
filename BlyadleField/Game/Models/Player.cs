using BlyadleField.Game.Structs;
using SharpDX;

namespace BlyadleField.Game.Models
{
    internal class Player
    {
        public string Name;

        public string VehicleName;

        public int Team;

        public Vector3 Origin;

        public RagDoll Bones;

        public bool InVehicle;

        public bool IsDriver;

        public bool IsSpectator;

        public bool IsCover;

        public Vector2 Fov = new Vector2();

        public bool Head;

        private static object Memory;

        private static object enderecoooo;

        private static long Adress;

        private static object num;

        public int Ammo;

        public int AmmoClip;

        public int Pose;

        public int IsOccluded;

        public float Yaw;

        public float Distance;

        public float Health;

        public float MaxHealth;

        public float VehicleHealth;

        public float VehicleMaxHealth;

        public AxisAlignedBox VehicleAABB;

        public Matrix VehicleTranfsorm;

        public Player()
        {
        }

        public AxisAlignedBox GetAABB()
        {
            AxisAlignedBox vector4 = new AxisAlignedBox();
            if (Pose == 0)
            {
                vector4.Min = new Vector4(-0.35f, 0f, -0.35f, 0f);
                vector4.Max = new Vector4(0.35f, 1.7f, 0.35f, 0f);
            }
            if (Pose == 1)
            {
                vector4.Min = new Vector4(-0.35f, 0f, -0.35f, 0f);
                vector4.Max = new Vector4(0.35f, 1.15f, 0.35f, 0f);
            }
            if (Pose == 2)
            {
                vector4.Min = new Vector4(-0.35f, 0f, -0.35f, 0f);
                vector4.Max = new Vector4(0.35f, 0.4f, 0.35f, 0f);
            }
            return vector4;
        }

        public bool IsValid()
        {
            return (this.Health <= 0.1f || this.Health > 100f ? false : !this.Origin.IsZero);
        }

        public bool IsVisible()
        {
            return this.IsOccluded == 0;
        }
    }
}
