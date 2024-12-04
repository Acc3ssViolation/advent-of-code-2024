using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day02_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var safeCount = 0;

            foreach (var report in input)
            {
                var readings = report.ExtractInts();

                var safe = IsSafeSequence(readings, true);
                if (safe)
                    safeCount++;

                // Sanity check
                //if (safe == false)
                //{
                //    for (var i = 0; i < readings.Count; i++)
                //    {
                //        if (IsSafeSequence(readings.CopyAndRemoveAt(i)))
                //            throw new Exception($"Test sequence found! [{report}] by removing [{i}]");
                //    }
                //}
            }

            return safeCount.ToString();
        }

        private static bool IsSafeSequence(List<int> sequence, bool allowFix = false)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(sequence.Count, 2);

            var sign = Math.Sign(sequence[1] - sequence[0]);
            if (sign == 0)
            {
                // Two of the same readings is not allowed!
                if (allowFix)
                {
                    if (IsSafeSequence(sequence.CopyAndRemoveAt(0)))
                    {
                        //Logger.DebugLine($"[{sequence.AggregateString()}] is safe with [0] removed");
                        return true;
                    }

                    if (IsSafeSequence(sequence.CopyAndRemoveAt(1)))
                    {
                        //Logger.DebugLine($"[{sequence.AggregateString()}] is safe with [1] removed");
                        return true;
                    }

                    //Logger.DebugLine($"[{sequence.AggregateString()}] is never safe");
                }

                return false;
            }

            for (var i = 1; i < sequence.Count; i++)
            {
                var delta = sequence[i] - sequence[i - 1];

                // The less than 1 case is already covered by the sign not being 0
                var faulty = Math.Sign(delta) != sign || Math.Abs(delta) > 3;
                if (faulty)
                {
                    if (allowFix)
                    {
                        if (IsSafeSequence(sequence.CopyAndRemoveAt(i)))
                        {
                            //Logger.DebugLine($"[{sequence.AggregateString()}] is safe with [{i}] removed");
                            return true;
                        }
                        else if (IsSafeSequence(sequence.CopyAndRemoveAt(i - 1)))
                        {
                            //Logger.DebugLine($"[{sequence.AggregateString()}] is safe with [{i - 1}] removed");
                            return true;
                        }
                        else if (i == 2 && IsSafeSequence(sequence.CopyAndRemoveAt(i - 2)))
                        {
                            //Logger.DebugLine($"[{sequence.AggregateString()}] is safe with [{i - 1}] removed");
                            return true;
                        }

                        //Logger.DebugLine($"[{sequence.AggregateString()}] is never safe");
                    }
                    return false;
                }
            }

            if (allowFix)
            {
                //Logger.DebugLine($"[{sequence.AggregateString()}] is always safe");
            }
            return true;
        }
    }
}
