using AsteroiderCore.ECS.Components;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class UFOMoveDirectionSystem : EcsSystem
    {
        public override void Update()
        {
            var rocketEntities = EcsWorld.GetEntities<Position, Rocket>();
            if (rocketEntities.Count == 0)
                return;
            
            var ufoEntities = EcsWorld.GetEntities<Position, Movable, UFO>();
            if (ufoEntities.Count == 0)
                return;

            var rocketEntity = rocketEntities[0];
            var rocketPosition = rocketEntity.Component1;

            foreach (var ufoEntity in ufoEntities)
            {
                var ufoPosition = ufoEntity.Component1;
                var ufoMovable = ufoEntity.Component2;

                var direction = rocketPosition.VectorPosition - ufoPosition.VectorPosition;
                ufoMovable.Direction = direction.normalized;
            }
        }
    }
}