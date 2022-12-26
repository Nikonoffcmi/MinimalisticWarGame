using GameBI.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace GameBI.Model
{
    [Serializable]
    public class Character
        : Object
    {
        public int HPModified { get; set; }
        public int DamageModified { get; set; }

        public int HP { get; set; }
        public int damage { get; set; }

        public int distanceMove { get; set; }
        public int distanceAttack { get; set; }
        public List<IPassiveAbility> passiveAbilities { get; set; }
        public List<IActiveAbility> ActiveAbilities { get; set; }

        public Character()
        {
        }

        public Character(string name, int hp, int damage, int distanceMove, int distanceAttack, 
            string texture, Vector pos, List<IActiveAbility> activeAbilities, List<IPassiveAbility> passiveAbilities)
             : base(name, texture, pos)
        {
            HPModified = hp;
            DamageModified = damage;
            HP = hp;
            this.damage = damage;
            this.distanceMove = distanceMove;
            this.distanceAttack = distanceAttack;
            ActiveAbilities = activeAbilities;
            this.passiveAbilities = passiveAbilities;
        }

        public void takeDamage(int damage)
        {
            if (HPModified - damage > HP)
                HPModified += (HP - HPModified);
            else
                HPModified -= damage;
        }

        public void ActivateAbility(Map map, Character character, IActiveAbility activeAbility)
        {
            if (!ActiveAbilities.Contains(activeAbility))
                return;
            ActiveAbilities
                .Find(aa => ActiveAbilities.Contains(activeAbility))
                .ActivateActiveAbility(map, character);
        }

        public List<Vector> AvailableMovements(Map map)
        {
            int[,] cMap = new int[map.map.GetLength(1), map.map.GetLength(0)];
            int x, y, step = 0;
            List<Vector> list = new List<Vector>();
            for (y = 0; y < map.map.GetLength(0); y++)
                for (x = 0; x < map.map.GetLength(1); x++)
                {
                    if (map.map[y, x].GetType() == typeof(Character) || map.map[y, x].GetType() == typeof(GameBarrier))
                        cMap[y, x] = -2;//индикатор стены или персонажа
                    else
                        cMap[y, x] = -1;//индикатор еще не ступали сюда
                }

            cMap[(int)Position.Y, (int)Position.X] = 0;//точка отсчета
            while (step < distanceMove)
            {
                for (y = 0; y < map.map.GetLength(1); y++)
                    for (x = 0; x < map.map.GetLength(0); x++)
                    {
                        if (cMap[x, y] == step)
                        {

                            //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            if (y - 1 >= 0 && cMap[x, y - 1] != -2 && cMap[x, y - 1] == -1)
                            {
                                cMap[x, y - 1] = step + 1;
                                list.Add(new Vector(y - 1, x));
                            }

                            if (x - 1 >= 0 && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                            {
                                cMap[x - 1, y] = step + 1;
                                list.Add(new Vector(y, x - 1));
                            }

                            if (y + 1 < map.map.GetLength(1) && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                            {
                                cMap[x, y + 1] = step + 1;
                                list.Add(new Vector(y + 1, x));
                            }

                            if (x + 1 < map.map.GetLength(0) && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                            {
                                cMap[x + 1, y] = step + 1;
                                list.Add(new Vector(y, x + 1));
                            }
                        }
                    }
                step++;
            }
            return list;
        }

        public void Move(Map map, Vector toPosition)
        {
            if (AvailableMovements(map).Contains(toPosition))
            {
                map.SetCell(new List<Object>() { new Object(Name, Texture, Position) });
                Position = toPosition;
                map.SetCell(new List<Object>() { this });
            }
        }

        public List<Vector> AvailableAttacks(Map map)
        {
            List<Vector> list = new List<Vector>();
            int x = 0, y = 0;
            for (y = 1, x = 0; y <= distanceAttack && (int)Position.Y + y < map.map.GetLength(0); y++)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = -1, x = 0; y <= -distanceAttack && (int)Position.Y + y >= 0; y--)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = 0, x = 1; x <= distanceAttack && (int)Position.X + x < map.map.GetLength(1); x++)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = 0, x = -1; x <= -distanceAttack && (int)Position.X + x >= 0; x--)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = 1, x = 1; y <= distanceAttack && (int)Position.X + x < map.map.GetLength(1) 
                && (int)Position.Y + y < map.map.GetLength(0); y++, x++)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = -1, x = -1; y <= -distanceAttack && (int)Position.X + x >= 0
                && (int)Position.Y + y >= 0; y--, x--)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = -1, x = 1; y <= -distanceAttack && (int)Position.X + x < map.map.GetLength(1)
                && (int)Position.Y + y >= 0; y--, x++)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }

            for (y = 1, x = -1; y <= distanceAttack && (int)Position.X + x >= 0
                && (int)Position.Y + y < map.map.GetLength(0); y++, x--)
            {
                if (map.isCellObject<GameBarrier>((int)Position.Y + y, (int)Position.X + x))
                    break;
                if (map.isCellObject<Character>((int)Position.Y + y, (int)Position.X + x))
                    list.Add(new Vector(Position.X + x, Position.Y + y));
            }
            list.Add(Position);
            return list;
        }

        public bool IsAlive()
        {
            if (HPModified <= 0)
                return false;
            return true;
        }

        public void NewTurn(Character character)
        {
            passiveAbilities.ForEach(pa => pa.ActivatePassiveAbility(character));
        }
    }
}
