using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace products
{
    public static class Util
    {
        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static int GetRandomNumber()
        {
            return rand.Next();
        }

        public static float GetRandomNumberF()
        {
            return (float)rand.NextDouble();
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
    }
}
