using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.GameObjects
{
    public class GameBarrier
        :Object
    {
        public GameBarrier(string name,(int, int) location)
            : base(name, location)
        {
        }

        GameBarrier()
        {
        }
    }
}
