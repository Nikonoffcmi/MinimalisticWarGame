using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    enum CellField
    {
        Empty,
        Barrier,
        Character
    }
    public class Map
    {
        CellField[,] map;

        public Map (int x, int y)
        {
            map = new CellField[x, y];
        }

        public List<Dictionary<int, int>> AvailableMovements(Dictionary<int, int> selectedCell, int distance)
        {
            
        }
    }
}
