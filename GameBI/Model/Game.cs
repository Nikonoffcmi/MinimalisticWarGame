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
        public Game()
        {
            charactersPlayerOne = new Character[5];
            charactersPlayerTwo = new Character[5];
            currPlayer = charactersPlayerOne;
        }

        public List<(int, int)> StartGame()
        {
            map = new Map(15, 15, charactersPlayerOne, charactersPlayerTwo);
            map.SetCharacters(charactersPlayerOne);
            map.SetCharacters(charactersPlayerTwo);
            map.SetBarriers();
            return currPlayer.Select(ch => ch.Location).ToList();
        }

        public List<(int, int)> SelctedCharacterShowMove ((int, int) location)
        {
            return map.AvailableMovements(currPlayer.ToList().Find(ch => ch.Location == location)
                , currPlayer.ToList().Find(ch => ch.Location == location).distanceMove);
        }

        public List<(int, int)> SelctedCharacterShowAttacks((int, int) location)
        {
            return map.AvailableAttacks(currPlayer.ToList().Find(ch => ch.Location == location)
                , currPlayer.ToList().Find(ch => ch.Location == location).distanceDamage);
        }

        public List<IActiveAbility> SelctedCharacterShowAbilities((int, int) location)
        {
            return currPlayer.ToList().Find(ch => ch.Location == location).ActiveAbilities;
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
