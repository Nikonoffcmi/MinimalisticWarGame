using GameBI.Model.ActiveAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GameBI.Model
{
    [XmlInclude(typeof(AttackIncrease))]
    [XmlInclude(typeof(Heal))]
    [Serializable]
    public abstract class IActiveAbility
    {
        public string Name { get; set; }
        protected int turnNext { get; set; }
        public abstract void ActivateActiveAbility(Map map, Character character);
        public IActiveAbility()
        {
        }


    }
}
