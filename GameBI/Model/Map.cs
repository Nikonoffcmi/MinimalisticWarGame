using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBI.Model
{
    
    public class Map
    {
        public CellField[,] map { get; private set; }
        private Character[] charactersPlayerOne;
        private Character[] charactersPlayerTwo;

        public Map (int y, int x, Character[] charactersPlayerOne, Character[] charactersPlayerTwo)
        {
            map = new CellField[y, x];
            this.charactersPlayerOne = charactersPlayerOne;
            this.charactersPlayerTwo = charactersPlayerTwo;
        }

        public List<(int, int)> AvailableMovements(Character selectedCharacter, int distance)
        {
            int[,] cMap = new int[map.GetLength(0), map.GetLength(1)];
            int x, y, step = 0;
            List<(int, int)> list = new List<(int, int)> ();
            for (y = 0; y < map.GetLength(0); y++)
                for (x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == CellField.Barrier || map[y, x] == CellField.Character)
                        cMap[y, x] = -2;//индикатор стены или персонажа
                    else
                        cMap[y, x] = -1;//индикатор еще не ступали сюда
                }
            cMap[selectedCharacter.Location.Item2, selectedCharacter.Location.Item1] = 0;//тоска отсчета
            while (step < distance)
            {
                for (y = 0; y < map.GetLength(1); y++)
                    for (x = 0; x < map.GetLength(0); x++)
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
                                list.Add((y, x-1));
                            }

                            if (y + 1 < map.GetLength(1) && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                            {
                                cMap[x, y + 1] = step + 1;
                                list.Add((y + 1, x));
                            }

                            if (x + 1 < map.GetLength(0) && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
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

        public void SetEmptyCell(List<(int, int)> emptyCells)
        {
            foreach (var emptyCell in emptyCells)
            {
                if (emptyCell.Item2 < map.GetLength(1) && emptyCell.Item1 < map.GetLength(0))
                    map[emptyCell.Item2, emptyCell.Item1] = CellField.Empty;
            }
        }

        public void SetBarriers (List<(int, int)> barriers)
        {
            foreach (var barrier in barriers)
            {
                if (barrier.Item2 < map.GetLength(1) && barrier.Item1 < map.GetLength(0))
                    map[barrier.Item2, barrier.Item1] = CellField.Barrier;
            }
        }

        public void SetCharacters(Character[] characters)
        {
            foreach (var character in characters)
            {
                if (character.Location.Item2 < map.GetLength(1) && character.Location.Item1 < map.GetLength(0))
                    map[character.Location.Item2, character.Location.Item1] = CellField.Character;
            }
        }

        public List<(int, int)> AvailableAttacks(Character selectedCharacter, int distance)
        {
            List<(int, int)> list = new List<(int, int)>();
            int x = 0, y = 0;
            for (y = 1, x = 0; y <= distance; y++)
            {
                if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = -1, x = 0; y <= -distance; y--)
            {
                if (selectedCharacter.Location.Item2 + y > 0
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = 0, x = 1; x <= distance; x++)
            {
                if (selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = 0, x = -1; x <= -distance; x--)
            {
                if (selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = 1, x = 1; y <= distance; y++, x++)
            {
                if (selectedCharacter.Location.Item2 + y < map.GetLength(0) 
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = -1, x = -1; y <= -distance; y--, x--)
            {
                if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = -1, x = 1; y <= -distance; y--, x++)
            {
                if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }

            for (y = 1, x = -1; y <= distance; y++, x--)
            {
                if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x]
                == CellField.Barrier)
                    break;
                else if (selectedCharacter.Location.Item2 + y < map.GetLength(0)
                    && selectedCharacter.Location.Item2 + x < map.GetLength(1)
                    && map[selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x] == CellField.Character)
                    list.Add((selectedCharacter.Location.Item2 + y, selectedCharacter.Location.Item1 + x));
            }
            return list;
        }
    }
}
