using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    
    public class Map
    {
        public Object[,] map { get; set; }

        public Map (int x, int y)
        {
            map = new Object[y, x];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i, j] = new Object();
        }

        public Map()
        {
        }
        
        public void SetCell(List<Object> gameObjects)
        {
            foreach (var emptyCell in gameObjects)
            {
                if (emptyCell.Position.Y < map.GetLength(1) && emptyCell.Position.X < map.GetLength(0))
                    map[(int)emptyCell.Position.Y, (int)emptyCell.Position.X] = emptyCell;
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
