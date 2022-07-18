using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AsteroiderCore
{
    public sealed class ObjectPool
    {
        private const string RootObjectName = "[POOL]";
        
        private readonly Dictionary<ObjectType, GameObject> _registeredObjects = new();
        private readonly Dictionary<ObjectType, Stack<GameObject>> _spawnedGameObjects = new();
        private GameObject _rootObject;

        public void RegisterObjects(ObjectType objectType, GameObject obj, int count)
        {
            if (IsObjectTypeRegistered(objectType))
            {
                throw new Exception($"Object {objectType} already registered in object pool");
            }
            
            RegisterObjectType(objectType, obj, count);
            CreateRootObject();

            for (int i = 0; i < count; i++)
            {
                var newObject = Object.Instantiate(obj);
                Put(objectType, newObject);
            }
        }

        public void SpawnObjects(ObjectType objectType, int count)
        {
            if (!IsObjectTypeRegistered(objectType))
            {
                throw ObjectNotRegisteredException(objectType);
            }
            
            for (int i = 0; i < count; i++)
            {
                var objExemplar = _registeredObjects[objectType];
                var newObject = Object.Instantiate(objExemplar);
                Put(objectType, newObject);
            }
        }

        public GameObject GetObject(ObjectType objectType)
        {
            if (IsObjectTypeRegistered(objectType))
            {
                return Get(objectType);
            }
            
            throw ObjectNotRegisteredException(objectType);
        }

        public void PutObject(ObjectType objectType, GameObject obj)
        {
            if (IsObjectTypeRegistered(objectType))
            {
                Put(objectType, obj);
                return;
            }

            throw ObjectNotRegisteredException(objectType);
        }

        public void Dispose()
        {
            Object.Destroy(_rootObject);
            _registeredObjects.Clear();
            foreach (var stack in _spawnedGameObjects.Values)
            {
                foreach (var obj in stack)
                {
                    Object.Destroy(obj);
                }
            }
            _spawnedGameObjects.Clear();
        }

        private void Put(ObjectType objectType, GameObject gameObject)
        {
            gameObject.transform.parent = _rootObject.transform;
            gameObject.SetActive(false);
            _spawnedGameObjects[objectType].Push(gameObject);
        }

        private GameObject Get(ObjectType objectType)
        {
            var stack = _spawnedGameObjects[objectType];
            GameObject obj;
            if (stack.Count > 0)
            {
                obj = stack.Pop();
                obj.transform.parent = null;
            }
            else
            {
                obj = Object.Instantiate(_registeredObjects[objectType]);
            }
            obj.SetActive(true);
            return obj;
        }

        private void CreateRootObject()
        {
            if (_rootObject == null)
            {
                _rootObject = new GameObject(RootObjectName);
            }
        }

        private void RegisterObjectType(ObjectType objectType, GameObject obj, int count)
        {
            _registeredObjects.Add(objectType, obj);
            var stack = new Stack<GameObject>(count);
            _spawnedGameObjects.Add(objectType, stack);
        }

        private bool IsObjectTypeRegistered(ObjectType objectType)
        {
            return _registeredObjects.ContainsKey(objectType);
        }

        private Exception ObjectNotRegisteredException(ObjectType objectType)
        {
            return new Exception($"Object {objectType.ToString()} not registered in object pool");
        }
    }
}
