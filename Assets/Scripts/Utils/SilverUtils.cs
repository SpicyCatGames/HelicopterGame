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
        public static Vector3 GetEditorRotations(Transform transform)//please ignore this as well
        {
            Vector3 angle = transform.eulerAngles;
            float x = angle.x;
            float y = angle.y;
            float z = angle.z;

            if (Vector3.Dot(transform.up, Vector3.up) >= 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = angle.x;
                }
                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = angle.x - 360f;
                }
            }
            if (Vector3.Dot(transform.up, Vector3.up) < 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = 180 - angle.x;
                }
                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = 180 - angle.x;
                }
            }

            if (angle.y > 180)
            {
                y = angle.y - 360f;
            }

            if (angle.z > 180)
            {
                z = angle.z - 360f;
            }
            return new Vector3(x, y, z);
        }
    }

}