using System;
using UnityEngine.InputSystem;

namespace AsteroiderCore.Input
{
    public class InputController : IInputController
    {
        public event Action OnForward;
        public event Action OnLeft;
        public event Action OnRight;
        public event Action OnFire;
        public event Action OnLaser;
        public bool IsForward => _inputAsset.Player.Forward.IsPressed();
        public bool IsLeft => _inputAsset.Player.Left.IsPressed();
        public bool IsRight => _inputAsset.Player.Right.IsPressed();
        public bool IsFire => _inputAsset.Player.Fire.IsPressed();
        public bool IsLaser => _inputAsset.Player.Laser.IsPressed();
        public bool Initialized => _initialized;
        
        private readonly AsteroiderInputActionAsset _inputAsset;
        private bool _initialized;

        public InputController(AsteroiderInputActionAsset inputAsset)
        {
            _inputAsset = inputAsset;
        }

        public void Initialize()
        {
            if(_initialized)
                return;
            
            _inputAsset.Player.Forward.performed += OnForwardPerformed;
            _inputAsset.Player.Left.performed += OnLeftPerformed;
            _inputAsset.Player.Right.performed += OnRightPerformed;
            _inputAsset.Player.Fire.performed += OnFirePerformed;
            _inputAsset.Player.Laser.performed += OnLaserPerformed;
            _initialized = true;
        }

        public void EnableInput()
        {
            _inputAsset.Enable();
        }

        public void DisableInput()
        {
            _inputAsset.Disable();
        }

        private void InvokeAction(Action action)
        {
            action?.Invoke();
        }

        private void OnForwardPerformed(InputAction.CallbackContext obj)
        {
            InvokeAction(OnForward);
        }

        private void OnLeftPerformed(InputAction.CallbackContext obj)
        {
            InvokeAction(OnLeft);
        }

        private void OnRightPerformed(InputAction.CallbackContext obj)
        {
            InvokeAction(OnRight);
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            InvokeAction(OnFire);
        }

        private void OnLaserPerformed(InputAction.CallbackContext obj)
        {
            InvokeAction(OnLaser);
        }
    }
}