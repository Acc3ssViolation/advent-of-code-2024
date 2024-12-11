using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Assignments
{
    internal class Day11_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var stones = new Dictionary<long, long>();
            var initialStones = input[0].ExtractInts();
            foreach (var stone in initialStones)
            {
                if (!stones.TryGetValue(stone, out var count))
                    count = 0;
                stones[stone] = count + 1;
            }

            var stoneCount = 0L;
            for (var round = 1; round <= 75; round++)
            {
                stoneCount = 0L;
                // TODO: Use two dicts and swap their contents?
                var list = stones.ToList();
                stones.Clear();
                foreach (var stone in list)
                {
                    checked
                    {
                        static void AddStone(Dictionary<long, long> stones, long number, long add)
                        {
                            if (!stones.TryGetValue(number, out var count))
                                count = 0;
                            stones[number] = count + add;
                        }

                        var number = stone.Key;
                        var count = stone.Value;
                        if (number == 0)
                        {
                            // 0 turns into 1
                            AddStone(stones, 1, count);
                            stoneCount += count;
                        }
                        else
                        {
                            var digits = NumberOfDigits(number);
                            if (digits % 2 == 0)
                            {
                                // Split into two stones
                                var divisor = (long)Math.Pow(10, digits / 2);
                                var left = number / divisor;
                                var right = number % divisor;
                                AddStone(stones, left, count);
                                AddStone(stones, right, count);
                                stoneCount += count * 2;
                            }
                            else
                            {
                                // Multiply value by 2024
                                AddStone(stones, number * 2024, count);
                                stoneCount += count;
                            }
                        }
                    }
                }

                //Logger.DebugLine($"Round {round}");
                //foreach (var stone in stones)
                //{
                //    Logger.DebugLine($"[{stone.Key}]={stone.Value}");
                //}
            }

            return stoneCount.ToString();
        }

        static int NumberOfDigits(long value)
        {
            if (value < 10)
                return 1;
            if (value < 100)
                return 2;
            if (value < 1000)
                return 3;
            if (value < 10000)
                return 4;
            if (value < 100000)
                return 5;
            if (value < 1000000)
                return 6;
            if (value < 10000000)
                return 7;
            if (value < 100000000)
                return 8;
            if (value < 1000000000)
                return 9;
            if (value < 10000000000)
                return 10;
            if (value < 100000000000)
                return 11;
            if (value < 1000000000000)
                return 12;
            if (value < 10000000000000)
                return 13;
            if (value < 100000000000000)
                return 14;
            if (value < 1000000000000000)
                return 15;
            if (value < 10000000000000000)
                return 16;
            if (value < 100000000000000000)
                return 17;
            if (value < 1000000000000000000)
                return 18;
            return 19;
        }
    }
}
