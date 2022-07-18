using System;

namespace AsteroiderCore
{
    public sealed class PauseManager
    {
        public bool IsPaused { get; private set; }

        public event Action OnPause;
        public event Action OnUnpause;

        public void Pause()
        {
            if (!IsPaused)
            {
                IsPaused = true;
                OnPause?.Invoke();
            }
        }

        public void Unpause()
        {
            if (IsPaused)
            {
                IsPaused = false;
                OnUnpause?.Invoke();
            }
        }
    }
}