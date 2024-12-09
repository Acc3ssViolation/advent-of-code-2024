using Advent.Shared;
using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day06_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var grid = new CharGrid(input);
            var pos = grid.Find('^');
            var dir = Direction.North;
            var count = 0;
            
            while (grid.InBounds(pos))
            {
                var target = pos + dir.ToVector();
                if (grid.InBounds(target))
                {
                    var tile = grid[target];
                    if (tile == '#')
                    {
                        dir = dir.Right();
                        continue;
                    }
                    else if (tile == '.')
                    {
                        count++;
                        grid[target] = 'X';
                    }

                    pos = target;
                }
                else
                {
                    count++;
                    break;
                }
            }

            return count.ToString();
        }
    }
}
