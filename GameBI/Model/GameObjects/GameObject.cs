using GameBI.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    public class GameObject
    {
        public (int, int) location { get; private set; }

        public GameObject ((int, int) location)
        {
            this.location = location;
        }

        public void SetLocation (Map map, (int, int) location)
        {
            this.location = location;
        }
        
    }
}
