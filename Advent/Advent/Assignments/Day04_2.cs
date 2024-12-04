using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Assignments
{
    internal class Day04_2 : IAssignment
    {
        private class ShapeMatcher3x3
        {
            private char[] _chars = new char[3 * 3];

            public ShapeMatcher3x3(string shape)
            {
                var chars = shape.Split(Environment.NewLine).SelectMany(s => s).ToArray();
                chars.CopyTo(_chars, 0);
            }

            public bool Match(Point point, CharGrid grid)
            {
                for (var i = 0; i < _chars.Length; i++)
                {
                    if (_chars[i] == '.')
                        continue;

                    var x = i % 3 + point.x;
                    var y = i / 3 + point.y;
                    if (x >= grid.Width)
                        return false;
                    if (y >= grid.Height)
                        return false;

                    var index = x + y * grid.Height;
                    var gridChar = grid.Chars[index];
                    if (gridChar != _chars[i])
                        return false;
                }
                return true;
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            ShapeMatcher3x3[] matchers = [
                new ShapeMatcher3x3("""
                                    M.M
                                    .A.
                                    S.S
                                    """),
                new ShapeMatcher3x3("""
                                    M.S
                                    .A.
                                    M.S
                                    """),
                new ShapeMatcher3x3("""
                                    S.S
                                    .A.
                                    M.M
                                    """),
                new ShapeMatcher3x3("""
                                    S.M
                                    .A.
                                    S.M
                                    """),
            ];
            var grid = new CharGrid(input);
            var matchCount = 0;
            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    foreach (var matcher in matchers)
                    {
                        if (matcher.Match(new Point(x, y), grid))
                            matchCount++;
                    }
                }
            }
            return matchCount.ToString();
        }
    }
}
