using UnityEngine;

namespace SilverUtils.Angle
{
    public static class Degrees
    {
        public static float From360to180(float angleInDegrees)
        {
            return (angleInDegrees > 180) ? (angleInDegrees - 360) : angleInDegrees;
        }
        public static float From180to360(float angleInDegrees)
        {
            return (angleInDegrees < 0) ? (angleInDegrees + 360) : angleInDegrees;
        }

        public static float Vec2toDeg(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return (angle < 0) ? (angle + 360) : angle;
        }
        public static float Vec2toDeg(Vector2 from, Vector2 to) => Vec2toDeg(to - from);

        /// <summary>
        /// Values may be a number very close but not enough for Mathf.Approximately()
        /// </summary>
        public static Vector2 DegtoVec2(float angleZ)
        {
            //some statements for 90, 180 , 270 to avoid accuracy loss from float
            /*if(Mathf.Approximately(Mathf.Abs(angleZ % 90), 0))
            {
                angleZ = Normalizeto360(angleZ);
                if (Mathf.Approximately(angleZ, 90)) return new Vector2(0, 1);
                if (Mathf.Approximately(angleZ, 180)) return new Vector2(-1, 0);
                if (Mathf.Approximately(angleZ, 270)) return new Vector2(0, -1);
            }*/
            float angleRad = angleZ * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
        /*public static Vector2 DegtoVec2(float angleZ)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, -angleZ);
            return rotation * new Vector2(0, 1); 
        }*/

        public static float Normalizeto360(float angle)
        {
            if (angle >= 360)
            {
                return angle % 360;
            }
            if (angle < 0)
            {
                /*angle = Mathf.Abs(angle);
                angle %= 360;
                angle = 360 - angle;*/
                return 360 - (Mathf.Abs(angle) % 360);
            }
            return angle;
        }

        /// <summary>
        /// Returns -1, 1 , whichever to add to get to target rotation quickest
        /// Values must be between 0 and 360
        /// </summary>
        public static int RotationDirection(float fromEuler, float toEuler)
        {
            //if difference between the two rotations is more than 180, the closest way to the target is through 360 wraparound
            if(Mathf.Abs(fromEuler - toEuler) < 180)
            {
                return (toEuler > fromEuler) ? 1 : -1;
            }
            return (toEuler > fromEuler) ? -1 : 1;
        }

        /// <summary>
        /// Clamp while taking into account the 360 wraparound
        /// </summary>
        public static float ClampRotation(float eulerAngle, float fromEuler, float toEuler)
        {
            eulerAngle = Normalizeto360(eulerAngle);

            if (fromEuler < toEuler)
            {
                return Mathf.Clamp(eulerAngle, fromEuler, toEuler);
            }

            //else if from is greater than to
            //if it's between the two numbers, clamp to the closest one.
            if ((eulerAngle < fromEuler) && (eulerAngle > toEuler))
            {
                return ((fromEuler - eulerAngle) < (eulerAngle - toEuler)) ? fromEuler : toEuler;
            }

            return eulerAngle;
        }

        /// <summary>
        /// Values must be between 0 and 359.99999999999999999
        /// </summary>
        public static bool RotationIsBetween(float eulerAngle, float minEuler, float maxEuler)
        {
            if(RotationDirection(eulerAngle, minEuler) == -1 && RotationDirection(eulerAngle, maxEuler) == 1)
            {
                return true;
            }
            return false;
        }
    }
}

namespace SilverUtils
{
    public static class Misc{
        public static float GetPPU(Camera cam)
        {
            return (Screen.height / 2) / cam.orthographicSize;
        }
    }
}