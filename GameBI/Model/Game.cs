using GameBI.Model.GameObjects;
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
        private Character currCharater;
        private IActiveAbility currActiveAbility;
        public bool isGameEnd = false;
        public Game()
        {
            charactersPlayerOne = new Character[5];
            charactersPlayerTwo = new Character[5];
            currPlayer = charactersPlayerOne;
            map = new Map(15, 15);
        }

        public void AddCharacterPlayerOne(Character character)
        {
            for (int i = 0; i < charactersPlayerOne.Length; i++)
                if (charactersPlayerOne[i] == null)
                {
                    charactersPlayerOne[i] = character;
                    charactersPlayerOne[i].SetLocation(map, (map.map.GetLength(0), i));
                    break;
                }
        }

        public void AddCharacterPlayerTwo(Character character)
        {
            for (int i = 0; i < charactersPlayerTwo.Length; i++)
                if (charactersPlayerTwo[i] == null)
                {
                    charactersPlayerTwo[i] = character;
                    charactersPlayerTwo[i].SetLocation(map, (0, map.map.GetLength(0) - i));
                    break;
                }
        }

        public List<(int, int)> StartGame(List<(int, int)> barrierLocation)
        {
            map.SetCell(charactersPlayerOne.ToList<GameObject>());
            map.SetCell(charactersPlayerTwo.ToList<GameObject>());
            var barriers = new List<Barrier>(barrierLocation.Count);
            for (int i = 0; i < barrierLocation.Count; i++)
                barriers.Add(new Barrier(barrierLocation[i]));
            map.SetCell(barriers.ToList<GameObject>());

            currPlayer = charactersPlayerOne;
            return currPlayer.Select(ch => ch.location).ToList();
        }

        public List<(int, int)> OnCharacterPress((int, int) location)
        {
            if (currCharater == null)
            {
                currCharater = currPlayer.ToList().Find(ch => ch.location == location);
                var result = currCharater.AvailableMovements(map);
                result.AddRange(currCharater.AvailableAttacks(map));
                return result;
            }
            else
            {
                if (map.isCellObject<Character>(location.Item2, location.Item1))
                {
                    var ch = charactersPlayerOne.ToList();
                    ch.AddRange(charactersPlayerTwo.ToList());
                    var selCh = ch.Find(chr => chr.location == location);
                    if (currActiveAbility != null)
                        currCharater.ActivateAbility(map, selCh, currActiveAbility);
                    else 
                        selCh.takeDamage(currCharater.damage);
                }
                else if (!map.isCellObject<Barrier>(location.Item2, location.Item1))
                    currCharater.Move(map, location);
                currCharater = null;
                currActiveAbility = null;
                SwitchPlayer();
                return currPlayer.Select(ch => ch.location).ToList();
            }
        }

        public List<IActiveAbility> CharacterAbilities((int, int) location)
        {
            return currPlayer.ToList().Find(ch => ch.location == location).ActiveAbilities;
        }

        public List<(int, int)> ActiveAbilitiesDistans(IActiveAbility activeAbility)
        {
            currActiveAbility = activeAbility;
            return currCharater.ActiveAbilities.Find(aa => aa.GetType() == activeAbility.GetType()).AbilityDistance(map);
        }

        public List<(int, int)> CancelingSelectedCharacter()
        {
            currCharater = null;
            currActiveAbility = null;
            return currPlayer.Select(ch => ch.location).ToList();
        }

        public List<(int, int)> SkipTurn()
        {
            SwitchPlayer();
            return currPlayer.Select(ch => ch.location).ToList();
        }

        private void SwitchPlayer()
        {
            if (currPlayer == charactersPlayerOne)
                currPlayer = charactersPlayerTwo;
            else
                currPlayer = charactersPlayerOne;

            var liveCharater = 0; 
            for (int i = 0; i < currPlayer.Count(); i++)
            {
                if (currPlayer[i].IsAlive())
                {
                    liveCharater++;
                    currPlayer[i].NewTurn();
                }
                else
                    currPlayer[i] = null;
            }
            if (liveCharater == 0)
                isGameEnd = true;
        }
    }
}
