using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day05_2 : IAssignment
    {
        private record SortRule(int Left, int Right);

        public string Run(IReadOnlyList<string> input)
        {
            var rules = new Dictionary<int, SortRule>();
            var i = 0;
            // Parse rules
            while (i < input.Count)
            {
                var line = input[i];
                i++;
                if (line.Length == 0)
                    break;

                var left = int.Parse(line.AsSpan(0, 2));
                var right = int.Parse(line.AsSpan(3, 2));
                rules.Add(GetKey(left, right), new SortRule(left, right));
            }
            // Parse updates
            var sum = 0;
            while (i < input.Count)
            {
                var line = input[i];
                i++;

                // Can probably optimize the parsing code considering the format
                var pages = line.ExtractInts();
                if (!IsSorted(pages, rules))
                {
                    pages.Sort((left, right) =>
                    {
                        if (!rules.TryGetValue(GetKey(left, right), out var rule))
                            return 0;

                        if (rule.Left == left)
                            return -1;

                        return 1;
                    });
                    Debug.Assert(IsSorted(pages, rules));
                    var center = pages[pages.Count / 2];
                    sum += center;
                }
            }

            return sum.ToString();
        }

        private static int GetKey(int a, int b)
        {
            if (b < a)
                (a, b) = (b, a);

            return a | (b << 8);
        }

        private static bool IsSorted(List<int> pages, Dictionary<int, SortRule> rules)
        {
            for (var i = 1; i < pages.Count; i++)
            {
                var left = pages[i - 1];
                var right = pages[i];
                if (rules.TryGetValue(GetKey(left, right), out var rule))
                {
                    if (left != rule.Left)
                    {
                        Debug.Assert(left == rule.Right);
                        Debug.Assert(right == rule.Left);
                        return false;
                    }

                    Debug.Assert(left == rule.Left);
                    Debug.Assert(right == rule.Right);
                }
            }
            return true;
        }
    }
}
