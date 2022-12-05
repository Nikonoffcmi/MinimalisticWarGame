using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameBI.Model
{
    public interface IActiveAbility
    {
        string Name { get; }
        int turnNext { get; set; }
        void ActivateActiveAbility(Map map, Character character);
        List<(int, int)> AbilityDistance(Map map);
    }
}
