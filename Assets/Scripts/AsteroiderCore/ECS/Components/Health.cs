using ECS;

namespace AsteroiderCore.ECS.Components
{
    public sealed class Health : EcsComponent
    {
        public int MaxHealthPoints;
        public int HealthPoints;
    }
}