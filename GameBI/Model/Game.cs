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
            DefaultAbility();
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
            var barriers = new List<GameBarrier>() { gameBarrier};
            map.SetCell(barriers.ToList<Object>());
            objects.Add(gameBarrier);
        }

        public List<Vector> StartGame()
        {
            currPlayer = charactersPlayerOne;
            map.SetCell(charactersPlayerOne.ToList<Object>());
            map.SetCell(charactersPlayerTwo.ToList<Object>());

            isGameEnd = false;
            currPlayer = charactersPlayerOne;
            return currPlayer.Select(ch => ch.Position).ToList();
        }

        public List<Vector> OnCharacterPress(Vector location)
        {
            if (currCharater == null)
            {
                currCharater = currPlayer.ToList().Find(ch => ch.Position == location);
                var result = currCharater.AvailableMovements(map);
                result.AddRange(currCharater.AvailableAttacks(map));
                return result;
            }
            else
            {
                if (map.isCellObject<Character>((int)location.Y, (int)location.X))
                {
                    var ch = charactersPlayerOne.ToList();
                    ch.AddRange(charactersPlayerTwo.ToList());
                    var selCh = ch.Find(chr => chr.Position == location);
                    if (currActiveAbility != null)
                        currCharater.ActivateAbility(map, selCh, currActiveAbility);
                    else
                        selCh.takeDamage(currCharater.damage);
                }
                else if (!map.isCellObject<Barrier>((int)location.Y, (int)location.X))
                    currCharater.Move(map, location);
                currCharater = null;
                currActiveAbility = null;
                SwitchPlayer();
                return currPlayer.Select(ch => ch.Position).ToList();
            }
        }

        public List<IActiveAbility> CharacterAbilities(Vector position)
        {
            return currPlayer.ToList().Find(ch => ch.Position == position).ActiveAbilities;
        }

        public List<Vector> ActiveAbilitiesDistans(IActiveAbility activeAbility)
        {
            currActiveAbility = activeAbility;
            return currCharater.ActiveAbilities.Find(aa => aa.GetType() == activeAbility.GetType()).AbilityDistance(map);
        }

        public List<Vector> CancelingSelectedCharacter()
        {
            currCharater = null;
            currActiveAbility = null;
            return currPlayer.Select(ch => ch.Position).ToList();
        }

        public List<Vector> SkipTurn()
        {
            SwitchPlayer();
            return currPlayer.Select(ch => ch.Position).ToList();
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
                    currPlayer[i].NewTurn(currPlayer[i]);
                }
                else
                    currPlayer[i] = null;
            }
            if (liveCharater == 0)
                isGameEnd = true;
        }

        private void DefaultAbility()
        {
            activeAbilities = new List<IActiveAbility>();
            passiveAbilities = new List<IPassiveAbility>();

            activeAbilities.Add(new Heal("лечение", 1, 2));
            activeAbilities.Add(new AttackIncrease("увеличение атаки", 3, 2));

            passiveAbilities.Add(new PassiveHeal("пассивное лечение", 1, 2));
            passiveAbilities.Add(new PassiveAttackIncrease("пассивное увеличение атаки", 3, 2));
        }
    }
}
