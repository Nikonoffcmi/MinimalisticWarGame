using GameBI.Model.ActiveAbility;
using GameBI.Model.GameObjects;
using GameBI.Model.PassiveAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GameBI.Model
{
    [Serializable]
    public class Game
    {
        public List<Object> objects;
        public List<IActiveAbility> activeAbilities;
        public List<IPassiveAbility> passiveAbilities;
        public bool isGameEnd = true;
        public List<Character> charactersPlayerOne;
        public List<Character> charactersPlayerTwo;
        public List<Character> currPlayer;
        private Map map;
        private Character currCharater;
        private IActiveAbility currActiveAbility;

        public Vector mapSize { get => new Vector(map.map.GetLength(0), map.map.GetLength(1)); }
        public Game()
        {
            charactersPlayerOne = new List<Character>();
            charactersPlayerTwo = new List<Character>();
            currPlayer = new List<Character>();
            map = new Map(10, 10);
            objects = new List<Object>();
            activeAbilities = new List<IActiveAbility>();
            passiveAbilities = new List<IPassiveAbility>();
        }

        public void AddCharacterPlayerOne(Character character)
        {
            character.SetLocation(new Vector(charactersPlayerOne.Count, map.map.GetLength(0) - 1));
            charactersPlayerOne.Add(character);

            objects.Add(character);
        }

        public void AddCharacterPlayerTwo(Character character)
        {
            character.SetLocation(new Vector(map.map.GetLength(0) - charactersPlayerTwo.Count - 1, 0));
            charactersPlayerTwo.Add(character);
        }

        public void AddGameBarrier(GameBarrier gameBarrier)
        {
            objects.Add(gameBarrier);
        }

        public List<Vector> StartGame()
        {
            map.SetCell(objects.Where(b => b.GetType() == typeof(GameBarrier)).ToList());
            map.SetCell(charactersPlayerOne.ToList<Object>());
            map.SetCell(charactersPlayerTwo.ToList<Object>());
            if (isGameEnd)
            {
                isGameEnd = false;
                currPlayer = charactersPlayerOne;
            }

            if (currPlayer.Select(ch => ch.Position).ToList()
                .Contains(charactersPlayerOne.Select(ch => ch.Position).ToList()[0]))
                currPlayer = charactersPlayerOne;
            else
                currPlayer = charactersPlayerTwo;
            return currPlayer.Select(ch => ch.Position).ToList();
        }

        public List<Vector> OnCharacterPressMove(Vector location)
        {
            if (currCharater == null)
            {
                currCharater = currPlayer.ToList().Find(ch => ch.Position == location);
                if (currCharater == null)
                    return currPlayer.Select(ch => ch.Position).ToList();
                var result = currCharater.AvailableMovements(map);
                result.AddRange(currCharater.AvailableAttacks(map));
                return result;
            }
            else
            {
                if (map.isCellObject<Character>((int)location.Y, (int)location.X)
                    && currCharater.AvailableAttacks(map).Contains(location))
                {
                    var ch = charactersPlayerOne.ToList();
                    ch.AddRange(charactersPlayerTwo.ToList());
                    var selCh = ch.Find(chr => chr.Position == location);
                    if (currActiveAbility != null)
                        currCharater.ActivateAbility(map, selCh, currActiveAbility);
                    else
                        selCh.takeDamage(currCharater.DamageModified);
                }
                else if (!map.isCellObject<GameBarrier>((int)location.Y, (int)location.X) 
                    && currCharater.AvailableMovements(map).Contains(location))
                    currCharater.Move(map, location);
                else
                {
                    var result = currCharater.AvailableMovements(map);
                    result.AddRange(currCharater.AvailableAttacks(map));
                    return result;
                }
                currCharater = null;
                currActiveAbility = null;
                SwitchPlayer();
                return currPlayer.Select(ch => ch.Position).ToList();
            }
        }

        public List<string> CharacterAbilities(Vector position)
        {
            var allch = new List<Character>(charactersPlayerOne);
            allch.AddRange(charactersPlayerTwo);
            return allch.ToList().Find(ch => ch.Position == position).ActiveAbilities.Select(aa => aa.Name).ToList();
        }

        public List<Vector> ActiveAbilitiesDistans(string activeAbility)
        {
            if (currCharater == null)
                return currPlayer.Select(ch => ch.Position).ToList();
            currActiveAbility = currCharater.ActiveAbilities.Find(aa => aa.Name.Equals(activeAbility));
            return currCharater.AvailableAttacks(map);
        }

        public List<Vector> CancelingSelectedCharacter()
        {
            currCharater = null;
            currActiveAbility = null;
            return currPlayer.Select(ch => ch.Position).ToList();
        }

        public List<Vector> SkipTurn()
        {
            currCharater = null;
            currActiveAbility = null;
            SwitchPlayer();
            return currPlayer.Select(ch => ch.Position).ToList();
        }

        private void SwitchPlayer()
        {
            var liveCharater = 0;
            var tempPlayer = new List<Character>(currPlayer);

            for (int i = 0; i < tempPlayer.Count(); i++)
            {
                if (tempPlayer[i].IsAlive())
                    liveCharater++;
                else
                {
                    currPlayer.Remove(tempPlayer[i]);
                    map.SetCell(new List<Object>() { new Object(null, null, tempPlayer[i].Position) });
                }
            }
            if (liveCharater == 0)
                isGameEnd = true;

            if (currPlayer == charactersPlayerOne)
                currPlayer = charactersPlayerTwo;
            else
                currPlayer = charactersPlayerOne;

            liveCharater = 0;
            tempPlayer = new List<Character>(currPlayer);
            for (int i = 0; i < tempPlayer.Count(); i++)
            {
                if (tempPlayer[i].IsAlive())
                {
                    liveCharater++;
                    tempPlayer[i].NewTurn(tempPlayer[i]);
                }
                else
                {
                    currPlayer.Remove(tempPlayer[i]);
                    map.SetCell(new List<Object>() { new Object(null, null, tempPlayer[i].Position) });
                }
            }
            if (liveCharater == 0)
                isGameEnd = true;
        }
    }
}
