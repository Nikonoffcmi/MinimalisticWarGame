using GameBI.Model.ActiveAbility;
using GameBI.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace GameBI.Model
{
    [XmlInclude(typeof(Character))]
    [XmlInclude(typeof(GameBarrier))]
    [XmlInclude(typeof(IPassiveAbility))]
    [XmlInclude(typeof(IActiveAbility))]
    [Serializable]
    public class Object
    {
        public string Name { get; set; }
        public string Texture { get; set; }
        public Vector Position { get; set; }

        public Object(string name, string texture, Vector pos)
        {
            this.Name = name;
            this.Texture = texture;
            this.Position = pos;
        }

        public Object()
        {
        }

        public void SetLocation(Vector position)
        {
            this.Position = position;
        }
    }
}
