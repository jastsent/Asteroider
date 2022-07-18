using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class EcsWorld
    {
        public EcsSettings Settings { get; }

        private readonly List<EcsSystem> _systems;
        private readonly Dictionary<int, Dictionary<Type, EcsComponent>> _entities;
        private readonly Dictionary<Type, Dictionary<int, EcsComponent>> _components;
        private int _entitiesIdCount;
        private int _currentSystemIndex = -1;

        public EcsWorld(EcsSettings settings)
        {
            Settings = settings;

            _systems = new List<EcsSystem>(Settings.SystemsCacheSize);
            _entities = new Dictionary<int, Dictionary<Type, EcsComponent>>(Settings.EntitiesCacheSize);
            _components = new Dictionary<Type, Dictionary<int, EcsComponent>>(Settings.ComponentTypesCacheSize);
        }

        public void AddComponent(int entity, EcsComponent component)
        {
            var entityComponents = GetEntityComponents(entity);
            var componentType = component.GetType();

            if (_components.TryGetValue(componentType, out var components))
            {
                components.Add(entity, component);
            }
            else
            {
                components = new Dictionary<int, EcsComponent>(Settings.EntitiesCacheSize)
                {
                    {entity, component}
                };
                _components.Add(componentType, components);
            }

            entityComponents.Add(componentType, component);
        }

        public void RemoveComponent(int entity, EcsComponent component)
        {
            var entityComponents = GetEntityComponents(entity);
            var componentType = component.GetType();

            if (!entityComponents.TryGetValue(componentType, out var registeredComponent))
                return;

            if (registeredComponent != component)
                return;

            entityComponents.Remove(componentType);
            _components[componentType].Remove(entity);
        }

        public int CreateEntity()
        {
            var newEntity = GetNewId();
            _entities.Add(newEntity, new Dictionary<Type, EcsComponent>(Settings.ComponentTypesCacheSize));
            return newEntity;
        }

        public EcsWorld RemoveEntity(int entity)
        {
            var entityComponents = GetEntityComponents(entity);
            foreach (var componentType in entityComponents.Keys)
            {
                _components[componentType].Remove(entity);
            }

            _entities.Remove(entity);
            return this;
        }

        public EcsWorld AddSystem(EcsSystem system)
        {
            _systems.Add(system);
            system.SetWorld(this);
            return this;
        }

        public void RemoveSystem(EcsSystem system)
        {
            system.Destroy();
            var systemIndex = _systems.IndexOf(system);
            if (systemIndex < 0)
            {
                return;
            }
            
            _systems.RemoveAt(systemIndex);
            
            if (_currentSystemIndex >= 0 && systemIndex <= _currentSystemIndex)
            {
                _currentSystemIndex--;
            }
        }

        public void Update()
        {
            for (var i = 0; i < _systems.Count; i++)
            {
                _currentSystemIndex = i;
                var system = _systems[i];
                system.Update();
            }

            _currentSystemIndex = -1;
        }

        public List<(int Id, T Component1)> GetEntities<T>() 
            where T : EcsComponent
        {
            var requestedComponents = new List<(int, T)>();

            var componentType = typeof(T);
            if (!_components.TryGetValue(componentType, out var components))
            {
                return requestedComponents;
            }

            foreach (var entity in components.Keys)
            {
                var entityComponents = _entities[entity];
                T component;

                if(entityComponents.TryGetValue(componentType, out var nextComponent))
                {
                    component = nextComponent as T;
                }
                else
                {
                    continue;
                }
                
                requestedComponents.Add((entity, component));
            }
            
            return requestedComponents;
        }

        public List<(int Id, T1 Component1, T2 Component2)> GetEntities<T1, T2>() 
            where T1 : EcsComponent where T2 : EcsComponent
        {
            var requestedComponents = new List<(int, T1, T2)>();

            var firstComponentType = typeof(T1);
            if (!_components.TryGetValue(firstComponentType, out var firstTypeComponents))
            {
                return requestedComponents;
            }

            var secondComponentType = typeof(T2);
            if (!_components.TryGetValue(secondComponentType, out var secondTypeComponents))
            {
                return requestedComponents;
            }

            var componentsLeast = firstTypeComponents.Count > secondTypeComponents.Count ? secondTypeComponents : firstTypeComponents;

            foreach (var entity in componentsLeast.Keys)
            {
                var entityComponents = _entities[entity];
                (int, T1, T2) components;
                components.Item1 = entity;

                if(entityComponents.TryGetValue(firstComponentType, out var nextComponent))
                {
                    components.Item2 = nextComponent as T1;
                }
                else
                {
                    continue;
                }
                
                if(entityComponents.TryGetValue(secondComponentType, out nextComponent))
                {
                    components.Item3 = nextComponent as T2;
                }
                else
                {
                    continue;
                }
                
                requestedComponents.Add(components);
            }
            
            return requestedComponents;
        }

        public List<(int Id, T1 Component1, T2 Component2, T3 Component3)> GetEntities<T1, T2, T3>() 
            where T1 : EcsComponent where T2 : EcsComponent where T3 : EcsComponent
        {
            var requestedComponents = new List<(int, T1, T2, T3)>();

            var firstComponentType = typeof(T1);
            if (!_components.TryGetValue(firstComponentType, out var firstTypeComponents))
            {
                return requestedComponents;
            }

            var secondComponentType = typeof(T2);
            if (!_components.TryGetValue(secondComponentType, out var secondTypeComponents))
            {
                return requestedComponents;
            }
            
            var thirdComponentType = typeof(T3);
            if (!_components.TryGetValue(thirdComponentType, out var thirdTypeComponents))
            {
                return requestedComponents;
            }

            var componentsLeast = firstTypeComponents.Count > secondTypeComponents.Count ? secondTypeComponents : firstTypeComponents;
            componentsLeast = componentsLeast.Count > thirdTypeComponents.Count ? thirdTypeComponents : componentsLeast;

            foreach (var entity in componentsLeast.Keys)
            {
                var entityComponents = _entities[entity];
                (int, T1, T2, T3) components;
                components.Item1 = entity;

                if(entityComponents.TryGetValue(firstComponentType, out var nextComponent))
                {
                    components.Item2 = nextComponent as T1;
                }
                else
                {
                    continue;
                }
                
                if(entityComponents.TryGetValue(secondComponentType, out nextComponent))
                {
                    components.Item3 = nextComponent as T2;
                }
                else
                {
                    continue;
                }
                
                if(entityComponents.TryGetValue(thirdComponentType, out nextComponent))
                {
                    components.Item4 = nextComponent as T3;
                }
                else
                {
                    continue;
                }
                
                requestedComponents.Add(components);
            }
            
            return requestedComponents;
        }

        public List<(int Id, T1 Component1, T2 Component2, T3 Component3, T4 Component4)> GetEntities<T1, T2, T3, T4>() 
            where T1 : EcsComponent where T2 : EcsComponent where T3 : EcsComponent where T4 : EcsComponent
        {
            var requestedComponents = new List<(int, T1, T2, T3, T4)>();

            var firstTypeComponent = typeof(T1);
            if (!_components.TryGetValue(firstTypeComponent, out var firstTypeComponents))
            {
                return requestedComponents;
            }

            var secondTypeComponent = typeof(T2);
            if (!_components.TryGetValue(secondTypeComponent, out var secondTypeComponents))
            {
                return requestedComponents;
            }
            
            var thirdTypeComponent = typeof(T3);
            if (!_components.TryGetValue(thirdTypeComponent, out var thirdTypeComponents))
            {
                return requestedComponents;
            }
            
            var fourthTypeComponent = typeof(T4);
            if (!_components.TryGetValue(fourthTypeComponent, out var fourthTypeComponents))
            {
                return requestedComponents;
            }

            var componentsLeast = firstTypeComponents.Count > secondTypeComponents.Count ? secondTypeComponents : firstTypeComponents;
            componentsLeast = componentsLeast.Count > thirdTypeComponents.Count ? thirdTypeComponents : componentsLeast;
            componentsLeast = componentsLeast.Count > fourthTypeComponents.Count ? fourthTypeComponents : componentsLeast;

            foreach (var entity in componentsLeast.Keys)
            {
                var entityComponents = _entities[entity];
                (int, T1, T2, T3, T4) components;
                components.Item1 = entity;

                if(entityComponents.TryGetValue(firstTypeComponent, out var nextComponent))
                {
                    components.Item2 = nextComponent as T1;
                }
                else
                {
                    continue;
                }
                
                if(entityComponents.TryGetValue(secondTypeComponent, out nextComponent))
                {
                    components.Item3 = nextComponent as T2;
                }
                else
                {
                    continue;
                }
                
                if(entityComponents.TryGetValue(thirdTypeComponent, out nextComponent))
                {
                    components.Item4 = nextComponent as T3;
                }
                else
                {
                    continue;
                }
                
                if(entityComponents.TryGetValue(fourthTypeComponent, out nextComponent))
                {
                    components.Item5 = nextComponent as T4;
                }
                else
                {
                    continue;
                }
                
                requestedComponents.Add(components);
            }
            
            return requestedComponents;
        }
        
        public T GetComponent<T>(int entity) where T : EcsComponent
        {
            var entityComponents = GetEntityComponents(entity);
            var componentType = typeof(T);
            if (entityComponents.TryGetValue(componentType, out var component))
            {
                return component as T;
            }

            throw new Exception($"Can't find component {componentType.Name}");
        }

        public bool HasComponent<T>(int entity) where T : EcsComponent
        {
            var entityComponents = GetEntityComponents(entity);
            var componentType = typeof(T);
            return entityComponents.ContainsKey(componentType);
        }

        public void Clear()
        {
            foreach (var system in _systems)
            {
                system.Destroy();
            }

            _systems.Clear();
            _entities.Clear();
            _components.Clear();
            _entitiesIdCount = 0;
            _currentSystemIndex = -1;
        }

        private int GetNewId()
        {
            return _entitiesIdCount++;
        }

        private Dictionary<Type, EcsComponent> GetEntityComponents(int entity)
        {
            if (!_entities.TryGetValue(entity, out var entityComponents))
            {
                throw new Exception($"Entity {entity} not exist, but you trying add component to it!");
            }

            return entityComponents;
        }
    }

    public sealed class EcsSettings
    {
        public readonly int EntitiesCacheSize;
        public readonly int ComponentTypesCacheSize;
        public readonly int SystemsCacheSize;

        public EcsSettings(
            int entitiesCacheSize = 100,  
            int componentTypesCacheSize = 30, 
            int systemsCacheSize = 25)
        {
            EntitiesCacheSize = entitiesCacheSize;
            ComponentTypesCacheSize = componentTypesCacheSize;
            SystemsCacheSize = systemsCacheSize;
        }
    }
}


