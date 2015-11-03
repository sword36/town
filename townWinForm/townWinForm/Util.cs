using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace townWinForm
{
    public static class Util
    {
        public delegate void CameraUpdatingHandler(float dx, float dy);
        public static event CameraUpdatingHandler UpdateCamera;

        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static int GetRandomNumber()
        {
            return rand.Next();
        }

        public static int GetRandomNumber(int maxValue)
        {
            return rand.Next(maxValue);
        }

        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }

        //Return positive valure from interval [number - delta; number + delta]
        public static int GetRandomDistribution(int number, int delta)
        {
            int randNum = number + rand.Next(-delta, delta);
            return randNum > 0 ? randNum : 0;
        }

        public static int GetRandomFromInterval(int a, int b)
        {
            return rand.Next(a, b);
        }

        //Return float value(positive and negative) from interval [number - delta; number + delta]
        public static float GetRandomDistribution(float number, float delta)
        {
            return number + (float)rand.NextDouble() * delta * ((float)rand.NextDouble() > 0.5f ? 1 : -1);
        }

        //Return PointF whitch is in center of rectangle with left corner (x, y), width - w, height - h
        public static PointF GetCenter(float x, float y, float w, float h)
        {
            return new PointF(x + w / 2, y + h / 2);
        }

        public static PointF GetCenter(RectangleF rec)
        {
            return GetCenter(rec.X, rec.Y, rec.Width, rec.Height);
        }

        //Return true if Rectangle a and b have collisions
        public static bool CheckCollision(RectangleF a, RectangleF b)
        {
            return !((a.X + a.Width < b.Y || a.X > b.X + b.Width)
                && (a.Y + a.Height < b.Y || a.Y > b.Y + b.Height));
        }

        //Return true if Rectagle a into Rectangle b (a is smaller than b)
        public static bool CheckIntersection(RectangleF a, RectangleF b) 
        {
            return ((a.X >= b.X) && (a.X + a.Width <= b.X + b.Width) &&
                (a.Y >= b.Y) && (a.Y + a.Height <= b.Y + b.Height));
        }

        //Return float value of distance between Points a and b
        public static float Distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        //Return float value of distance between center of Rectangle a and Point b
        public static float Distance(RectangleF a, PointF b)
        {
            return Distance(new PointF(a.X + a.Width / 2, a.Y + a.Height / 2), b);
        }

        //Returns a random point in circle 
        /// <summary>
        /// Returns a random point in circle 
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle's radius</param>
        /// <returns>PointF</returns>
        public static PointF GetRandomPointInCircle(PointF center, float radius)
        {
            double t = 2 * Math.PI * rand.NextDouble();
            double u = rand.NextDouble() * rand.NextDouble();
            double r = u > 1 ? 2 - u : u;
            float resx = center.X + (float)(radius * r * Math.Cos(t));
            float resy = center.Y + (float)(radius * r * Math.Sin(t));
            PointF result = new PointF(resx, resy);
            return result;
        }


        //Throw Exception with message if valute not into interval [left; right]
        //Need to validate inputs
        public static void ValidateInterval(int left, int right, int value, string message)
        {
            if (!(left <= value && value <= right))
            {
                throw new Exception(message);
            }
        }

        //Changes dx and dy values
        /// <summary>
        /// //Changes dx and dy values
        /// </summary>
        /// <param name="MousePoint">Current mouse position</param>
        /// <param name="Width">Window width</param>
        /// <param name="Height">Window height</param>
        /// <param name="dt">dt</param>
        public static void Move(Point MousePoint, int Width, int Height, long dt)
        {
            if (MousePoint.X <= 10)
            {
                Config.dx += Config.ScrollSpeed / dt;
            }

            if (MousePoint.X >= Width - 10)
            {
                Config.dx -= Config.ScrollSpeed / dt;
            }

            if (MousePoint.Y <= 10)
            {
                Config.dy += Config.ScrollSpeed / dt;
            }

            if (MousePoint.Y >= Height - 10)
            {
                Config.dy -= Config.ScrollSpeed / dt;
            }

            if (UpdateCamera != null)
                UpdateCamera(Config.dx, Config.dy);
        }

        //Returs random color
        /// <summary>
        /// Returs random color
        /// </summary>
        /// <returns>Random color</returns>
        public static Color GetRandomColor()
        {
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        //Returns random color, using seed
        /// <summary>
        /// Returns random color
        /// </summary>
        /// <param name="seed">Seed for Random function</param>
        /// <returns></returns>
        public static Color GetRandomColor(int seed)
        {
            if (seed == 1) return Color.FromArgb(215, 215, 215);
            else return Color.FromArgb(40, 40, 40);
            Random rnd = new Random(seed);
            return Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
        }

        //Checks the point of getting into the area of the screen
        public static bool CheckPoint(PointF point)
        {
            if (point.X + Config.TileSize < 0)
            {
                return false;
            }

            if (point.Y + Config.TileSize < 0)
            {
                return false;
            }

            if (point.X >= SystemInformation.PrimaryMonitorSize.Width)
            {
                return false;
            }

            if (point.Y >= SystemInformation.PrimaryMonitorSize.Height)
            {
                return false;
            }

            return true;
        }

        public static bool CheckPoint(Point point)
        {
            if (point.X < 0)
            {
                return false;
            }

            if (point.Y < 0)
            {
                return false;
            }

            if (point.X >= SystemInformation.PrimaryMonitorSize.Width)
            {
                return false;
            }

            if (point.Y >= SystemInformation.PrimaryMonitorSize.Height)
            {
                return false;
            }

            return true;
        }

        public static bool IsInList(List<int> l, int val)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == val) return true;
            }
            return false;
        }

        public static PointF ConvertIndexToInt(PointF index)
        {
            return new PointF(index.X * Config.tileSize + Config.dx, index.Y * Config.tileSize + Config.dy);
        }
    }
}
