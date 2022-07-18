using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class DestroyOutOfBoundsSystem : EcsSystem
    {
        private readonly BattleConfiguration _settings;

        public DestroyOutOfBoundsSystem(BattleConfiguration settings)
        {
            _settings = settings;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Position>();
            foreach (var entity in entities)
            {
                var position = entity.Component1;
                if (position.VectorPosition.x < _settings.BattlefieldWidthMin - _settings.BattlefieldWidth
                    || position.VectorPosition.x > _settings.BattlefieldWidthMax + _settings.BattlefieldWidth
                    || position.VectorPosition.y < _settings.BattlefieldHeightMin - _settings.BattlefieldHeight
                    || position.VectorPosition.y > _settings.BattlefieldHeightMax + _settings.BattlefieldHeight)
                {
                    if (EcsWorld.HasComponent<Destroyed>(entity.Id))
                    {
                        continue;
                    }
                    
                    EcsWorld.AddComponent(entity.Id, new Destroyed());
                }
            }
        }
    }
}