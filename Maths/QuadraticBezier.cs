using UnityEngine;

namespace Jan.Maths
{
    public struct QuadraticBezier
    {
        private Vector3 _start;
        private Vector3 _control;
        private Vector3 _end;
        
        public QuadraticBezier(Vector3 start, Vector3 control, Vector3 end)
        {
            _start = start;
            _control = control;
            _end = end;
        }
        
        public Vector3 GetQuadraticBezierPoint(float t)
        {
            return Mathf.Pow(1 - t, 2) * _start + 
                   2 * (1 - t) * t * _control + 
                   Mathf.Pow(t, 2) * _end;
        }

        public void UpdatePoints(Vector3 start, Vector3 control, Vector3 end)
        {
            _start = start;
            _control = control;
            _end = end;
        }
    }
}