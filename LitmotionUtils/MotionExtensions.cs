using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Jan.Core
{
    public static class MotionExtensions
    {
        public static void FloatMotion(this IMotion motion, float current, float target, float duration, Ease ease = Ease.Linear)
        {
            if(motion == null)
            {
                throw new System.ArgumentNullException(nameof(motion));
            }

            motion.MotionHandle.Cancel();

            var motionHandle = LMotion.Create(current, target, duration)
                .WithEase(ease.ConvertToLitEase())
                .Bind(motion.SetFloat);

            motion.MotionHandle = new Motion(motionHandle);
        }

        public static Motion LitMove(this Transform transform, Vector3 targetPosition, float duration, Ease ease = Ease.Linear, bool localSpace = false)
        {
            if(!localSpace)
            {
                var motionHandle = LMotion.Create(transform.position, targetPosition, duration)
                    .WithEase(ease.ConvertToLitEase())
                    .BindToPosition(transform);

                return new Motion(motionHandle);
            }
            else
            {
                var motionHandle = LMotion.Create(transform.localPosition, targetPosition, duration)
                    .WithEase(ease.ConvertToLitEase())
                    .BindToLocalPosition(transform);

                return new Motion(motionHandle);
            }
        }

        public static Motion LitRotate(this Transform transform, Quaternion targetRotation, float duration, Ease ease = Ease.Linear, bool localSpace = false)
        {
            if(!localSpace)
            {
                var motionHandle = LMotion.Create(transform.rotation, targetRotation, duration)
                    .WithEase(ease.ConvertToLitEase())
                    .BindToRotation(transform);

            return new Motion(motionHandle);
            }
            else
            {
                var motionHandle = LMotion.Create(transform.localRotation, targetRotation, duration)
                    .WithEase(ease.ConvertToLitEase())
                    .BindToLocalRotation(transform);

                return new Motion(motionHandle);
            }
        }

        public static void SafeCancel(this MotionHandle handle)
        {
            if (handle.IsActive())
            {
                handle.Cancel();
            }
        }

        private static LitMotion.Ease ConvertToLitEase(this Ease ease)
        {
            return ease switch
            {
                Ease.Linear => LitMotion.Ease.Linear,
                Ease.InSine => LitMotion.Ease.InSine,
                Ease.OutSine => LitMotion.Ease.OutSine,
                Ease.InOutSine => LitMotion.Ease.InOutSine,
                Ease.InQuad => LitMotion.Ease.InQuad,
                Ease.OutQuad => LitMotion.Ease.OutQuad,
                Ease.InOutQuad => LitMotion.Ease.InOutQuad,
                Ease.InCubic => LitMotion.Ease.InCubic,
                Ease.OutCubic => LitMotion.Ease.OutCubic,
                Ease.InOutCubic => LitMotion.Ease.InOutCubic,
                Ease.InQuart => LitMotion.Ease.InQuart,
                Ease.OutQuart => LitMotion.Ease.OutQuart,
                Ease.InOutQuart => LitMotion.Ease.InOutQuart,
                Ease.InQuint => LitMotion.Ease.InQuint,
                Ease.OutQuint => LitMotion.Ease.OutQuint,
                Ease.InOutQuint => LitMotion.Ease.InOutQuint,
                Ease.InExpo => LitMotion.Ease.InExpo,
                Ease.OutExpo => LitMotion.Ease.OutExpo,
                Ease.InOutExpo => LitMotion.Ease.InOutExpo,
                Ease.InCirc => LitMotion.Ease.InCirc,
                Ease.OutCirc => LitMotion.Ease.OutCirc,
                Ease.InOutCirc => LitMotion.Ease.InOutCirc,
                Ease.InBack => LitMotion.Ease.InBack,
                Ease.OutBack => LitMotion.Ease.OutBack,
                Ease.InOutBack => LitMotion.Ease.InOutBack,
                Ease.InElastic => LitMotion.Ease.InElastic,
                Ease.OutElastic => LitMotion.Ease.OutElastic,
                Ease.InOutElastic => LitMotion.Ease.InOutElastic,
                Ease.InBounce => LitMotion.Ease.InBounce,
                Ease.OutBounce => LitMotion.Ease.OutBounce, 
                Ease.InOutBounce => LitMotion.Ease.InOutBounce,
                _ => LitMotion.Ease.Linear
            };
        }
    }
}