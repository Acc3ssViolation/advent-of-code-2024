using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day07_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var sum = 0L;
            foreach (var line in input)
            {
                var numbers = line.ExtractLongs();
                var result = numbers[0];
                numbers.RemoveAt(0);

                var operatorCount = numbers.Count - 1;
                var combinationCount = 1 << operatorCount;
                
                for (var i = 0; i < combinationCount; i++)
                {
                    var left = numbers[0];
                    for (var n = 0; n < operatorCount; n++)
                    {
                        var right = numbers[n + 1];
                        if ((i & (1 << n)) == 0)
                        {
                            // Bit not set, do an add
                            left += right;
                        }
                        else
                        {
                            // Bit set, do a mul
                            left *= right;
                        }
                        if (left > result)
                            break;
                    }

                    if (left == result)
                    {
                        sum += result;
                        break;
                    }
                }
            }

            return sum.ToString();
        }
    }
}
