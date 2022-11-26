using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.PassiveAbility
{
    public class PassiveAttackIncrease
        : IPassiveAbility
    {
        public string Name { get; }
        public int turnNext { get; set; }
        private int turn;
        private int attackIncrease;

        public PassiveAttackIncrease(string name, int turn, int attackIncrease)
        {
            this.Name = name;
            this.turnNext = turn;
            this.turn = turn;
            this.attackIncrease = attackIncrease;
        }

        public void ActivatePassiveAbility(Character character)
        {
            character.DamageModified = +attackIncrease;
        }
    }
}
