using System;
using System.Collections.Generic;
using AsteroiderCore.ECS.Systems;
using AsteroiderCore.Settings;
using ECS;
using EcsWorld = ECS.EcsWorld;

namespace AsteroiderCore
{
    public sealed class BattleController
    {
        private readonly UpdateProvider _updateProvider;
        private readonly IReadOnlyDictionary<Type, Func<EcsSystem>> _systemFabrics;
        private readonly PauseManager _pauseManager;
        private readonly ObjectPool _objectPool;
        private readonly BattleConfiguration _battleConfiguration;
        private readonly EcsWorld _ecsWorld;
        private readonly GameSettings _gameSettings;

        public BattleController(UpdateProvider updateProvider, EcsWorld ecsWorld, GameSettings gameSettings,
            IReadOnlyDictionary<Type, Func<EcsSystem>> systemFabrics, PauseManager pauseManager, 
            ObjectPool objectPool, BattleConfiguration battleConfiguration)
        {
            _updateProvider = updateProvider;
            _ecsWorld = ecsWorld;
            _gameSettings = gameSettings;
            _systemFabrics = systemFabrics;
            _pauseManager = pauseManager;
            _objectPool = objectPool;
            _battleConfiguration = battleConfiguration;
        }
        
        public void StartBattle()
        {
            _objectPool.RegisterObjects(ObjectType.Asteroid, _gameSettings.AsteroidPrefab, _battleConfiguration.AsteroidsMaxCount);
            _objectPool.RegisterObjects(ObjectType.Rocket, _gameSettings.RocketPrefab, 1);
            _objectPool.RegisterObjects(ObjectType.Bullet, _gameSettings.BulletPrefab, 20);
            _objectPool.RegisterObjects(ObjectType.Ufo, _gameSettings.UFOPrefab, _battleConfiguration.UfoMaxCount);
            _objectPool.RegisterObjects(ObjectType.Laser, _gameSettings.LaserPrefab, 1);
            
            _ecsWorld
                .AddSystem(GetSystem<DestroyOutOfBoundsSystem>())
                .AddSystem(GetSystem<DestroyViewSystem>())
                .AddSystem(GetSystem<DestroyEntitySystem>())
                .AddSystem(GetSystem<PhysicClearSystem>())
                .AddSystem(GetSystem<InvulnerabilityTimerSystem>())

                .AddSystem(GetSystem<RocketControlSystem>())
                .AddSystem(GetSystem<UFOMoveDirectionSystem>())
                .AddSystem(GetSystem<MoveSystem>())
                
                .AddSystem(GetSystem<RocketSpawnSystem>())
                .AddSystem(GetSystem<AsteroidSpawnSystem>())
                .AddSystem(GetSystem<UFOSpawnSystem>())
                .AddSystem(GetSystem<ShootSystem>())
                .AddSystem(GetSystem<LaserSystem>())
                
                .AddSystem(GetSystem<CollideSystem>())
                .AddSystem(GetSystem<DamageSystem>())

                .AddSystem(GetSystem<ViewPositionSystem>())
                .AddSystem(GetSystem<ViewRotationSystem>())
                .AddSystem(GetSystem<ViewRotationAroundSystem>())
                .AddSystem(GetSystem<ViewInvulnerabilitySystem>())
                .AddSystem(GetSystem<ViewLaserSystem>())

                .AddSystem(GetSystem<RespawnSystem>())
                .AddSystem(GetSystem<AsteroidShatterSystem>())
                .AddSystem(GetSystem<ScoreSystem>())
                .AddSystem(GetSystem<GameOverSystem>());
            
            _updateProvider.AddListenerUpdate(OnUpdate);
        }

        private EcsSystem GetSystem<T>() where T : EcsSystem
        {
            return _systemFabrics[typeof(T)]?.Invoke();
        }

        public void EndBattle()
        {
            _objectPool.Dispose();
            _updateProvider.RemoveListenerUpdate(OnUpdate);
            _ecsWorld.Clear();
        }

        private void OnUpdate(float deltaTime)
        {
            if(!_pauseManager.IsPaused)
                _ecsWorld.Update();
        }
    }
}