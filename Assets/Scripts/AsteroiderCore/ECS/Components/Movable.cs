using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Components
{
    public enum FlyZone
    {
        Inside,
        InsidePortal,
        Everywhere
    }
    
    public sealed class Movable : EcsComponent
    {
        public float MaxMoveSpeed;
        public float MoveSpeed;
        public Vector2 Direction;
        public FlyZone FlyZone;
    }
}
