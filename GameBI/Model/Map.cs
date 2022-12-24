using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    
    public class Map
    {
        public Object[,] map { get; private set; }

        public Map (int x, int y)
        {
            map = new Object[y, x];
        }
        
        public void SetCell(List<Object> gameObjects)
        {
            foreach (var emptyCell in gameObjects)
            {
                if (emptyCell.pos.Y < map.GetLength(1) && emptyCell.pos.X < map.GetLength(0))
                    map[(int)emptyCell.pos.Y, (int)emptyCell.pos.X] = emptyCell;
            }
        }

        public bool isCellObject<T>(int y, int x)
            where T : class
        {
            if (y < map.GetLength(0) && x < map.GetLength(1)
                    && map[y, x].GetType() == typeof(T))
                return true;
            return false;
        }
    }
}
