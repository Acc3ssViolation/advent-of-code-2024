using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day02_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var safeCount = 0;

            foreach (var report in input)
            {
                var readings = report.ExtractInts();

                Debug.Assert(readings.Count >= 2);
                var sign = Math.Sign(readings[1] - readings[0]);
                if (sign == 0)
                {
                    // Two of the same readings is not allowed!
                    continue;
                }

                var safe = true;
                for (var i = 1; safe && i < readings.Count; i++)
                {
                    var delta = readings[i] - readings[i - 1];

                    if (Math.Sign(delta) != sign)
                    {
                        // Sequence changed direction!
                        safe = false;
                        break;
                    }

                    var absDelta = Math.Abs(delta);
                    if (absDelta > 3)
                    {
                        // Too big of a change!
                        safe = false;
                        break;
                    }

                    // The less than 1 case is already covered by the sign not being 0
                }

                if (safe)
                    safeCount++;
            }

            return safeCount.ToString();
        }
    }
}
