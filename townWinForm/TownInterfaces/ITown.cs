using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TownInterfaces
{
    public interface ITown
    {
        Point MousePosition { get; set; }
        Point CurrentTile { get; set; }

        List<ICitizen> Citizens { get; set; }

        int[,] matrix { get; set; }
        int[,] AstarMatrix { get; set; }

        //List<PointF> homeToWork { get; set; }

        List<IEntertainment> Taverns { get; set; }

        ICitizen god { get; set; }

        ICitizen God { get; set; }

        IWorkshop GetWorkshop(string prof);
        IResidence GetHome();
        PointF GetRandomStreetPoint();

        PointF GetRandomStreetPoint(PointF startPoint, float distance);

        List<PointF> FindPath(Point start, PointF finish);

        List<PointF> FindPath(Point start, IBuilding finish);

        List<PointF> FindPath(Point start, IResidence finish);

        List<PointF> FindPath(Point start, IWorkshop finish);

        IEntertainment GetTavern();

        IWorkshop GetNearestMarket(ICitizen h);

        KeyValuePair<string, Image> GetInfo();

        KeyValuePair<string, Image> GetInfo(int id);

        IBuilding IsHumanInBuilding(ICitizen h);
    }
}
