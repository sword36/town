using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TownInterfaces
{
    public interface IBuilding
    {
        string buildingType { get; set; }
        Color BuildingColor { get; set; }

        int idCounter { get; set; }
        int id { get; set; }

        PointF Entrance { get; set; }

        PointF Room { get; }

        Rectangle Position { get; set; }

        void Draw(Graphics g);

        void AddHuman(ICitizen h);

        void RemoveHuman(ICitizen h);
    }
}
