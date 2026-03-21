using LitMotion;

namespace Jan.Core
{
    public interface IMotion
    {
        public Motion MotionHandle { get; set;}
        void SetFloat(float value);
    }
}