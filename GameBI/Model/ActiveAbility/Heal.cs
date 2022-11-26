using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.ActiveAbility
{
    public class Heal
        : IActiveAbility
    {
        public string Name { get; }
        public int turnNext { get; set; }
        private int turn;
        private int heal;

        public Heal(string name, int turn, int heal)
        {
            this.Name = name;
            this.turnNext = 0;
            this.turn = turn;
            this.heal = heal;
        }

        public void ActivatePassiveAbility(Character character)
        {
            if (turnNext == 0)
                turnNext = turn;
            character.takeDamage(heal);
        }
    }
}
