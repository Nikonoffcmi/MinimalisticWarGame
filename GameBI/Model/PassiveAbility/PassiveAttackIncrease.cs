using GameBI.Model.ActiveAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.PassiveAbility
{
    [Serializable]
    public class PassiveAttackIncrease
        : IPassiveAbility
    {
        public int turn { get; set; }
        public int attackIncrease { get; set; }

        public PassiveAttackIncrease(string name, int turn, int attackIncrease)
        {
            this.Name = name;
            this.turnNext = turn;
            this.turn = turn;
            this.attackIncrease = attackIncrease;
        }

        public PassiveAttackIncrease()
        {
        }

        public override void ActivatePassiveAbility(Character character)
        {
            turnNext--;
            if (turnNext <= 0)
            {
                turnNext = turn;
                if (character.damage * 3 > character.DamageModified + attackIncrease)
                    character.DamageModified += attackIncrease;
            }
        }
    }
}
