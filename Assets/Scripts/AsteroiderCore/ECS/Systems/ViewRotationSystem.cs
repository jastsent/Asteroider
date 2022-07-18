using AsteroiderCore.ECS.Components;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ViewRotationSystem : EcsSystem
    {
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<ViewObject, ViewRotation, Movable>();
            foreach (var entity in entities)
            {
                var view = entity.Component1;
                var movable = entity.Component3;

                var transform = view.GameObject.transform;
                var vector = movable.Direction.normalized;
                var angle = -vector.GetAngleFromVector();
                if (angle != transform.rotation.z)
                {
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}