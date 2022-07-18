using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;
using Rigidbody = AsteroiderCore.ECS.Components.Rigidbody;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class CollideSystem : EcsSystem
    {
        private readonly LayerCollisionMatrix _layerCollisionMatrix;

        public CollideSystem(LayerCollisionMatrix layerCollisionMatrix)
        {
            _layerCollisionMatrix = layerCollisionMatrix;
        }
        
        public override void Update()
        {
            var radialColliderEntities = EcsWorld.GetEntities<Position, Rigidbody, RadialCollider>();
            var raycastEntities = EcsWorld.GetEntities<Raycast>();
            for (var i = radialColliderEntities.Count - 1; i >= 0; i--)
            {
                var entity = radialColliderEntities[i];

                for (var j = i - 1; j >= 0 && i > 0; j--)
                {
                    var compareEntity = radialColliderEntities[j];
                    CheckRadialCollision(entity, compareEntity);
                }
                
                foreach (var raycastEntity in raycastEntities)
                {
                    CheckRaycastCollision(entity, raycastEntity);
                }
            }
        }

        private void CheckRadialCollision((int Id, Position Component1, Rigidbody Component2, RadialCollider Component3) entity, 
            (int Id, Position Component1, Rigidbody Component2, RadialCollider Component3) compareEntity)
        {
            var position = entity.Component1;
            var rigidbody = entity.Component2;
            var collider = entity.Component3;
            var comparePosition = compareEntity.Component1;
            var compareRigidbody = compareEntity.Component2;
            var compareCollider = compareEntity.Component3;

            var collided = _layerCollisionMatrix.CheckCollision(rigidbody.CollisionLayer, compareRigidbody.CollisionLayer);
            if (!collided)
            {
                return;
            }

            var distance = Vector2.Distance(position.VectorPosition, comparePosition.VectorPosition);
            distance -= collider.Radius + compareCollider.Radius;

            if (distance <= 0)
            {
                rigidbody.EntitiesCollisions.Add(compareEntity.Id);
                compareRigidbody.EntitiesCollisions.Add(entity.Id);
            }
        }

        private void CheckRaycastCollision((int Id, Position Component1, Rigidbody Component2, RadialCollider Component3) entity,
            (int Id, Raycast Component1) raycastEntity)
        {

            var position = entity.Component1;
            var rigidbody = entity.Component2;
            var collider = entity.Component3;
            var raycast = raycastEntity.Component1;
            
            var collided = _layerCollisionMatrix.CheckCollision(rigidbody.CollisionLayer, raycast.CollisionLayer);
            if (!collided)
            {
                return;
            }

            var originToColliderVector = position.VectorPosition - raycast.Origin;

            if (Vector2.Dot(originToColliderVector.normalized, raycast.Vector.normalized) < 0)
            {
                return;
            }

            var supportPoint = raycast.Vector.normalized * 1000f;
            var originToSupportVector = supportPoint - raycast.Origin;

            var point = raycast.Origin + originToSupportVector
                * (Vector2.Dot(originToColliderVector, originToSupportVector)
                   / Vector2.Dot(originToSupportVector, originToSupportVector));

            var distance = Vector2.Distance(position.VectorPosition, point);
            distance -= raycast.Width / 2f + collider.Radius;

            if (distance <= 0)
            {
                rigidbody.EntitiesCollisions.Add(raycastEntity.Id);
                raycast.EntitiesCollisions.Add(entity.Id);
            }
        }
    }
}