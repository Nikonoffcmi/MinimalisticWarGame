using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.GameObjects
{
    internal class Barrier
        :GameObject
    {
        public Barrier((int, int) location)
            : base(location)
        {
        }
    }
}
