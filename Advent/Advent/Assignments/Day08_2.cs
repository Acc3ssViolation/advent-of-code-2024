using Advent.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day08_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var antennas = new Dictionary<char, List<Point>>();
            var width = input[0].Length;
            var height = input.Count;

            var y = 0;
            foreach (var mapRow in input)
            {
                var x = 0;
                foreach (var tile in mapRow)
                {
                    if (tile != '.')
                    {
                        if (!antennas.TryGetValue(tile, out var antennaLocations))
                        {
                            antennaLocations = new List<Point>();
                            antennas[tile] = antennaLocations;
                        }
                        antennaLocations.Add(new Point(x, y));
                    }
                    x++;
                }
                y++;
            }

            var sum = 0;

            // TODO: We could do this in parallel if required
            var markedPoints = new BitArray(width * height);
            foreach (var antenna in antennas)
            {
                var locations = antenna.Value;
                Debug.Assert(locations.Count >= 2);
                for (var i = 0; i < locations.Count; i++)
                {
                    var a = locations[i];
                    for (var k = i + 1; k < locations.Count; k++)
                    {
                        var b = locations[k];
                        var delta = a - b;
                        var p1 = a;
                        var p2 = b;

                        while (p1.x >= 0 && p1.y >= 0 && p1.x < width && p1.y < height)
                        {
                            var index = p1.x + p1.y * width;
                            if (!markedPoints[index])
                            {
                                markedPoints[index] = true;
                                sum++;
                            }
                            p1 += delta;
                        }

                        while (p2.x >= 0 && p2.y >= 0 && p2.x < width && p2.y < height)
                        {
                            var index = p2.x + p2.y * width;
                            if (!markedPoints[index])
                            {
                                markedPoints[index] = true;
                                sum++;
                            }
                            p2 -= delta;
                        }
                    }
                }
            }

            return sum.ToString();
        }
    }
}
