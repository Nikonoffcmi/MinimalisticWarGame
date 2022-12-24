using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    
    public class Map
    {
        public GameObject[,] map { get; private set; }

        public Map (int y, int x)
        {
            map = new GameObject[y, x];
        }

        
        public void SetCell(List<GameObject> gameObjects)
        {
            foreach (var emptyCell in gameObjects)
            {
                if (emptyCell.location.Item2 < map.GetLength(1) && emptyCell.location.Item1 < map.GetLength(0))
                    map[emptyCell.location.Item2, emptyCell.location.Item1] = emptyCell;
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
