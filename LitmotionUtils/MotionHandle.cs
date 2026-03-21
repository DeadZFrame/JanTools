using LitMotion;

namespace Jan.Core
{
    public struct Motion
    {
        private MotionHandle _handle;

        public Motion(MotionHandle handle)
        {
            _handle = handle;
        }

        public void Cancel()
        {
            _handle.SafeCancel();
        }
    }
}