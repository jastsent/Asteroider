using System;
using System.Collections.Generic;

namespace DIContainer
{
    public enum Lifestyle
    {
        Single,
        Transient
    }
    
    public sealed class DIContainer
    {
        private readonly Dictionary<Type, Type> _registeredTypes = new();
        private readonly Dictionary<Type, Lifestyle> _registeredLifestyles = new();
        private readonly Dictionary<Type, Func<DIContainer, object>> _callbacks = new();
        private readonly Dictionary<Type, object> _instances = new();
        private readonly Dictionary<Type, MulticastDelegate> _registeredFactoryMethods = new();
        private Type _currentResolveType;

        public DIContainer Register<T>(Lifestyle lifestyle) where T : class
        {
            var genericType = typeof(T);
            if (!genericType.IsClass)
            {
                throw new Exception($"Resolve type {genericType.Name} is not a class");
            }
            _registeredTypes.Add(genericType, genericType);
            _registeredLifestyles.Add(genericType, lifestyle);
            return this;
        }

        public DIContainer Register<T1, T2>(Lifestyle lifestyle) where T1 : class where T2 : class, T1
        {
            var registerType = typeof(T1);
            var resolveType = typeof(T2);
            _registeredTypes.Add(registerType, resolveType);
            _registeredLifestyles.Add(resolveType, lifestyle);
            return this;
        }
        
        public DIContainer RegisterInstance<T>(T instance) where T : class
        {
            var genericType = typeof(T);
            var instanceType = instance.GetType();
            _registeredTypes.Add(genericType, instanceType);
            _registeredLifestyles.Add(instanceType, Lifestyle.Single);
            _instances.Add(instanceType, instance);
            return this;
        }

        public DIContainer RegisterCallback<T>(Lifestyle lifestyle, Func<DIContainer, T> callback) where T : class
        {
            var genericType = typeof(T);
            _registeredTypes.Add(genericType, genericType);
            _registeredLifestyles.Add(genericType, lifestyle);
            _callbacks.Add(genericType, callback);
            return this;
        }

        public DIContainer RegisterFactory<T>(Func<T> factory) where T : class
        {
            RegisterFactory(factory.GetType(), factory);
            return this;
        }
        
        public DIContainer RegisterFactory<T1, T2>(Func<T1, T2> factory) where T2 : class
        {
            RegisterFactory(factory.GetType(), factory);
            return this;
        }
        
        public DIContainer RegisterFactory<T1, T2, T3>(Func<T1, T2, T3> factory) where T3 : class
        {
            RegisterFactory(factory.GetType(), factory);
            return this;
        }

        private void RegisterFactory(Type type, MulticastDelegate factory)
        {
            _registeredFactoryMethods.Add(type, factory);
        }

        public DIContainer RegisterWithFactory<T>(Lifestyle lifestyle, Func<T> factory) where T : class
        {
            Register<T>(lifestyle);
            RegisterFactory(factory);
            return this;
        }

        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        private object Resolve(Type type)
        {
            if (_registeredTypes.TryGetValue(type, out var registeredType))
            {
                return ResolveType(registeredType);
            }
            
            if (_registeredFactoryMethods.ContainsKey(type))
            {
                return ResolveFactoryMethod(type);
            }
            
            throw new Exception($"Can't resolve {_currentResolveType?.Name}, type {type.Name} not registered");
        }

        private object ResolveType(Type type)
        {
            _currentResolveType = type;
            var lifestyle = _registeredLifestyles[type];
            if (lifestyle == Lifestyle.Single)
            {
                if (_instances.TryGetValue(type, out var instance))
                {
                    return instance;
                }
                var newInstance = CreateInstance(type);
                _instances.Add(type, newInstance);
                return newInstance;
            }
            
            if (lifestyle == Lifestyle.Transient)
            {
                return CreateInstance(type);
            }

            throw new Exception($"Can't find lifestyle for {type.Name}");
        }

        private object CreateInstance(Type type)
        {
            if (_callbacks.ContainsKey(type))
            {
                var instanceFromCallback = _callbacks[type].Invoke(this);
                if (instanceFromCallback is null)
                {
                    throw new Exception($"Resolve callback returned null, type {type.Name}");
                }

                return instanceFromCallback;
            }
            
            var constructors = type.GetConstructors();
            if(constructors.Length > 1)
            {
                throw new Exception($"Can't resolve, type {type.Name} has more than one constructor");
            }
            
            var constructorParams = constructors[0].GetParameters();
            if (constructorParams.Length <= 0)
            {
                return Activator.CreateInstance(type);
            }
            
            var instancesOfDependencies = new object[constructorParams.Length];
            for (var i = 0; i < constructorParams.Length; i++)
            {
                var parameterType = constructorParams[i].ParameterType;
                if (type == parameterType)
                {
                    throw new Exception($"Can't resolve, recursive resolve with type {type.Name}");
                }

                instancesOfDependencies[i] = Resolve(parameterType);
                _currentResolveType = type;
            }
            
            return Activator.CreateInstance(type, instancesOfDependencies);
        }

        private object ResolveFactoryMethod(Type type)
        {
            return _registeredFactoryMethods[type];
        }
    }
}