using System;
using System.Collections.Generic;
using AsteroiderCore.ECS.Systems;
using AsteroiderCore.GameFSM;
using AsteroiderCore.GameFSM.States;
using AsteroiderCore.Input;
using AsteroiderCore.SaveSystem;
using AsteroiderCore.Settings;
using AsteroiderCore.UI;
using DIContainer;
using ECS;
using UnityEngine;

namespace AsteroiderCore
{
    public sealed class GameRoot : MonoBehaviour
    {
        [field:SerializeField] public GameSettings Settings { get; private set; }
        [field:SerializeField] public BattleConfiguration BattleConfiguration { get; private set; }
        [field:SerializeField] public LayerCollisionMatrix LayerCollisionMatrix { get; private set; }
        [field:SerializeField] public Camera MainCamera { get;  private set; }
        
        private DIContainer.DIContainer _container;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(MainCamera);

            _container = new DIContainer.DIContainer();

            RegisterData();
            RegisterControllers();
            RegisterInput();
            RegisterSaveSystem();
            RegisterStateMachine();
            RegisterEcs();
            RegisterUI();

            var inputController = _container.Resolve<IInputController>();
            inputController.Initialize();
            inputController.EnableInput();

            var stateMachine = _container.Resolve<GameStateMachine>();
            stateMachine.SetState<MainMenuState>();
        }

        private void RegisterData()
        {
            _container
                .RegisterInstance(Settings)
                .RegisterInstance(BattleConfiguration)
                .RegisterInstance(LayerCollisionMatrix)
                .RegisterCallback(Lifestyle.Single, container =>
                {
                    var saveSystem = container.Resolve<ISaver>();
                    var settings = container.Resolve<GameSettings>();
                    var savePath = settings.SavePath;
                    var saveData = saveSystem.FileExist(savePath)
                        ? saveSystem.Load<PlayerData>(savePath)
                        : new PlayerData();
                    return saveData;
                });
        }

        private void RegisterControllers()
        {
            _container
                .Register<PauseManager>(Lifestyle.Single)
                .RegisterCallback(Lifestyle.Single, _ => gameObject.AddComponent<UpdateProvider>())
                .Register<BattleController>(Lifestyle.Single)
                .Register<ObjectPool>(Lifestyle.Single);
        }

        private void RegisterInput()
        {
            _container
                .Register<AsteroiderInputActionAsset>(Lifestyle.Single)
                .Register<IInputController, InputController>(Lifestyle.Single);
        }

        private void RegisterSaveSystem()
        {
            _container
                .Register<JsonSaver.JsonSaver>(Lifestyle.Single)
                .Register<ISaver, SaveSystem.SaveSystem>(Lifestyle.Single);
        }

        private void RegisterStateMachine()
        {
            var dictionary = new Dictionary<Type, Func<BaseState>>();
            void Add<T>() where T : BaseState
            {
                _container.Register<T>(Lifestyle.Transient);
                dictionary.Add(typeof(T), () => _container.Resolve<T>());
            }
            Add<MainMenuState>();
            Add<BattleState>();
            
            _container
                .Register<GameStateMachine>(Lifestyle.Single)
                .RegisterInstance<IReadOnlyDictionary<Type, Func<BaseState>>>(dictionary);
        }

        private void RegisterEcs()
        {
            var dictionary = new Dictionary<Type, Func<EcsSystem>>();
            void Add<T>() where T : EcsSystem
            {
                _container.Register<T>(Lifestyle.Transient);
                dictionary.Add(typeof(T), () => _container.Resolve<T>());
            }
            Add<AsteroidSpawnSystem>();
            Add<CollideSystem>();
            Add<PhysicClearSystem>();
            Add<DamageSystem>();
            Add<DestroyEntitySystem>();
            Add<GameOverSystem>();
            Add<InvulnerabilityTimerSystem>();
            Add<MoveSystem>();
            Add<RocketControlSystem>();
            Add<RespawnSystem>();
            Add<RocketSpawnSystem>();
            Add<UFOMoveDirectionSystem>();
            Add<ViewPositionSystem>();
            Add<ViewRotationSystem>();
            Add<ViewRotationAroundSystem>();
            Add<DestroyOutOfBoundsSystem>();
            Add<ViewInvulnerabilitySystem>();
            Add<UFOSpawnSystem>();
            Add<LaserSystem>();
            Add<ViewLaserSystem>();
            Add<AsteroidShatterSystem>();
            Add<DestroyViewSystem>();
            Add<ShootSystem>();
            Add<ScoreSystem>();
            Add<ViewSpawnSystem>();
            
            _container
                .RegisterInstance(new EcsSettings())
                .Register<EcsWorld>(Lifestyle.Single)
                .RegisterInstance<IReadOnlyDictionary<Type, Func<EcsSystem>>>(dictionary);
        }

        private void RegisterUI()
        {
            _container
                .RegisterWithFactory(Lifestyle.Transient, () => _container.Resolve<MenuPresenter>())
                .RegisterWithFactory(Lifestyle.Transient, () => _container.Resolve<GamePresenter>())
                .RegisterCallback(Lifestyle.Transient, _ => FindObjectOfType<MenuView>(true))
                .RegisterCallback(Lifestyle.Transient, _ => FindObjectOfType<GameView>(true));
        }
    }
}
