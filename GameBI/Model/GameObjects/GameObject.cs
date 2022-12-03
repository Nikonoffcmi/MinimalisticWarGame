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
        protected (int, int) location;

        public (int, int) GetLocation()
        {
            return location;
        }
        
    }
}
