using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public static class Util
    {
        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static int GetRandomDistribution(int number, int delta)
        {
            int randNum = number + rand.Next(-delta, delta);
            return randNum > 0 ? randNum : 0;
        }

        public static float GetRandomDistribution(float number, float delta)
        {
            return number + (float)rand.NextDouble() * delta * ((float)rand.NextDouble() > 0.5f ? 1 : -1);
        }

        public static PointF GetCenter(float x, float y, float w, float h)
        {
            return new PointF(x + w / 2, y + h / 2);
        }

        public static PointF GetCenter(RectangleF rec)
        {
            return GetCenter(rec.X, rec.Y, rec.Width, rec.Height);
        }

        public static bool CheckCollision(RectangleF a, RectangleF b)
        {
            return !((a.X + a.Width < b.Y || a.X > b.X + b.Width)
                && (a.Y + a.Height < b.Y || a.Y > b.Y + b.Height));
        }

        public static bool CheckIntersection(RectangleF a, RectangleF b) // check a in b?
        {
            return ((a.X >= b.X) && (a.X + a.Width <= b.X + b.Width) &&
                (a.Y >= b.Y) && (a.Y + a.Height <= b.Y + b.Height));
        }

        public static float Distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public static float Distance(RectangleF a, PointF b)
        {
            return Distance(new PointF(a.X + a.Width / 2, a.Y + a.Height / 2), b);
        }

        public static void ValidateInterval(int left, int right, int value, string message)
        {
            if (!(left <= value && value <= right))
            {
                throw new Exception(message);
            }
        }
    }
}
