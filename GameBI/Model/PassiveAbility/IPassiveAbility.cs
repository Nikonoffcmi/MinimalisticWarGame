using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameBI.Model
{
    public interface IPassiveAbility
    {
        string Name { get; }
        int turnNext { get; set; }
        void ActivatePassiveAbility(Character character);
    }
}
