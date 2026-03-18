using LitMotion;

namespace Jan.Core
{
    public interface IMotion
    {
        public MotionHandle MotionHandle { get; set;}
        void SetFloat(float value);
    }
}