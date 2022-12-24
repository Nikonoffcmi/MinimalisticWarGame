using GameBI.Model.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    public class Character
        : GameObject
    {
        public int HPModified { get; set; }
        public int DamageModified { get; set; }


        private string Name;
        private int HP;
        public int damage { get; private set; }

        private int distanceMove;
        private int distanceAttack;
        private List<IPassiveAbility> passiveAbilities;

        public Character((int, int) location) : base(location)
        {
        }

        public List<IActiveAbility> ActiveAbilities { get; set; }

        public void takeDamage(int damage)
        {
            this.HP -= damage;
        }

        public void ActivateAbility(Map map, Character character, IActiveAbility activeAbility)
        {
            if (!ActiveAbilities.Contains(activeAbility))
                return;
            ActiveAbilities
                .Find(aa => aa.GetType() == typeof(IActiveAbility))
                .ActivateActiveAbility(map, character);
        }

        public List<(int, int)> AvailableMovements(Map map)
        {
            int[,] cMap = new int[map.map.GetLength(0), map.map.GetLength(1)];
            int x, y, step = 0;
            List<(int, int)> list = new List<(int, int)>();
            for (y = 0; y < map.map.GetLength(0); y++)
                for (x = 0; x < map.map.GetLength(1); x++)
                {
                    if (map.map[y, x].GetType() == typeof(Character) || map.map[y, x].GetType() == typeof(Barrier))
                        cMap[y, x] = -2;//индикатор стены или персонажа
                    else
                        cMap[y, x] = -1;//индикатор еще не ступали сюда
                }

            cMap[location.Item2, location.Item1] = 0;//точка отсчета
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
                                list.Add((y, x));
                            }

                            if (x - 1 >= 0 && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                            {
                                cMap[x - 1, y] = step + 1;
                                list.Add((y, x - 1));
                            }

                            if (y + 1 < map.map.GetLength(1) && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                            {
                                cMap[x, y + 1] = step + 1;
                                list.Add((y + 1, x));
                            }

                            if (x + 1 < map.map.GetLength(0) && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                            {
                                cMap[x + 1, y] = step + 1;
                                list.Add((y, x + 1));
                            }
                        }
                    }
                step++;
            }
            return list;
        }

        public void Move(Map map, (int, int) toLocation)
        {
            if (AvailableMovements(map).Contains(toLocation))
                SetLocation(map, toLocation);
        }

        public List<(int, int)> AvailableAttacks(Map map)
        {
            List<(int, int)> list = new List<(int, int)>();
            int x = 0, y = 0;
            for (y = 1, x = 0; y <= distanceAttack; y++)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = -1, x = 0; y <= -distanceAttack; y--)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = 0, x = 1; x <= distanceAttack; x++)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = 0, x = -1; x <= -distanceAttack; x--)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = 1, x = 1; y <= distanceAttack; y++, x++)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = -1, x = -1; y <= -distanceAttack; y--, x--)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = -1, x = 1; y <= -distanceAttack; y--, x++)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }

            for (y = 1, x = -1; y <= distanceAttack; y++, x--)
            {
                if (map.isCellObject<Character>(location.Item2 + y, location.Item1 + x))
                    list.Add((location.Item2 + y, location.Item1 + x));
            }
            return list;
        }

        public bool IsAlive()
        {
            if (HPModified < 0)
                return false;
            return true;
        }

        public void NewTurn()
        {
            passiveAbilities.ForEach(pa => pa.ActivatePassiveAbility(null));
        }
    }
}
