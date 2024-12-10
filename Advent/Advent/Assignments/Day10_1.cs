using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day10_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var trailheads = new List<Point>(32);
            var grid = new CompactGrid(input[0].Length + 2, input.Count + 2);
            for (var y = 0; y < input.Count; y++)
            {
                var row = input[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var point = new Point(x + 1, y + 1);
                    var value = (byte)(row[x] - '0');
                    if (value == 0)
                        trailheads.Add(point);
                    grid[point] = value;
                }
            }

            var sum = 0;

            var peaks = new HashSet<Point>();
            var pointsToCheck = new Queue<Point>();
            foreach (var trailhead in trailheads)
            {
                peaks.Clear();
                pointsToCheck.Clear();
                pointsToCheck.Enqueue(trailhead);
                while (pointsToCheck.Count > 0)
                {
                    var point = pointsToCheck.Dequeue();
                    var elevation = grid[point];
                    if (elevation == 9 && !peaks.Contains(point))
                    {
                        sum++;
                        peaks.Add(point);
                    }
                    else
                    {
                        if (grid[point + Point.North] == elevation + 1)
                            pointsToCheck.Enqueue(point + Point.North);
                        if (grid[point + Point.East] == elevation + 1)
                            pointsToCheck.Enqueue(point + Point.East);
                        if (grid[point + Point.West] == elevation + 1)
                            pointsToCheck.Enqueue(point + Point.West);
                        if (grid[point + Point.South] == elevation + 1)
                            pointsToCheck.Enqueue(point + Point.South);
                    }
                }
            }

            return sum.ToString();
        }
    }
}
