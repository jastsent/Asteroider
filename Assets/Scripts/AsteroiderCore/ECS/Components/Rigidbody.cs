using System.Collections.Generic;
using AsteroiderCore.Settings;
using ECS;

namespace AsteroiderCore.ECS.Components
{
    public sealed class Rigidbody : EcsComponent
    {
        public CollisionLayer CollisionLayer;
        public readonly List<int> EntitiesCollisions = new();
    }
}