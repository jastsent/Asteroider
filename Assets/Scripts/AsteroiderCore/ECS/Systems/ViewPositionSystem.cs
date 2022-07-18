using AsteroiderCore.ECS.Components;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ViewPositionSystem : EcsSystem
    {
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<ViewObject, ViewPosition, Position>();
            foreach (var entity in entities)
            {
                var view = entity.Component1;
                var position = entity.Component3;

                var transform = view.GameObject.transform;
                transform.position = position.VectorPosition;
            }
        }
    }
}