using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroiderCore
{
    public sealed class UpdateProvider : MonoBehaviour
    {
        private readonly List<Action<float>> _subscribersUpdate = new();
        private readonly List<Action<float>> _subscribersLateUpdate = new();
        private bool _executingUpdate;
        private bool _executingLateUpdate;
        private int _executeUpdateIndex;
        private int _executeLateUpdateIndex;
        
        private void Update()
        {
            _executingUpdate = true;
            for (_executeUpdateIndex = 0; _executeUpdateIndex < _subscribersUpdate.Count; _executeUpdateIndex++)
            {
                var subscriber = _subscribersUpdate[_executeUpdateIndex];
                subscriber?.Invoke(Time.deltaTime);
            }
            _executingUpdate = false;
        }
        
        private void LateUpdate()
        {
            _executingLateUpdate = true;
            for (_executeLateUpdateIndex = 0; _executeLateUpdateIndex < _subscribersLateUpdate.Count; _executeLateUpdateIndex++)
            {
                var subscriber = _subscribersLateUpdate[_executeLateUpdateIndex];
                subscriber?.Invoke(Time.deltaTime);
            }
            _executingLateUpdate = false;
        }

        public void AddListenerUpdate(Action<float> callback)
        {
            if(!_subscribersUpdate.Contains(callback))
                _subscribersUpdate.Add(callback);
        }

        public void RemoveListenerUpdate(Action<float> callback)
        {
            var index = _subscribersUpdate.IndexOf(callback);
            if (index < 0) 
                return;
            
            _subscribersUpdate.RemoveAt(index);
            if(_executingUpdate && index <= _executeUpdateIndex)
            {
                _executeUpdateIndex--;
            }
        }
        
        public void AddListenerLateUpdate(Action<float> callback)
        {
            if(!_subscribersLateUpdate.Contains(callback))
                _subscribersLateUpdate.Add(callback);
        }

        public void RemoveListenerLateUpdate(Action<float> callback)
        {
            var index = _subscribersLateUpdate.IndexOf(callback);
            if (index < 0) 
                return;
            
            _subscribersLateUpdate.RemoveAt(index);
            if(_executingLateUpdate && index <= _executeLateUpdateIndex)
            {
                _executeLateUpdateIndex--;
            }
        }
    }
}