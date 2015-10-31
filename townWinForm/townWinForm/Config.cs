using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public static class Config
    {
        //Common
        public static int GameSpeed = 1; //control: Track bar
        public static int Zoom = 1; //control: Track bar

        //Citizens
        public static int MaxCitizens = 50;
        public static int MaxHappiness = 100;
        public static int StartHappiness = 75;
        public static int StartHappinessDelta = 25;
        public static string[] ProfList = new string[] {"craftsman", "farmer", "guardian", "thief", "trader"};
        public static int MaxProfLevel = 10;

        //Display
        public static int FPS = 30;

        //Economic
        public static int StartMoney = 1000;
        public static int StartMoneyDelta = 500;
    }
}
