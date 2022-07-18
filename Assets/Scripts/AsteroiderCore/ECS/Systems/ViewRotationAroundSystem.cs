using AsteroiderCore.ECS.Components;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ViewRotationAroundSystem : EcsSystem
    {
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<ViewObject, ViewRotationAround>();
            foreach (var entity in entities)
            {
                var view = entity.Component1;
                var viewRotation = entity.Component2;

                var transform = view.GameObject.transform;
                transform.Rotate(0, 0, viewRotation.RotationSpeed * Time.deltaTime);
            }
        }
    }
}