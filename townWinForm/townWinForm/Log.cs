using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public static class Log
    {
        private static List<string> all = new List<string>();
        private static List<string> buildings = new List<string>();
        private static List<string> citizens = new List<string>();
        private static List<string> things = new List<string>();
        private static List<string> paths = new List<string>();
        private static List<string> other = new List<string>();

        public delegate void logUpdateHandler();
        public static event logUpdateHandler UpdateEvent;
        public static void FireUpdate()
        {
            if (UpdateEvent != null)
            {
                UpdateEvent();
            }
        }

        //Format of adding is - "LogType:Message"
        //If LogType not exist it will be Add
        //If formate not standart it will be Other
        public static void Add(string log)
        {
            int l = log.IndexOf(":");
            //if not standart format (without ":")
            if (l == -1)
            {
                other.Add(log);
                return;
            }
            
            string type = log.Substring(0, l).ToLower();
            //The most longest length of LogType is 8
            if (type.Length > 8)
            {
                other.Add(log);
                return;
            }

            l += 1; //to cut ":"

            int messageL = log.Length - l;
            int min = Math.Min(messageL, Config.MaxMessageLength);

            string message = log.Substring(l, min);
            message = DateTime.Now.ToShortTimeString() + ": " + message;

            all.Add(message);
            switch(type)
            {
                case "all":
                    break; //because allready added
                case "buildings":
                    buildings.Add(message);
                    break;
                case "citizens":
                    citizens.Add(message);
                    break;
                case "things":
                    things.Add(message);
                    break;
                case "paths":
                    paths.Add(message);
                    break;
                default:
                    other.Add(message);
                    break;
            }

            FireUpdate();
        }

        private static string listToSting(List<string> list)
        {
            string str = "";
            foreach (string s in list)
            {
                str += s + Environment.NewLine;
            }
            return str;
        }

        public static string Print(string type)
        {
            type = type.ToLower();
            switch(type)
            {
                case "all":
                    return listToSting(all);
                case "buildings":
                    return listToSting(buildings);
                case "citizens":
                    return listToSting(citizens);
                case "things":
                    return listToSting(things);
                case "paths":
                    return listToSting(paths);
                default:
                    return listToSting(other);
            }
        }
    }
}
