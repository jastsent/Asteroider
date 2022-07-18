using System.Collections.Generic;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Components
{
    public sealed class Raycast : EcsComponent
    {
        public Vector2 Origin;
        public CollisionLayer CollisionLayer;
        public Vector2 Vector;
        public float Width;
        public readonly List<int> EntitiesCollisions = new();
    }
}