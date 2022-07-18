using ECS;

namespace AsteroiderCore.ECS.Components
{
    public sealed class Gun : EcsComponent
    {
        public float AttackSpeed;
        public float AttackTimer;
    }
}