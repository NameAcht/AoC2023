using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC2023
{
    internal class Day03
    {
        public static int Part1(string[] schematic)
        {
            int sum = 0;
            
            for (int i = 0; i < schematic.Length; i++)
            {
                string currLine = schematic[i];
                string nextLine = "............................................................................................................................................";
                string prevLine = "............................................................................................................................................";

                if(i != schematic.Length - 1)
                    nextLine = schematic[i + 1];
                if(i != 0)
                    prevLine = schematic[i - 1];

                var matches = Regex.Matches(currLine, @"\d+");
                foreach(Match m in matches)
                {
                    var match = m.ToString();
                    var matchStart = m.Index;

                    for (int matchIndex = matchStart; matchIndex < matchStart + match.Length; matchIndex++)
                    {
                        var list = GetSurroundingChars(prevLine, nextLine, currLine, matchIndex);

                        if (!list.TrueForAll(surrounding => char.IsDigit(surrounding) || surrounding == '.'))
                        {
                            sum += int.Parse(match);
                            break;
                        }
                    }
                }
            }
            return sum;
        }
        public static int Part2(string[] schematic)
        {
            int sum = 0;

            for (int row = 0; row < schematic.Length; row++)
            {
                string currLine = schematic[row];
                string nextLine = "............................................................................................................................................";
                string prevLine = "............................................................................................................................................";

                if (row != schematic.Length - 1)
                    nextLine = schematic[row + 1];
                if (row != 0)
                    prevLine = schematic[row - 1];

                for (int column = 0; column < currLine.Length; column++)
                {
                    char currChar = currLine[column];
                    if (currChar == '*')
                    {
                        // (column, row)
                        var list = new List<(int row, int col)>();

                        bool isLeftEdge = column == 0;
                        bool isRightEdge = column == currLine.Length - 1;
                        bool isEdge = isRightEdge || isLeftEdge;

                        // currLine
                        if (!isLeftEdge && char.IsDigit(currLine[column - 1]))
                            list.Add((0, -1));
                        if (!isRightEdge && char.IsDigit(currLine[column + 1]))
                            list.Add((0, 1));

                        // prevLine
                        if (char.IsDigit(prevLine[column]))
                            list.Add((-1, 0));
                        else if (!isEdge && char.IsDigit(prevLine[column - 1]) && char.IsDigit(prevLine[column + 1]))
                        {
                            list.Add((-1, -1)); 
                            list.Add((-1, 1));
                        }
                        else if (!isLeftEdge && char.IsDigit(prevLine[column - 1]))
                            list.Add((-1, -1));
                        else if (!isRightEdge && char.IsDigit(prevLine[column + 1]))
                            list.Add((-1, 1));

                        // nextLine
                        if (char.IsDigit(nextLine[column]))
                            list.Add((1, 0));
                        else if (!isEdge && char.IsDigit(nextLine[column - 1]) && char.IsDigit(nextLine[column + 1]))
                        {
                            list.Add((1, -1));
                            list.Add((1, 1));
                        }
                        else if (!isLeftEdge && char.IsDigit(nextLine[column - 1]))
                            list.Add((1, -1));
                        else if (!isRightEdge && char.IsDigit(nextLine[column + 1]))
                            list.Add((1, 1));

                        if (list.Count == 2)
                        {
                            int firstCol = column + list[0].col;
                            int firstRow = row + list[0].row;
                            int secondCol = column + list[1].col;
                            int secondRow = row + list[1].row;

                            var first = new StringBuilder(schematic[firstRow][firstCol].ToString());

                            for (int i = firstCol + 1; i < schematic[firstRow].Length && char.IsDigit(schematic[firstRow][i]); i++)
                                first.Append(schematic[firstRow][i]);
                            for (int i = firstCol - 1; i >= 0 && char.IsDigit(schematic[firstRow][i]); i--)
                                first.Insert(0, schematic[firstRow][i]);

                            var second = new StringBuilder(schematic[secondRow][secondCol].ToString());

                            for (int i = secondCol + 1; i < schematic[secondRow].Length && char.IsDigit(schematic[secondRow][i]); i++)
                                second.Append(schematic[secondRow][i]);
                            for (int i = secondCol - 1; i >= 0 && char.IsDigit(schematic[secondRow][i]); i--)
                                second.Insert(0, schematic[secondRow][i]);

                            sum += int.Parse(first.ToString()) * int.Parse(second.ToString());
                        }
                    }
                }
            }
            return sum;
        }
        static List<char> GetSurroundingChars(string prevLine, string nextLine, string currLine, int column)
        {
            var list = new List<char>()
            {
                prevLine[column],
                nextLine[column]
            };

            if (column != 0)
            {
                list.Add(currLine[column - 1]);
                list.Add(prevLine[column - 1]);
                list.Add(nextLine[column - 1]);
            }
            if (column != currLine.Length - 1)
            {
                list.Add(currLine[column + 1]);
                list.Add(prevLine[column + 1]);
                list.Add(nextLine[column + 1]);
            }
            return list;
        }
    }
}
