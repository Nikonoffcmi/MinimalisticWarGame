using GameBI.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameBI.Model
{
    public class Object
    {
        public string name;
        public string texture;
        public Vector pos;
        public Vector size;

        public Object(string name, string texture, Vector pos, Vector size)
        {
            this.name = name;
            this.texture = texture;
            this.pos = pos;
            this.size = size;
        }

        public Object()
        {
        }

        public void SetLocation (Map map, (int, int) location)
        {
            //this.location = location;
        }
        
    }
}
