using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ViewLaserSystem : EcsSystem
    {
        private const float LaserTime = 0.7f;
            
        private readonly GameSettings _gameSettings;
        private readonly ObjectPool _objectPool;

        public ViewLaserSystem(GameSettings gameSettings, ObjectPool objectPool)
        {
            _gameSettings = gameSettings;
            _objectPool = objectPool;
        }
        
        public override void Update()
        {
            UpdateLaserView();
            CreateLaserView();
        }

        private void UpdateLaserView()
        {
            var viewLasers = EcsWorld.GetEntities<ViewLaser>();
            foreach (var entity in viewLasers)
            {
                var viewLaser = entity.Component1;

                var timePercentOld = viewLaser.Timer / (LaserTime / 100f);
                viewLaser.Timer -= Time.deltaTime;
                var timePercentNew = viewLaser.Timer / (LaserTime / 100f);
                var currentWidth = viewLaser.LineRenderer.startWidth;
                var widthOnePercent = currentWidth / timePercentOld;
                var newWidth = widthOnePercent * timePercentNew;
                viewLaser.LineRenderer.startWidth = newWidth;
                viewLaser.LineRenderer.endWidth = newWidth;

                if (viewLaser.Timer > 0)
                {
                    continue;
                }

                EcsWorld.AddComponent(entity.Id, new Destroyed());
            }
        }

        private void CreateLaserView()
        {
            var laserRaycasts = EcsWorld.GetEntities<Laser, Raycast>();
            foreach (var entity in laserRaycasts)
            {
                var gameObject = _objectPool.GetObject(ObjectType.Laser);
                if (!gameObject.TryGetComponent<LineRenderer>(out var lineRenderer))
                {
                    _objectPool.PutObject(ObjectType.Laser, gameObject);
                    return;
                }
                
                var raycast = entity.Component2;

                lineRenderer.startWidth = raycast.Width;
                lineRenderer.endWidth = raycast.Width;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, raycast.Origin);
                lineRenderer.SetPosition(1, raycast.Vector.normalized * 100f);

                var newEntity = EcsWorld.CreateEntity();
                var laserView = new ViewLaser
                {
                    LineRenderer = lineRenderer,
                    Timer = LaserTime
                };
                EcsWorld.AddComponent(newEntity, laserView);
            }
        }
    }
}