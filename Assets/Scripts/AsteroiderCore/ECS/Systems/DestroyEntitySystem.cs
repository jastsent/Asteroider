using AsteroiderCore.ECS.Components;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class DestroyEntitySystem : EcsSystem
    {
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Destroyed>();
            foreach (var entity in entities)
            {
                EcsWorld.RemoveEntity(entity.Id);
            }
        }
    }
}