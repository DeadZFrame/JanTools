using UnityEngine;

namespace Jan.Maths
{
    /// <summary>
    /// Provides extension methods for Vector2, Vector3, and Vector4 structs, enabling common
    /// operations such as modifying components, performing arithmetic, and utility
    /// functionality for geometric operations.
    /// </summary>
    public static class VectorExtensions 
    {
        /// <summary>
        /// Returns a new <see cref="Vector2"/> containing the x and y components of the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="v">The source <see cref="Vector3"/> to extract the x and y components from.</param>
        /// <returns>A <see cref="Vector2"/> containing the x and y components of the given <see cref="Vector3"/>.</returns>
        public static Vector2 xy (this Vector3 v) {
            return new Vector2 (v.x, v.y);
        }

        /// <summary>
        /// Creates a new Vector3 with the x-coordinate replaced by the provided value.
        /// </summary>
        /// <param name="v">The original Vector3.</param>
        /// <param name="x">The new x-coordinate value.</param>
        /// <returns>A new Vector3 with its x-coordinate set to the specified value and other components unchanged.</returns>
        public static Vector3 WithX (this Vector3 v, float x) {
            return new Vector3 (x, v.y, v.z);
        }

        /// Sets the y component of a Vector3 to the specified value and returns the modified vector.
        /// <param name="v">The original Vector3 to modify.</param>
        /// <param name="y">The new y value to set.</param>
        /// <return>The modified Vector3 with the updated y component.</return>
        public static Vector3 WithY (this Vector3 v, float y) {
            return new Vector3 (v.x, y, v.z);
        }

        /// <summary>
        /// Creates a new Vector3 with the same x and y values but with a specified z value.
        /// </summary>
        /// <param name="v">The original Vector3.</param>
        /// <param name="z">The new z value to replace in the vector.</param>
        /// <returns>A new Vector3 with the same x and y values, but z replaced with the specified value.</returns>
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// Returns a new <see cref="Vector3"/> with the x and y components replaced by specified values, and the z component unchanged.
        /// </summary>
        /// <param name="v">The original <see cref="Vector3"/>.</param>
        /// <param name="x">The new value for the x component.</param>
        /// <param name="y">The new value for the y component.</param>
        /// <returns>A new <see cref="Vector3"/> with updated x and y components.</returns>
        public static Vector3 WithXY(this Vector3 v, float x, float y)
        {
            return new Vector3(x, y, v.z);
        }

        /// <summary>
        /// Creates a new Vector3 with the X component unchanged and the Y and Z components replaced with the specified values.
        /// </summary>
        /// <param name="v">The original Vector3.</param>
        /// <param name="y">The value to set for the Y component.</param>
        /// <param name="z">The value to set for the Z component.</param>
        /// <returns>A new Vector3 with the modified Y and Z components.</returns>
        public static Vector3 WithYZ(this Vector3 v, float y, float z)
        {
            return new Vector3(v.x, y, z);
        }

        /// Returns a new Vector3 with the x and z components replaced with the specified values, while keeping the original y component unchanged.
        /// <param name="v">The original Vector3.</param>
        /// <param name="x">The new value for the x component.</param>
        /// <param name="z">The new value for the z component.</param>
        /// <returns>A new Vector3 with updated x and z components.</returns>
        public static Vector3 WithXZ(this Vector3 v, float x, float z)
        {
            return new Vector3(x, v.y, z);
        }

        /// <summary>
        /// Returns a new Vector3 instance with the specified x value,
        /// while retaining the y and z values of the original vector.
        /// </summary>
        /// <param name="v">The original Vector3.</param>
        /// <param name="x">The new x value to apply to the vector.</param>
        /// <returns>A new Vector3 with the updated x value.</returns>
        public static Vector2 WithX (this Vector2 v, float x) {
            return new Vector2 (x, v.y);
        }

        /// <summary>
        /// Returns a new <see cref="UnityEngine.Vector2"/> instance with the y component set to the specified value,
        /// while retaining the x component of the original vector.
        /// </summary>
        /// <param name="v">The original vector.</param>
        /// <param name="y">The new value for the y component.</param>
        /// <returns>A new <see cref="UnityEngine.Vector2"/> with the updated y component.</returns>
        public static Vector2 WithY (this Vector2 v, float y) {
            return new Vector2 (v.x, y);
        }

        /// <summary>
        /// Sets the z-coordinate of a Vector3 to a specified value.
        /// </summary>
        /// <param name="v">The original Vector3 instance.</param>
        /// <param name="z">The new z-coordinate value.</param>
        /// <returns>A new Vector3 with the specified z-coordinate value.</returns>
        public static Vector3 WithZ (this Vector2 v, float z) {
            return new Vector3 (v.x, v.y, z);
        }

        /// <summary>
        /// Adds the specified value to the X component of the given Vector3.
        /// </summary>
        /// <param name="v">The original Vector3 to modify.</param>
        /// <param name="x">The value to add to the X component.</param>
        /// <returns>A new Vector3 with the updated X component.</returns>
        public static Vector3 AddX (this ref Vector3 v, float x) {
            return v + Vector3.right * x;
        }

        /// <summary>
        /// Adds the specified value to the Y component of the given Vector3 and returns the resulting vector.
        /// </summary>
        /// <param name="v">The original Vector3.</param>
        /// <param name="y">The value to add to the Y component.</param>
        /// <returns>A new Vector3 with the Y component modified by the specified value.</returns>
        public static Vector3 AddY (this Vector3 v, float y) {
            return v + Vector3.up * y;
        }

        /// <summary>
        /// Adds a specified value to the z-component of the given Vector3.
        /// </summary>
        /// <param name="v">The original Vector3.</param>
        /// <param name="z">The value to add to the z-component.</param>
        /// <returns>A new Vector3 with the z-component modified by the specified value.</returns>
        public static Vector3 AddZ (this Vector3 v, float z) {
            return v + Vector3.forward * z;
        }

        /// <summary>
        /// Adds the specified value to the X component of the Vector2.
        /// </summary>
        /// <param name="v">The original Vector2.</param>
        /// <param name="x">The value to add to the X component.</param>
        /// <returns>A new Vector2 with the added X value while retaining the original Y component.</returns>
        public static Vector2 AddX (this Vector2 v, float x) {
            return v + Vector2.right * x;
        }

        /// <summary>
        /// Adds the specified value to the Y component of the given Vector2.
        /// </summary>
        /// <param name="v">The original Vector2 instance to modify.</param>
        /// <param name="y">The value to add to the Y component.</param>
        /// <returns>A new Vector2 with the modified Y component.</returns>
        public static Vector2 AddY (this Vector2 v, float y) {
            return v + Vector2.up * y;
        }

        /// <summary>
        /// Calculates the area of a <see cref="UnityEngine.Vector3"/> by multiplying its x, y, and z components.
        /// </summary>
        /// <param name="v">The <see cref="UnityEngine.Vector3"/> vector to calculate the area for.</param>
        /// <returns>The area calculated as x * y * z.</returns>
        public static float Area(this Vector3 v) => v.x * v.y * v.z;

        /// <summary>
        /// Calculates the area of a 2D vector by multiplying its x and y components.
        /// </summary>
        /// <param name="v">The Vector2 instance used for area calculation.</param>
        /// <returns>The calculated area as a float.</returns>
        public static float Area(this Vector2 v) => v.x * v.y;

        /// Rotates a given position around a pivot point by applying a specified quaternion rotation.
        /// Updates the original position to the new rotated position.
        /// <param name="originalPosition">Reference to the original position to be rotated.</param>
        /// <param name="pivot">The point around which the position should be rotated.</param>
        /// <param name="rotation">The quaternion rotation to apply.</param>
        /// <returns>The new position after rotation around the pivot.</returns>
        public static Vector3 RotatePositionAroundPivot(this ref Vector3 originalPosition, Vector3 pivot, Quaternion rotation)
        {
            Vector3 direction = originalPosition - pivot;
            Vector3 rotatedDirection = rotation * direction;
            Vector3 newPosition = pivot + rotatedDirection;
            originalPosition = newPosition;
            return newPosition;
        }

        /// Converts a Vector2 to a Vector3 by preserving the x and y components
        /// and assigning the z component a value of 0.
        /// <param name="v">The Vector2 instance to convert.</param>
        /// <returns>A new Vector3 instance where x and z are mapped from
        /// the Vector2's x and y, respectively, and z is set to 0.</returns>
        public static Vector3 ConvertToVector3 (this Vector2 v) => new Vector3(v.x, 0, v.y); 

        // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
        // point - the point to find nearest on line for
        /// <summary>
        /// Calculates the nearest point on an axis, defined by its direction, to a given point in space.
        /// </summary>
        /// <param name="axisDirection">A unit vector representing the direction of the axis. If not normalized, it will be normalized internally.</param>
        /// <param name="point">The point in space for which the nearest position on the axis will be calculated.</param>
        /// <param name="isNormalized">Specifies whether the <paramref name="axisDirection"/> is already normalized. Defaults to false.</param>
        /// <returns>A <see cref="Vector3"/> representing the nearest point on the axis to the given <paramref name="point"/>.</returns>
        public static Vector3 NearestPointOnAxis (this Vector3 axisDirection, Vector3 point, bool isNormalized = false) {
            if (!isNormalized) axisDirection.Normalize ();
            var d = Vector3.Dot (point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - unit vector in direction of line
        // pointOnLine - a point on the line (allowing us to define an actual line in space)
        // point - the point to find nearest on line for
        /// <summary>
        /// Calculates the nearest point on a line to a specified point in space.
        /// </summary>
        /// <param name="lineDirection">The direction vector of the line. This should ideally be normalized. If not, set <paramref name="isNormalized"/> to false.</param>
        /// <param name="point">The point for which the nearest position on the line is to be calculated.</param>
        /// <param name="pointOnLine">A known point on the line, used to define the position of the line in space.</param>
        /// <param name="isNormalized">Indicates whether the <paramref name="lineDirection"/> is already normalized. Defaults to false.</param>
        /// <returns>The nearest point on the line to the given point.</returns>
        public static Vector3 NearestPointOnLine (
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false) {
            if (!isNormalized) lineDirection.Normalize ();
            var d = Vector3.Dot (point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }

        /// Converts a given Euler angles vector to a quaternion representation.
        /// The Euler angles are expected to be in degrees.
        /// <param name="p">The Vector3 representing the Euler angles in degrees (x: pitch, y: yaw, z: roll).</param>
        /// <returns>A Vector4 representing the quaternion (x, y, z, w).</returns>
        public static Vector4 EulerToQuaternion (Vector3 p) {
            p.x *= Mathf.Deg2Rad;
            p.y *= Mathf.Deg2Rad;
            p.z *= Mathf.Deg2Rad;
            Vector4 q;
            float cy = Mathf.Cos (p.z * 0.5f);
            float sy = Mathf.Sin (p.z * 0.5f);
            float cr = Mathf.Cos (p.y * 0.5f);
            float sr = Mathf.Sin (p.y * 0.5f);
            float cp = Mathf.Cos (p.x * 0.5f);
            float sp = Mathf.Sin (p.x * 0.5f);
            q.w = cy * cr * cp + sy * sr * sp;
            q.x = cy * cr * sp + sy * sr * cp;
            q.y = cy * sr * cp - sy * cr * sp;
            q.z = sy * cr * cp - cy * sr * sp;
            return q;
        }

        /// <summary>
        /// Converts a quaternion represented as a Vector4 to Euler angles.
        /// </summary>
        /// <param name="p">The quaternion, represented as a Vector4 (x, y, z, w).</param>
        /// <returns>A Vector3 representing the Euler angles (in degrees) corresponding to the input quaternion.</returns>
        public static Vector3 QuaternionToEuler (Vector4 p) {
            Vector3 v;
            Vector4 q = new Vector4 (p.w, p.z, p.x, p.y);
            v.y = Mathf.Atan2 (2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));
            v.x = Mathf.Asin (2f * (q.x * q.z - q.w * q.y));
            v.z = Mathf.Atan2 (2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));
            v *= Mathf.Rad2Deg;
            v.x = v.x > 360 ? v.x - 360 : v.x;
            v.x = v.x < 0 ? v.x + 360 : v.x;
            v.y = v.y > 360 ? v.y - 360 : v.y;
            v.y = v.y < 0 ? v.y + 360 : v.y;
            v.z = v.z > 360 ? v.z - 360 : v.z;
            v.z = v.z < 0 ? v.z + 360 : v.z;
            return v;
        }

        /// <summary>
        /// Clamps an integer value between a specified minimum and maximum value.
        /// </summary>
        /// <param name="val">The integer value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped integer value, ensuring it does not fall below the minimum or exceed the maximum.</returns>
        public static int Clamp(this int val, int min, int max)
        {
            return Mathf.Clamp(val, min, max);
        }

        /// <summary>
        /// Finds and returns the nearest element of type T (MonoBehaviour) in the provided array based on the given position.
        /// </summary>
        /// <typeparam name="T">The type of MonoBehaviour to search for in the array.</typeparam>
        /// <param name="array">An array of elements of type T to search.</param>
        /// <param name="currentPos">The current position to calculate the distance from.</param>
        /// <returns>The nearest element of type T from the array, or null if the array is empty.</returns>
        public static T GetNearest<T>(T[] array, Vector3 currentPos) where T : MonoBehaviour
        {
            float minDistance = float.MaxValue;
            T nearest = null;

            for (int i = 0; i < array.Length; i++)
            {
                var element = array[i];

                var dist = (element.transform.position - currentPos).sqrMagnitude;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = element;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Converts a Vector2Int to a Vector2 by casting the integer components to floats.
        /// </summary>
        /// <param name="v">The Vector2Int to convert.</param>
        /// <returns>A Vector2 with the same component values as floats.</returns>
        public static Vector2 ToVector2(this Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// Converts a Vector3Int to a Vector3 by casting the integer components to floats.
        /// </summary>
        /// <param name="v">The Vector3Int to convert.</param>
        /// <returns>A Vector3 with the same component values as floats.</returns>
        public static Vector3 ToVector3(this Vector3Int v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        /// <summary>
        /// Generates a random point on the unit circle in 2D space.
        /// </summary>
        /// <returns>A Vector2 representing a point on the unit circle.</returns>
        public static Vector2 GetRandomPointOnUnitCircle()
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2); // Random angle in radians
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // Point on the circle
        }

        /// <summary>
        /// Generates a random point on a unit circle oriented by the specified normal vector.
        /// </summary>
        /// <param name="normal">The normal vector determining the orientation of the circle.</param>
        /// <returns>A Vector3 representing a point on the oriented unit circle.</returns>
        public static Vector3 GetRandomPointOnUnitCircle(Vector3 normal)
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2); // Random angle in radians

            // Create a coordinate system where normal is one of the axes
            Vector3 tangent = Vector3.Cross(normal, Vector3.up);
            if (tangent.magnitude < 0.0001f) // If normal is parallel to up
                tangent = Vector3.Cross(normal, Vector3.right);

            tangent.Normalize();
            Vector3 bitangent = Vector3.Cross(normal, tangent).normalized;

            // Calculate point on the circle using the tangent and bitangent as the circle plane
            return tangent * Mathf.Cos(angle) + bitangent * Mathf.Sin(angle);
        }

        /// <summary>
        /// Generates a random point on the surface of a sphere in 3D space.
        /// </summary>
        /// <param name="sourcePosition">The center position of the sphere.</param>
        /// <param name="radius">The radius of the sphere. Default is 1.</param>
        /// <returns>A Vector3 representing a point on the sphere's surface.</returns>
        public static Vector3 GetRandomPointOnUnitSphere(Vector3 sourcePosition, float radius = 1f)
        {
            Vector3 randomPoint = UnityEngine.Random.onUnitSphere * radius; // Random point on the sphere surface
            return sourcePosition + randomPoint; // Offset by the source position
        }
    }
}