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
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }
        public static float Vec2toDeg(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;
            return Vec2toDeg(direction);
        }

        public static Vector2 DegtoVec2(float angleZ)
        {
            /*//some statements for 90, 180 , 270 to avoid accuracy loss from float
            if(Mathf.Approximately(Mathf.Abs(angleZ % 90), 0))
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
            Quaternion rotation = Quaternion.Euler(0, 0, angleZ);
            return rotation * new Vector2(0, 1); 
        }*/

        public static float Normalizeto360(float angle)
        {
            if (angle >= 360)
            {
                angle %= 360;
            }
            else if (angle < 0)
            {
                angle = Mathf.Abs(angle);
                angle %= 360;
                angle = 360 - angle;
            }
            return angle;
        }
    }

}