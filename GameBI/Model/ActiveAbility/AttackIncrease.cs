using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model.ActiveAbility
{
    public class AttackIncrease
        : IActiveAbility
    {
        public string Name { get; }
        public int turnNext { get; set; }
        private int turn;
        private int attackIncrease;

        public AttackIncrease(string name, int turn, int attackIncrease)
        {
            this.Name = name;
            this.turnNext = turn;
            this.turn = turn;
            this.attackIncrease = attackIncrease;
        }

        public void ActivateActiveAbility(Map map, Character character)
        {
            character.DamageModified = +attackIncrease;
        }

        public List<(int, int)> AbilityDistance(Map map)
        {
            return null;
        }
    }
}
