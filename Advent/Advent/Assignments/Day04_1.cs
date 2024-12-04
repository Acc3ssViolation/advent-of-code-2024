using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Assignments
{
    internal class Day04_1 : IAssignment
    {
        private class ShapeMatcher4x4
        {
            private char[] _chars = new char[4 * 4];

            public ShapeMatcher4x4(string shape)
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

                    var x = i % 4 + point.x;
                    var y = i / 4 + point.y;
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

        private static readonly ShapeMatcher4x4[] Matchers = [
            new ShapeMatcher4x4("""
                                XMAS
                                ....
                                ....
                                ....
                                """),
            new ShapeMatcher4x4("""
                                X...
                                .M..
                                ..A.
                                ...S
                                """),
            new ShapeMatcher4x4("""
                                X...
                                M...
                                A...
                                S...
                                """),
            new ShapeMatcher4x4("""
                                ...X
                                ..M.
                                .A..
                                S...
                                """),
            new ShapeMatcher4x4("""
                                SAMX
                                ....
                                ....
                                ....
                                """),
            new ShapeMatcher4x4("""
                                S...
                                .A..
                                ..M.
                                ...X
                                """),
            new ShapeMatcher4x4("""
                                S...
                                A...
                                M...
                                X...
                                """),
            new ShapeMatcher4x4("""
                                ...S
                                ..A.
                                .M..
                                X...
                                """),
        ];

        public string Run(IReadOnlyList<string> input)
        {

            var grid = new CharGrid(input);
            var matchCount = 0;
            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    foreach (var matcher in Matchers)
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
