using LitMotion;

namespace Jan.Core
{
    public static class MotionExtensions
    {
        public static void FloatMotion(this IMotion motion, float current,float target, float duration, Ease ease = Ease.Linear)
        {
            if(motion == null)
            {
                throw new System.ArgumentNullException(nameof(motion));
            }

            motion.MotionHandle.SafeCancel();

            motion.MotionHandle = LMotion.Create(current, target, duration)
                .WithEase(ease)
                .Bind(motion.SetFloat);
        }

        public static void SafeCancel(this MotionHandle handle)
        {
            if (handle.IsActive())
            {
                handle.Cancel();
            }
        }
    }
}