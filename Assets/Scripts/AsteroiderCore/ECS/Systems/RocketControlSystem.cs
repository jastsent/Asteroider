using AsteroiderCore.ECS.Components;
using AsteroiderCore.Input;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class RocketControlSystem : EcsSystem
    {
        private readonly BattleConfiguration _configuration;
        private readonly IInputController _inputController;

        public RocketControlSystem(BattleConfiguration configuration, IInputController inputController)
        {
            _configuration = configuration;
            _inputController = inputController;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Movable, Rocket>();
            foreach (var entity in entities)
            {
                var movable = entity.Component1;
                if (_inputController.IsForward && movable.MoveSpeed <= movable.MaxMoveSpeed)
                {
                    movable.MoveSpeed += Time.deltaTime * _configuration.AccelerationSpeed;
                    movable.MoveSpeed = Mathf.Clamp(movable.MoveSpeed, 0, movable.MaxMoveSpeed);
                }
                else if(movable.MoveSpeed > 0)
                {
                    movable.MoveSpeed -= Time.deltaTime * _configuration.SlowdownSpeed;
                    movable.MoveSpeed = Mathf.Clamp(movable.MoveSpeed, 0, movable.MaxMoveSpeed);
                }

                if (_inputController.IsLeft)
                {
                    movable.Direction = 
                        Quaternion.AngleAxis(_configuration.RotationSpeed * Time.deltaTime, Vector3.forward)
                        * movable.Direction;
                }
                else if (_inputController.IsRight)
                {
                    movable.Direction = 
                        Quaternion.AngleAxis(-_configuration.RotationSpeed * Time.deltaTime, Vector3.forward) 
                        * movable.Direction;
                }
            }
        }
    }
}