using Advent.Shared;
using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day13_1 : IAssignment
    {
        private readonly record struct ClawMachine(RealPoint A, RealPoint B, RealPoint Prize);

        public string Run(IReadOnlyList<string> input)
        {
            var globalTokens = 0;
            for (var i = 0; (i + 2) < input.Count; i += 4)
            {
                var a = input[i + 0].ExtractInts();
                var b = input[i + 1].ExtractInts();
                var p = input[i + 2].ExtractInts();
                var machine = new ClawMachine(new RealPoint(a[0], a[1]), new RealPoint(b[0], b[1]), new RealPoint(p[0], p[1]));
                var tokens = FindMinTokens(ref machine);
                globalTokens += tokens;
            }

            return globalTokens.ToString();
        }

        private static int FindMinTokens(ref ClawMachine machine)
        {
            var a = machine.A;
            var b = machine.B;
            var p = machine.Prize;

            // Just worked this out on paper, it's just a linear system with two variables N and M:
            // N * Ax + M * Bx = Px
            // N * Ay + M * By = Py
            // If N and M are integers it is solvable, otherwise it isn't

            var m = (p.y - ((a.y * p.x) / a.x)) / (b.y - ((a.y * b.x) / a.x));
            var n = (p.x / a.x) - (b.x / a.x) * m;

            var mInt = (int)Math.Round(m);
            var nInt = (int)Math.Round(n);
            if (Math.Abs(m - mInt) < 0.0000001 && Math.Abs(n - nInt) < 0.0000001)
            {
                //Logger.DebugLine($"{machine} | N = {nInt} | M = {mInt} | SOLVABLE");
                return nInt * 3 + mInt;
            }
            else
            {
                //Logger.DebugLine($"{machine} | N = {n} | M = {m} | IMPOSSIBLE");
                return 0;
            }
        }
    }
}
