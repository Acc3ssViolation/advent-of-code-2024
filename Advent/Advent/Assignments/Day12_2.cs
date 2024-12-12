using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Advent.Assignments
{
    internal class Day12_2 : IAssignment
    {
        private struct Region
        {
            public char plant;
            public int area;
            public int sides;

            public override readonly string ToString()
            {
                return $"A region of {plant} plants with price {area} * {sides} = {area * sides}";
            }
        }

        private struct SimpleList<T>
        {
            public T[] data;
            public int length;

            public void Add(T item)
            {
                data ??= ArrayPool<T>.Shared.Rent(256);
                data[length++] = item;
            }

            public void Clear()
            {
                length = 0;
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var height = input.Count;
            var width = input[0].Length;
            var regions = new Region[width * height / 5];
            var regionTiles = new SimpleList<int>[regions.Length];
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

                    if (x > 0)
                    {
                        var leftIndex = (x - 1) + y * width;
                        var leftRegionIndex = areaMap[leftIndex];
                        if (regions[leftRegionIndex].plant == plant)
                        {
                            // Attach to the left region
                            regionIndex = leftRegionIndex;
                        }
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

                                ref var topTiles = ref regionTiles[topRegionIndex];
                                ref var tiles = ref regionTiles[regionIndex];
                                for (var i = 0; i < topTiles.length; i++)
                                {
                                    var point = topTiles.data[i];
                                    areaMap[point] = regionIndex;
                                    tiles.Add(point);
                                }
                                regions[topRegionIndex].area = 0;
                                topTiles.Clear();
                            }
                        }
                    }

                    areaMap[x + y * width] = regionIndex;
                    regionTiles[regionIndex].Add(x + y * width);
                    regions[regionIndex].plant = plant;
                    regions[regionIndex].area += 1;
                    if (regionIndex == regionCount)
                        regionCount++;
                }
            }

            // Scan horizontal edges
            for (var y = 0; y < height; y++)
            {
                var topEdgeLength = 0;
                var bottomEdgeLength = 0;
                var previousRegionId = -1;

                for (var x = 0; x < width; x++)
                {
                    var regionId = areaMap[x + y * width];
                    var topRegionId = y > 0 ? areaMap[x + (y - 1) * width] : -1;
                    var bottomRegionId = y < height - 1 ? areaMap[x + (y + 1) * width] : -1;

                    if (regionId != previousRegionId)
                    {
                        // Moved to a new region, add pending counts to previous region
                        if (topEdgeLength != 0)
                        {
                            regions[previousRegionId].sides++;
                            topEdgeLength = 0;
                        }
                        if (bottomEdgeLength != 0)
                        {
                            regions[previousRegionId].sides++;
                            bottomEdgeLength = 0;
                        }
                    }

                    if (regionId != topRegionId)
                    {
                        // There is an edge to the top of this tile
                        topEdgeLength++;
                    }
                    else if (topEdgeLength != 0)
                    {
                        // Edge ended because we are below ourselves
                        regions[regionId].sides++;
                        topEdgeLength = 0;
                    }

                    if (regionId != bottomRegionId)
                    {
                        // There is an edge on the bottom of this tile
                        bottomEdgeLength++;
                    }
                    else if (bottomEdgeLength != 0)
                    {
                        // Edge ended because we are above ourselves
                        regions[regionId].sides++;
                        bottomEdgeLength = 0;
                    }

                    previousRegionId = regionId;
                }

                if (topEdgeLength != 0)
                {
                    // Don't forget pending edge on the last tile
                    regions[previousRegionId].sides++;
                }

                if (bottomEdgeLength != 0)
                {
                    // Don't forget pending edge on the last tile
                    regions[previousRegionId].sides++;
                }
            }

            // Scan vertical edges
            for (var x = 0; x < width; x++)
            {
                var leftEdgeLength = 0;
                var rightEdgeLength = 0;
                var previousRegionId = -1;

                for (var y = 0; y < height; y++)
                {
                    var regionId = areaMap[x + y * width];
                    var leftRegionId = x > 0 ? areaMap[x - 1 + y * width] : -1;
                    var rightRegionId = x < width - 1 ? areaMap[x + 1 + y * width] : -1;

                    if (regionId != previousRegionId)
                    {
                        // Moved to a new region, add pending counts to previous region
                        if (leftEdgeLength != 0)
                        {
                            regions[previousRegionId].sides++;
                            leftEdgeLength = 0;
                        }
                        if (rightEdgeLength != 0)
                        {
                            regions[previousRegionId].sides++;
                            rightEdgeLength = 0;
                        }
                    }

                    if (regionId != leftRegionId)
                    {
                        // There is an edge to the top of this tile
                        leftEdgeLength++;
                    }
                    else if (leftEdgeLength != 0)
                    {
                        // Edge ended because we are below ourselves
                        regions[regionId].sides++;
                        leftEdgeLength = 0;
                    }

                    if (regionId != rightRegionId)
                    {
                        // There is an edge on the bottom of this tile
                        rightEdgeLength++;
                    }
                    else if (rightEdgeLength != 0)
                    {
                        // Edge ended because we are above ourselves
                        regions[regionId].sides++;
                        rightEdgeLength = 0;
                    }

                    previousRegionId = regionId;
                }

                if (leftEdgeLength != 0)
                {
                    // Don't forget pending edge on the last tile
                    regions[previousRegionId].sides++;
                }

                if (rightEdgeLength != 0)
                {
                    // Don't forget pending edge on the last tile
                    regions[previousRegionId].sides++;
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
                    price += regions[i].area * regions[i].sides;
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
