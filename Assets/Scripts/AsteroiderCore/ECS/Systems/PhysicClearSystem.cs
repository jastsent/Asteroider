using AsteroiderCore.ECS.Components;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class PhysicClearSystem : EcsSystem
    {
        public override void Update()
        {
            ClearRaycasts();
            ClearCollisions();
        }

        private void ClearRaycasts()
        {
            var raycastEntities = EcsWorld.GetEntities<Raycast>();
            foreach (var entity in raycastEntities)
            {
                EcsWorld.RemoveComponent(entity.Id, entity.Component1);
            }
        }

        private void ClearCollisions()
        {
            var rigidBodyEntities = EcsWorld.GetEntities<Rigidbody>();
            foreach (var entity in rigidBodyEntities)
            {
                entity.Component1.EntitiesCollisions.Clear();
            }
        }
    }
}