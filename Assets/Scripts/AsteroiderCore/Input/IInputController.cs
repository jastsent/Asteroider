using System;

namespace AsteroiderCore.Input
{
    public interface IInputController
    {
        public event Action OnForward;
        public event Action OnLeft;
        public event Action OnRight;
        public event Action OnFire;
        public event Action OnLaser;
        public bool IsForward { get; }
        public bool IsLeft { get; }
        public bool IsRight { get; }
        public bool IsFire { get; }
        public bool IsLaser { get; }
        public void Initialize();
        public void EnableInput();
        public void DisableInput();
    }
}