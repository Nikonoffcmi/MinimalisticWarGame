using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    public class Game
    {
        private int[,] field;
        private Character[] charactersPlayerOne;
        private Character[] charactersPlayerTwo;

        public Game()
        {
            charactersPlayerOne = new Character[5];
            charactersPlayerTwo = new Character[5];
        }

        public void StartGame()
        {
        }

        public void EndGame()
        {
        }
    }
}
