using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    public enum PlayersCharacter
    {
        PlayerOne,
        PlayerTwo,
    }
    public class Character
    {
        public int HPModified { get; set; }
        public int DamageModified { get; set; }
        public (int, int) Location { get; set; }
        public PlayersCharacter PlayerCharacter { get; private set; }


        private string Name;
        private int HP;
        private int damage;
        public int distanceMove { get; private set; }
        public int distanceDamage { get; private set; }
        private List<IPassiveAbility> passiveAbilities;
        public List<IActiveAbility> ActiveAbilities { get; set; }

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
