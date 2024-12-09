using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day06_2 : IAssignment
    {
        internal class LoopFinder
        {
            public int ObstructionCount { get; private set; }

            private readonly CharGrid _grid;

            public LoopFinder(CharGrid grid)
            {
                _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            }

            private Point[] _corners = new Point[3];
            private int _index;
            private int _cornerCount = 0;
            private bool _waitOnCorner = false;

            public void OnCorner(Point newPosition)
            {
                if (_corners[GetOffset(2)] == newPosition)
                    return;
                _corners[_index] = newPosition;
                _index = (_index + 1) % _corners.Length;
                if (_cornerCount < 3)
                    _cornerCount++;
                _waitOnCorner = false;
            }

            public void OnMove(Point newPosition, Direction direction)
            {
                if (_cornerCount < 3 || _waitOnCorner)
                    return;

                var d2 = (_corners[GetOffset(1)] - _corners[GetOffset(0)]).Length;
                var d4 = (newPosition - _corners[GetOffset(2)]).Length;
                if (d4 >= d2)
                {
                    // Shortcut, just assume it will work lol
                    var newWall = newPosition + direction.ToVector();
                    if (_grid.InBounds(newWall))
                    {
                        ObstructionCount++;
                        _grid[newWall] = (char)('0' + ObstructionCount);
                    }

                    _waitOnCorner = true;
                }
            }

            private int GetOffset(int i)
            {
                return (_index + i) % _corners.Length;
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var grid = new CharGrid(input);
            var pos = grid.Find('^');
            var dir = Direction.North;
            var finder = new LoopFinder(grid);

            while (grid.InBounds(pos))
            {
                var target = pos + dir.ToVector();
                if (grid.InBounds(target))
                {
                    var tile = grid[target];
                    if (tile == '#')
                    {
                        dir = dir.Right();
                        finder.OnCorner(pos);
                        continue;
                    }
                    else if (tile == '.')
                    {
                        grid[target] = 'X';
                    }

                    pos = target;
                    finder.OnMove(target, dir);
                }
                else
                {
                    break;
                }
            }

            Logger.DebugLine(grid.ToString());

            return finder.ObstructionCount.ToString();
        }
    }
}
