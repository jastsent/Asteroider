using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class MoveSystem : EcsSystem
    {
        private readonly BattleConfiguration _settings;

        public MoveSystem(BattleConfiguration settings)
        {
            _settings = settings;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Position, Movable>();
            foreach (var entity in entities)
            {
                var position = entity.Component1;
                var movable = entity.Component2;

                var oldPosition = position.VectorPosition;
                var moveSpeed = Mathf.Min(movable.MoveSpeed, movable.MaxMoveSpeed) * Time.deltaTime;
                position.VectorPosition += movable.Direction.normalized * moveSpeed;

                if (movable.FlyZone == FlyZone.Everywhere)
                {
                    continue;
                }

                var wasOutside = WasOutside(oldPosition);
                if (wasOutside)
                {
                    continue;
                }
                
                if (movable.FlyZone == FlyZone.InsidePortal)
                {
                    if (position.VectorPosition.x < _settings.BattlefieldWidthMin)
                    {
                        position.VectorPosition.x += _settings.BattlefieldWidth;
                    }
                    else if (position.VectorPosition.x > _settings.BattlefieldWidthMax)
                    {
                        position.VectorPosition.x -= _settings.BattlefieldWidth;
                    }

                    if (position.VectorPosition.y < _settings.BattlefieldHeightMin)
                    {
                        position.VectorPosition.y += _settings.BattlefieldHeight;
                    }
                    else if (position.VectorPosition.y > _settings.BattlefieldHeightMax)
                    {
                        position.VectorPosition.y -= _settings.BattlefieldHeight;
                    }
                }
                else if (movable.FlyZone == FlyZone.Inside)
                {
                    if (position.VectorPosition.x < _settings.BattlefieldWidthMin)
                    {
                        position.VectorPosition.x = _settings.BattlefieldWidthMin;
                    }
                    else if (position.VectorPosition.x > _settings.BattlefieldWidthMax)
                    {
                        position.VectorPosition.x = _settings.BattlefieldWidthMax;
                    }

                    if (position.VectorPosition.y < _settings.BattlefieldHeightMin)
                    {
                        position.VectorPosition.y = _settings.BattlefieldHeightMin;
                    }
                    else if (position.VectorPosition.y > _settings.BattlefieldHeightMax)
                    {
                        position.VectorPosition.y = _settings.BattlefieldHeightMin;
                    }
                }
            }
        }

        private bool WasOutside(Vector2 position)
        {
            return position.x < _settings.BattlefieldWidthMin
                   || position.x > _settings.BattlefieldWidthMax
                   || position.y < _settings.BattlefieldHeightMin
                   || position.y > _settings.BattlefieldHeightMax;
        }
    }
}