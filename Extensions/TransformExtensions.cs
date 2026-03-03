using System.Collections.Generic;
using Jan.Maths;
using Jan.Tasks;
using UnityEngine;

namespace Jan.Core
{
    public static class TransformExtensions
    {
        private static Dictionary<Transform, Cts> cancellationTokenSources = new Dictionary<Transform, Cts>();
        public static Cts MoveBezier(this Transform target, Transform to, float altitude, float duration)
        {
            if (cancellationTokenSources.TryGetValue(target, out var cts))
            {
                cts.SafeCancel();
                cancellationTokenSources.Remove(target);
            }

            float timePassed = 0;
            Vector3 initialPosition = target.position;
            Vector3 midpoint = (initialPosition + to.position) / 2 + Vector3.up * altitude;

            var bezier = new QuadraticBezier(initialPosition, midpoint, to.position);

            void UpdatePosition()
            {
                midpoint = (initialPosition + to.position) / 2 + Vector3.up * 3;
                bezier.UpdatePoints(initialPosition, midpoint, to.position);
                target.transform.position = bezier.GetQuadraticBezierPoint(timePassed);
                target.transform.rotation = Quaternion.Slerp(target.transform.rotation, to.rotation, duration);
                timePassed += Time.deltaTime / duration;
                timePassed = Mathf.Clamp01(timePassed);
            }

            void Cancel() => target.transform.position = to.position;

            cts = Timed.CallContinuesFor(duration, UpdatePosition, target.gameObject);
            cts.OnCompleted(Cancel);

            cancellationTokenSources.Add(target, cts);

            return cts;
        }

        public static Cts MoveBezier(this Transform target, Vector3 to, Quaternion targetRotation, float altitude, float duration)
        {
            if (cancellationTokenSources.TryGetValue(target, out var cts))
            {
                cts.SafeCancel();
                cancellationTokenSources.Remove(target);
            }

            float timePassed = 0;
            Vector3 initialPosition = target.position;
            Vector3 midpoint = (initialPosition + to) / 2 + Vector3.up * altitude;
            Quaternion initialRotation = target.rotation;

            var bezier = new QuadraticBezier(initialPosition, midpoint, to);

            void UpdatePosition()
            {
                target.transform.position = bezier.GetQuadraticBezierPoint(timePassed);
                target.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timePassed);

                timePassed += Time.deltaTime / duration;
                timePassed = Mathf.Clamp01(timePassed);
            }

            void Cancel()
            {
                target.transform.position = to;
                target.transform.rotation = targetRotation;
            }

            cts = Timed.CallContinuesFor(duration, UpdatePosition, target.gameObject);
            cts.OnCompleted(Cancel);

            cancellationTokenSources.Add(target, cts);

            return cts;
        }
    }
}