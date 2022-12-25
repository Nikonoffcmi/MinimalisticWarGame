using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameBI.Model.ActiveAbility
{
    [Serializable]
    public class Heal
        : IActiveAbility
    {
        public int turn { get; set; }
        public int heal { get; set; }

        public Heal(string name, int turn, int heal)
        {
            Name = name;
            turnNext = 0;
            this.turn = turn;
            this.heal = heal;
        }

        public Heal()
        {
        }

        public override void ActivateActiveAbility(Map map, Character character)
        {
            if (turnNext == 0)
                turnNext = turn;
            character.takeDamage(heal);
        }

        public override List<Vector> AbilityDistance(Map map)
        {
            return null;
        }
    }
}
