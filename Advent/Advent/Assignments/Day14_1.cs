using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day14_1 : IAssignment
    {
        private struct Robot
        {
            public Point pos;
            public Point vel;
        }

        public string Run(IReadOnlyList<string> input)
        {
            var robots = new Robot[input.Count];
            var robotCount = 0;
            foreach (var line in input)
            {
                var numbers = line.ExtractInts();
                robots[robotCount++] = new Robot { pos = new Point(numbers[0], numbers[1]), vel = new Point(numbers[2], numbers[3]) };
            }

            var width = input.Count > 20 ? 101 : 7;
            var height = input.Count > 20 ? 103 : 11;
            var halfWidth = width / 2;
            var halfHeight = height / 2;
            var size = new Point(width, height);

            var dt = 100;

            var quadrants = new int[4];

            for (var i = 0; i < robots.Length; i++)
            {
                var pos = robots[i].pos + robots[i].vel * dt;
                pos = pos.Wrap(size);

                if (pos.x == halfWidth || pos.y == halfHeight)
                    continue;

                var quadrant = 0;
                if (pos.y > halfHeight)
                    quadrant += 2;
                if (pos.x > halfWidth)
                    quadrant += 1;
                quadrants[quadrant]++;
            }

            var safetyScore = quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3];

            return safetyScore.ToString();
        }
    }
}
