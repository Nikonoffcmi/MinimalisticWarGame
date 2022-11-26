using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    public class Character
    {
        public int HPModified { get; set; }
        public int DamageModified { get; set; }
        public int DistanceMoveModified { get; set; }

        private string Name;
        private int HP;
        private int damage;
        private int distanceMove;
        private List<IPassiveAbility> passiveAbilities;
        private List<IActiveAbility> ActiveAbilities { get; set; }

        public void Attack(Character character)
        {
            character.HP -= this.damage;
        }

        public void takeDamage(int damage)
        {
            this.HP -= damage;
        }

        public void ActivateAbility(Character character, IActiveAbility activeAbility)
        {
            if (!ActiveAbilities.Contains(activeAbility))
                return;
            ActiveAbilities
                .Find(aa => aa.GetType() == typeof(IActiveAbility))
                .ActivatePassiveAbility(character);
        }
    }
}
