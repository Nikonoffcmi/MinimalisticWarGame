using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    public class Game
    {
        private Character[] charactersPlayerOne;
        private Character[] charactersPlayerTwo;
        private Character[] currPlayer;
        private Map map;
        private Character SelectedCh;
        public Game()
        {
            charactersPlayerOne = new Character[5];
            charactersPlayerTwo = new Character[5];
            currPlayer = charactersPlayerOne;
            map = new Map(15, 15);
        }


        public List<(int, int)> StartGame()
        {
            
        }

        public List<(int, int)> CharacterMove((int, int) location)
        {
            if (SelectedCh != null)
            {
                return currPlayer.ToList().Find(ch => ch.GetLocation() == location).AvailableMovements(map);
            }
            IsSelected = false;
            
        }

        public List<(int, int)> CharacterAttacks((int, int) location)
        {
            return map.AvailableAttacks(currPlayer.ToList().Find(ch => ch.Location == location)
                , currPlayer.ToList().Find(ch => ch.Location == location).distanceDamage);
        }

        public List<IActiveAbility> CharacterAbilities((int, int) location)
        {
            return currPlayer.ToList().Find(ch => ch.Location == location).ActiveAbilities;
        }

        public void Move()
        {

        }
        private void SwitchPlayer()
        {
            if (currPlayer == charactersPlayerOne)
                currPlayer = charactersPlayerTwo;
            else
                currPlayer = charactersPlayerOne;
        }
    }
}
