using System.Collections.Generic;
using System.Text;

namespace Advent.Assignments
{
    internal class Day12_1 : IAssignment
    {
        private struct Region
        {
            public char plant;
            public int area;
            public int edges;

            public override readonly string ToString()
            {
                return $"A region of {plant} plants with price {area} * {edges} = {area * edges}";
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var height = input.Count;
            var width = input[0].Length;
            var regions = new Region[width * height / 5];
            var areaMap = new int[width * height];
            var regionCount = 0;

            for (var y = 0; y < height; y++)
            {
                var row = input[y];
                if (row.Length != width)
                    return "";

                for (var x = 0; x < width; x++)
                {
                    var plant = row[x];
                    var regionIndex = regionCount;

                    var edges = 0;

                    if (x > 0)
                    {
                        var leftIndex = (x - 1) + y * width;
                        var leftRegionIndex = areaMap[leftIndex];
                        if (regions[leftRegionIndex].plant == plant)
                        {
                            // Attach to the left region
                            regionIndex = leftRegionIndex;
                        }
                        else
                        {
                            // Different type of region, count an edge
                            regions[leftRegionIndex].edges++;
                            edges++;
                        }

                        if (x + 1 == width)
                        {
                            // Edge of map always counts as an edge
                            edges++;
                        }
                    }
                    else
                    {
                        // x == 0, edge of map always counts as an edge
                        edges++;
                    }

                    if (y > 0)
                    {
                        var topIndex = x + (y - 1) * width;
                        var topRegionIndex = areaMap[topIndex];
                        if (regions[topRegionIndex].plant == plant)
                        {
                            if (regionIndex == regionCount)
                            {
                                // Attach to the top region
                                regionIndex = topRegionIndex;
                            }
                            else if (regionIndex != topRegionIndex)
                            {
                                // We need to merge the top and left regions!
                                // The left region is currently in regionIndex
                                regions[regionIndex].area += regions[topRegionIndex].area;
                                regions[regionIndex].edges += regions[topRegionIndex].edges;

                                for (var xx = 0; xx < width; xx++)
                                {
                                    for (var yy = 0; yy <= y; yy++)
                                    {
                                        if (areaMap[xx + yy * width] == topRegionIndex)
                                            areaMap[xx + yy * width] = regionIndex;
                                    }
                                }
                                regions[topRegionIndex].area = 0;
                                //regions[topRegionIndex].edges = 0;
                            }
                        }
                        else
                        {
                            // Different type of region, count an edge
                            regions[topRegionIndex].edges++;
                            edges++;
                        }

                        if (y + 1 == height)
                        {
                            // Edge of map always counts as an edge
                            edges++;
                        }
                    }
                    else
                    {
                        // y == 0, edge of map always counts as an edge
                        edges++;
                    }

                    areaMap[x + y * width] = regionIndex;
                    regions[regionIndex].plant = plant;
                    regions[regionIndex].area += 1;
                    regions[regionIndex].edges += edges;
                    if (regionIndex == regionCount)
                        regionCount++;
                }
            }

            // Combine prices
            //Logger.DebugLine(DisplayMap(areaMap, regions, width, height));
            var price = 0;
            checked
            {
                for (var i = 0; i < regionCount; i++)
                {
                    if (regions[i].area == 0)
                        continue;

                    //Logger.DebugLine(regions[i].ToString());
                    price += regions[i].area * regions[i].edges;
                }
            }
            
            //var usedRegions = regions.Where(r => r.area > 0).ToList();
            //Debug.Assert(usedRegions.Sum(r => r.area) == width * height);
            return price.ToString();
        }

        private static string DisplayMap(int[] areaMap, Region[] regions, int width, int height)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var regionIndex = areaMap[x + y * width];
                    sb.Append(regions[regionIndex].plant);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
