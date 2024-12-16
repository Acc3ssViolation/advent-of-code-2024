using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day14_2 : IAssignment
    {
        private struct Robot
        {
            public Point pos;
            public Point vel;
        }

        public string Run(IReadOnlyList<string> input)
        {
            if (input.Count < 20)
                return "UWU";

            var robots = new Robot[input.Count];
            var robotCount = 0;
            foreach (var line in input)
            {
                var numbers = line.ExtractInts();
                robots[robotCount++] = new Robot { pos = new Point(numbers[0], numbers[1]), vel = new Point(numbers[2], numbers[3]) };
            }

            var width = 101;
            var height = 103;
            var size = new Point(width, height);

            var iterations = height * width;

            for (var r = 0; r < iterations; r++)
            {
                var tempRobots = new Robot[input.Count];
                for (var i = 0; i < robots.Length; i++)
                {
                    var pos = robots[i].pos + robots[i].vel * r;
                    pos = pos.Wrap(size);
                    tempRobots[i].pos = pos;
                }
                var xHisto = tempRobots.GroupBy(r => r.pos.x).ToDictionary(g => g.Key, g => g.Count());
                var yHisto = tempRobots.GroupBy(r => r.pos.y).ToDictionary(g => g.Key, g => g.Count());

                if (xHisto.Values.Any(v => v > width / 3) && yHisto.Values.Any(v => v > height / 3))
                {
                    // Possible tree?

                    Logger.DebugLine($"Round {r}");
                    Logger.DebugLine(ToMap(tempRobots, width, height));
                    return r.ToString();
                }
            }
            

            var safetyScore = "LOL";

            return safetyScore.ToString();
        }

        private static string ToMap(Robot[] robots, int width, int height)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var drawRobot = false;
                    for (var r = 0; r < robots.Length; r++)
                    {
                        if (robots[r].pos == new Point(x, y))
                        {
                            drawRobot = true;
                            break;
                        }
                    }
                    if (drawRobot)
                        sb.Append('#');
                    else
                        sb.Append('.');
                    //sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
