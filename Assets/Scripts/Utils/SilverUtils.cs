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
    }

}