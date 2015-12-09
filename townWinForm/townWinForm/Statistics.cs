using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public class Statistics
    {
        private List<int> time;
        private List<double> values;

        private static int maxValue;

        public static int MaxValue
        {
            get { return maxValue; }
        }

        public List<int> Time
        {
            get { return time; }
        }

        public List<double> Values
        {
            get { return values; }
        }

        private void updateTime()
        {
            for (int i = 0; i < time.Count; i++)
            {
                time[i]++;
            }
        }

        public Statistics(int mValue = 300)
        {
            maxValue = mValue;

            time = new List<int>(maxValue);

            for (int i = 0; i < maxValue; i++)
            {
                time.Add(i + 1);
            }

            //time.Reverse();

            values = new List<double>();

        }


        public void AddValue(double value)
        {
            if (values.Count == maxValue)
                values.RemoveAt(0);

            values.Add(value);
        }

        public void Update()
        {
            if (values.Count == maxValue)
            updateTime();
        }
    }
}
