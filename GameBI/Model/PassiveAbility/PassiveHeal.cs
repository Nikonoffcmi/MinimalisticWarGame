using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.PassiveAbility
{
    [Serializable]
    public class PassiveHeal
        : IPassiveAbility
    {
        public int turn { get; set; }
        public int heal { get; set; }

        public PassiveHeal(string name, int turn, int heal)
        {
            this.Name = name;
            this.turnNext = 0;
            this.turn = turn;
            this.heal = heal;
        }

        public PassiveHeal()
        {
        }

        public override void ActivatePassiveAbility(Character character)
        {
            if (turnNext == 0)
                turnNext = turn;
            character.takeDamage(-heal);
        }
    }
}
