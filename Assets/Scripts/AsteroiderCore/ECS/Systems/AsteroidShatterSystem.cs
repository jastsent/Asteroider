using AsteroiderCore.ECS.Components;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class AsteroidShatterSystem : EcsSystem
    {
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Position, Asteroid, Destroyed>();
            foreach (var entity in entities)
            {
                var position = entity.Component1;
                var asteroid = entity.Component2;

                var newEntity = EcsWorld.CreateEntity();
                var newPosition = new Position
                {
                    VectorPosition = position.VectorPosition 
                };
                
                var shatteredAsteroid = new ShatteredAsteroid
                {
                    SplitCount =  asteroid.SplitCount
                };

                EcsWorld.AddComponent(newEntity, newPosition);
                EcsWorld.AddComponent(newEntity, shatteredAsteroid);
            }
        }
    }
}