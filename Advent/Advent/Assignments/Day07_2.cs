using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day07_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var sum = 0L;
            foreach (var line in input)
            {
                var rawNumbers = line.ExtractLongs();
                var result = rawNumbers[0];
                rawNumbers.RemoveAt(0);
                var numbers = rawNumbers.ToArray().AsSpan();

                if (EqualsThing(result, 0, numbers))
                {
                    sum += result;
                }
            }

            return sum.ToString();
        }

        private static bool EqualsThing(long result, long left, Span<long> numbers)
        {
            if (left > result)
                return false;
            if (numbers.Length == 0)
                return left == result;

            var right = numbers[0];
            var nextNumbers = numbers[1..];
            if (EqualsThing(result, left + right, nextNumbers))
                return true;
            if (EqualsThing(result, left * right, nextNumbers))
                return true;
            var power = (long)Math.Log10(right);
            if (EqualsThing(result, left * (long)Math.Pow(10, power + 1) + right, nextNumbers))
                return true;
            return false;
        }
    }
}
