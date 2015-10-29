using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm.Graph
{
    public class Graph
    {
        List<Node> Nodes;
        List<Edge> Edges;

        public Graph()
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }
    }
}
