using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day06_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var grid = new CharGrid(input);
            var pos = grid.Find('^');
            grid[pos] = '.';
            var dir = Direction.North;

            var loops = FindLoops(grid, pos, dir);
            Debug.Assert(loops == grid.Count('O'));
            //Logger.DebugLine(grid.ToString());

            return loops.ToString();
        }

        private static int FindLoops(CharGrid grid, Point pos, Direction dir)
        {
            var pathPositions = new HashSet<Point>();
            var visitedPositions = new HashSet<(Point, Direction)>();
            var iterations = 0;
            var loops = 0;
            while (grid.InBounds(pos))
            {
                if (iterations++ >= 100000)
                {
                    throw new TimeoutException();
                }
                pathPositions.Add(pos);
                var target = pos + dir.ToVector();
                if (grid.InBounds(target))
                {
                    var tile = grid[target];
                    if (tile == '#')
                    {
                        dir = dir.Right();
                        continue;
                    }
                    else if (tile != 'O' && !pathPositions.Contains(target))
                    {
                        // Place a wall in front of us
                        grid[target] = '#';
                        // Find a loop
                        if (HasLoop(visitedPositions, grid, pos, dir))
                        {
                            // Yes loop, mark it and count it!
                            grid[target] = 'O';
                            loops++;
                        }
                        else
                        {
                            // No loop, restore the map
                            grid[target] = tile;
                        }
                    }

                    pos = target;
                }
                else
                {
                    break;
                }
            }

            return loops;
        }

        private static bool HasLoop(HashSet<(Point, Direction)> visitedPositions, CharGrid grid, Point pos, Direction dir)
        {
            visitedPositions.Clear();
            visitedPositions.Add((pos, dir));
            var iterations = 0;
            while (grid.InBounds(pos))
            {
                if (iterations++ >= 100000)
                {
                    throw new TimeoutException();
                }

                var target = pos + dir.ToVector();
                if (grid.InBounds(target))
                {
                    var tile = grid[target];
                    if (tile == '#')
                    {
                        dir = dir.Right();
                        visitedPositions.Add((pos, dir));
                        continue;
                    }

                    pos = target;
                    if (visitedPositions.Contains((pos, dir)))
                        return true;

                    visitedPositions.Add((pos, dir));
                }
                else
                {
                    break;
                }
            }

            return false;
        }
    }
}
