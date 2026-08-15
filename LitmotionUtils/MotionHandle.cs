using System;
using Jan.Tasks;
using LitMotion;

namespace Jan.Core
{
    public struct Motion
    {
        private MotionHandle _handle;
        private Cts _cts;

        public Motion(MotionHandle handle)
        {
            _handle = handle;
            _cts = null;
        }

        public readonly void Cancel()
        {
            _handle.SafeCancel();
            _cts?.SafeCancel();
        }

        public readonly void Complete()
        {
            _handle.Complete();
            _cts?.SafeCancel();
        }

        public void OnCompleted(Action callback)
        {
            if(_handle.IsActive())
            {
                _cts = Timed.CallDelayed(_handle.Duration, callback);
            }
        }
    }
}