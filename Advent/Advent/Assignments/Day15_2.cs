using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Assignments
{
    internal class Day15_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var rows = new List<string>();
            for (var i = 0; i < input.Count; i++)
            {
                if (input[i].Length == 0)
                    break;

                rows.Add(input[i].Replace(".", "..").Replace("O", "[]").Replace("#", "##").Replace("@", "@."));
            }
            var sb = new StringBuilder();
            var instructions = input.Skip(rows.Count + 1).Aggregate("", (a, b) => a + b);
            var grid = new CharGrid(rows);
            //Logger.DebugLine(grid.ToString());
            var robot = grid.Find('@');
            grid[robot] = '.';

            //var boxCount = grid.Count('[');

            for (var i = 0; i < instructions.Length; i++)
            {
                //Logger.DebugLine($"Move {instructions[i]}:");

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
                    case '[':
                    case ']':
                        {
                            static Point GetBoxPosition(Point point, char value)
                            {
                                return value == '[' ? point : point + Point.West;
                            }

                            static bool BoxCanMove(CharGrid grid, Point boxPosition, Point dir)
                            {
                                if (dir.y != 0)
                                {
                                    // Vertical movement
                                    var leftPos = boxPosition + dir;
                                    var rightPos = boxPosition + dir;
                                    rightPos.x++;

                                    var leftTile = grid[leftPos];
                                    var rightTile = grid[rightPos];

                                    if (leftTile == '.' && rightTile == '.')
                                        return true;

                                    if (leftTile == '#' || rightTile == '#')
                                        return false;

                                    if (leftTile == '.')
                                    {
                                        // Box to our upper-right
                                        return BoxCanMove(grid, GetBoxPosition(rightPos, rightTile), dir);
                                    }
                                    else if (rightTile == '.')
                                    {
                                        // Box to our upper-left
                                        return BoxCanMove(grid, GetBoxPosition(leftPos, leftTile), dir);
                                    }
                                    else
                                    {
                                        // Two boxes above us!
                                        return 
                                            BoxCanMove(grid, GetBoxPosition(rightPos, rightTile), dir) && 
                                            BoxCanMove(grid, GetBoxPosition(leftPos, leftTile), dir);
                                    }
                                }
                                else
                                {
                                    // Horizontal movement
                                    var pos = boxPosition + dir;
                                    if (dir.x > 0)
                                        pos.x++;
                                    var tile = grid[pos];
                                    if (tile == '.')
                                        return true;

                                    if (tile == '#')
                                        return false;

                                    return BoxCanMove(grid, GetBoxPosition(pos, tile), dir);
                                }
                            }

                            static void MoveBox(CharGrid grid, Point boxPosition, Point dir)
                            {
                                if (dir.y != 0)
                                {
                                    // Vertical movement
                                    var leftPos = boxPosition + dir;
                                    var rightPos = boxPosition + dir + Point.East;

                                    var leftTile = grid[leftPos];

                                    if (leftTile == '[' || leftTile == ']')
                                    {
                                        // Box to our upper-left
                                        MoveBox(grid, GetBoxPosition(leftPos, leftTile), dir);
                                    }

                                    var rightTile = grid[rightPos];

                                    if (rightTile == '[' || rightTile == ']')
                                    {
                                        // Box to our upper-right
                                        MoveBox(grid, GetBoxPosition(rightPos, rightTile), dir);
                                    }

                                    grid[boxPosition] = '.';
                                    grid[boxPosition + Point.East] = '.';

                                    grid[leftPos] = '[';
                                    grid[rightPos] = ']';
                                }
                                else
                                {
                                    // Horizontal movement
                                    var pos = boxPosition + dir;
                                    if (dir.x > 0)
                                        pos.x++;
                                    var tile = grid[pos];

                                    if (tile == '[' || tile == ']')
                                    {
                                        MoveBox(grid, GetBoxPosition(pos, tile), dir);
                                    }

                                    if (dir.x > 0)
                                    {
                                        grid[boxPosition] = '.';
                                        grid[boxPosition + Point.East] = '[';
                                        grid[boxPosition + (Point.East * 2)] = ']';
                                    }
                                    else
                                    {
                                        grid[boxPosition + Point.East] = '.';
                                        grid[boxPosition] = ']';
                                        grid[boxPosition + Point.West] = '[';
                                    }
                                }
                            }

                            var boxPos = GetBoxPosition((robot + dir), targetObj);
                            if (BoxCanMove(grid, boxPos, dir))
                            {
                                MoveBox(grid, boxPos, dir);
                                robot += dir;
                            }

                            //Debug.Assert(grid.Count(']') == grid.Count('['));
                            //Debug.Assert(boxCount == grid.Count(']'));
                        }
                        break;
                }

                //grid[robot] = '@';
                //Logger.DebugLine(grid.ToString());
                //grid[robot] = '.';
            }

            var sum = 0;
            var gridSize = grid.Width * grid.Height;
            var postBoxCount = 0;
            for (var i = 0; i < gridSize; i++)
            {
                if (grid.Chars[i] == '[')
                {
                    var x = i % grid.Width;
                    var y = i / grid.Width;
                    var gps = 100 * y + x;
                    sum += gps;
                    postBoxCount++;
                }
            }

            //Logger.DebugLine(grid.ToString());

            return sum.ToString();
        }
    }
}
