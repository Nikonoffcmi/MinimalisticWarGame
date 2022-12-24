using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameBI.Model.GameObjects
{
    public class GameBarrier
        :Object
    {
        public GameBarrier(string name, string texture, Vector pos, Vector size)
            : base(name, texture, pos, size)
        {
        }

        GameBarrier()
        {
        }
    }
}
