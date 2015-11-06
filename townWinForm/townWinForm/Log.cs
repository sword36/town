using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public static class Log
    {
        public static List<string> All = new List<string>();
        public static List<string> Buildings = new List<string>();
        public static List<string> Citizens = new List<string>();
        public static List<string> Things = new List<string>();
        public static List<string> Paths = new List<string>();
        public static List<string> Other = new List<string>();

        //Format of adding is - "LogType:Message"
        //If LogType not exist it will be Add
        //If formate not standart it will be Other
        public static void Add(string log)
        {
            int l = log.IndexOf(":");
            //if not standart format (without ":")
            if (l == -1)
            {
                Other.Add(log);
                return;
            }
            
            string type = log.Substring(0, l).ToLower();
            //The most longest length of LogType is 8
            if (type.Length > 8)
            {
                Other.Add(log);
                return;
            }


            string message = log.Substring(l, Config.MaxMessageLength);

            All.Add(message);
            switch(type)
            {
                case "all":
                    break; //because allready added
                case "buildings":
                    Buildings.Add(message);
                    break;
                case "citizens":
                    Citizens.Add(message);
                    break;
                case "things":
                    Things.Add(message);
                    break;
                case "paths":
                    Paths.Add(message);
                    break;
                default:
                    Other.Add(message);
                    break;
            }
        }
    }
}
