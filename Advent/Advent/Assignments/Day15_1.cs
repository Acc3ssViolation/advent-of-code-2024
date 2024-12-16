using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Assignments
{
    internal class Day15_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var rows = new List<string>();
            for (var i = 0; i < input.Count; i++)
            {
                if (input[i].Length == 0)
                    break;

                rows.Add(input[i]);
            }
            var sb = new StringBuilder();
            var instructions = input.Skip(rows.Count + 1).Aggregate("", (a, b) => a + b);
            var grid = new CharGrid(rows);
            var robot = grid.Find('@');
            grid[robot] = '.';

            for (var i = 0; i < instructions.Length; i++)
            {
                var dir = instructions[i] switch
                {
                    '^' => Point.North,
                    '>' => Point.East,
                    'v' => Point.South,
                    '<' => Point.West,
                    _ => throw new NotImplementedException()
                };

                var targetObj = grid[robot + dir];
                switch (targetObj)
                {
                    case '#':
                        break;
                    case '.':
                        robot += dir;
                        break;
                    case 'O':
                        {
                            var start = robot + dir;
                            var pos = robot + (dir * 2);
                            while (grid.InBounds(pos))
                            {
                                targetObj = grid[pos];
                                if (targetObj == '.' || targetObj == '#')
                                    break;
                                pos += dir;
                            }

                            if (targetObj == '.')
                            {
                                while (pos != start)
                                {
                                    grid[pos] = 'O';
                                    pos -= dir;
                                }
                                grid[start] = '.';
                                robot = start;
                            }
                        }
                        break;
                }

                //grid[robot] = '@';
                //Logger.DebugLine($"Move {instructions[i]}:");
                //Logger.DebugLine(grid.ToString());
                //grid[robot] = '.';
            }

            var sum = 0;
            var gridSize = grid.Width * grid.Height;
            for (var i = 0; i < gridSize; i++)
            {
                if (grid.Chars[i] == 'O')
                {
                    var x = i % grid.Width;
                    var y = i / grid.Width;
                    var gps = 100 * y + x;
                    sum += gps;
                }
            }

            //Logger.DebugLine(grid.ToString());

            return sum.ToString();
        }
    }
}
