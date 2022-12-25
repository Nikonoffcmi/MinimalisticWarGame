using GameBI.Model.PassiveAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GameBI.Model
{
    [XmlInclude(typeof(PassiveHeal))]
    [XmlInclude(typeof(PassiveAttackIncrease))]
    [Serializable]
    public abstract class IPassiveAbility
    {
        public string Name { get; set; }
        public int turnNext { get; set; }
        public abstract void ActivatePassiveAbility(Character character);
        
        public IPassiveAbility()
        {
        }
    }
}
