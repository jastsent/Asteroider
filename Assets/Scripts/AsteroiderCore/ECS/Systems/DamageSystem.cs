using System;
using AsteroiderCore.ECS.Components;
using ECS;
using Rigidbody = AsteroiderCore.ECS.Components.Rigidbody;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class DamageSystem : EcsSystem
    {
        public override void Update()
        {
            CalculateDamage<Rocket>(entity => 
                EcsWorld.HasComponent<Asteroid>(entity) 
                || EcsWorld.HasComponent<UFO>(entity));
            
            CalculateDamage<Asteroid>(entity => 
                EcsWorld.HasComponent<Bullet>(entity)
                || EcsWorld.HasComponent<Laser>(entity));
            
            CalculateDamage<UFO>(entity => 
                EcsWorld.HasComponent<Bullet>(entity)
                || EcsWorld.HasComponent<Laser>(entity));
            
            CalculateDamage<Bullet>(entity => 
                EcsWorld.HasComponent<Asteroid>(entity)
                || EcsWorld.HasComponent<UFO>(entity));
        }

        private void CalculateDamage<T1>(Func<int, bool> damageCheck) where T1 : EcsComponent
        {
            var entities = EcsWorld.GetEntities<T1, Rigidbody>();
            foreach (var entity in entities)
            {
                if(EcsWorld.HasComponent<Invulnerability>(entity.Id))
                    continue;

                var rigidbody = entity.Component2;

                foreach (var collisionEntity in rigidbody.EntitiesCollisions)
                {
                    var damaged = damageCheck.Invoke(collisionEntity);
                    if (damaged)
                    {
                        EcsWorld.AddComponent(entity.Id, new Destroyed());
                        break;
                    }
                }
            }
        }
    }
}