﻿using System;
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

        //Return positive valure from interval [number - delta; number + delta]
        public static int GetRandomDistribution(int number, int delta)
        {
            int randNum = number + rand.Next(-delta, delta);
            return randNum > 0 ? randNum : 0;
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

        //Throw Exception with message if valute not into interval [left; right]
        //Need to validate inputs
        public static void ValidateInterval(int left, int right, int value, string message)
        {
            if (!(left <= value && value <= right))
            {
                throw new Exception(message);
            }
        }
    }
}