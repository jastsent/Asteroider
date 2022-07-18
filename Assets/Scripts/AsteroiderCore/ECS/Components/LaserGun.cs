using ECS;

namespace AsteroiderCore.ECS.Components
{
    public sealed class LaserGun : EcsComponent
    {
        public float CooldownTimer;
        public float RechargeTimer;
        public int Charges;
    }
}