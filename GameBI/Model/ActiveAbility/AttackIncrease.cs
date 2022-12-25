using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace GameBI.Model.ActiveAbility
{
    [XmlInclude(typeof(AttackIncrease))]
    [XmlInclude(typeof(Heal))]
    [Serializable]
    public class AttackIncrease
        : IActiveAbility
    {
        public int turn { get; set; }
        public int attackIncrease { get; set; }

        public AttackIncrease(string name, int turn, int attackIncrease)
        {
            Name = name;
            turnNext = turn;
            this.turn = turn;
            this.attackIncrease = attackIncrease;
        }

        public AttackIncrease()
        {
        }

        public override void ActivateActiveAbility(Map map, Character character)
        {
            character.DamageModified = +attackIncrease;
        }

        public override List<Vector> AbilityDistance(Map map)
        {
            return null;
        }
    }
}
