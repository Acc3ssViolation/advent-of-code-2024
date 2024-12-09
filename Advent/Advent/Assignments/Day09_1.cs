using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day09_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var diskMap = input[0];
            // I don't have the spare brain capacity at this point in time to bother with a sparse, range-based approach
            var blocks = new ushort[diskMap.Length * 9];
            var blockCount = 0;
            ushort fileId = 0;
            var firstEmptyIndex = -1;
            for (var i = 0; i < diskMap.Length; i += 2)
            {
                var fileLength = diskMap[i] - '0';
                for (var k = 0; k < fileLength; k++)
                {
                    blocks[k + blockCount] = fileId;
                }
                blockCount += fileLength;
                fileId++;

                if (i + 1 < diskMap.Length)
                {
                    var emptyLength = diskMap[i + 1] - '0';
                    for (var k = 0; k < emptyLength; k++)
                    {
                        blocks[k + blockCount] = 0xFFFF;
                    }
                    if (firstEmptyIndex < 0)
                        firstEmptyIndex = blockCount;
                    blockCount += emptyLength;
                }
            }

            // Defrag that shit
            for (var i = blockCount - 1; i > 0; i--)
            {
                // Ran out, exit
                if (firstEmptyIndex >= i)
                    break;

                if (blocks[i] != 0xFFFF)
                {
                    // Swap blocks
                    blocks[firstEmptyIndex] = blocks[i];
                    blocks[i] = 0xFFFF;

                    // Logger.DebugLine(DiskToString(blocks.AsSpan(0, blockCount)));

                    // Move up empty index
                    while (blocks[firstEmptyIndex] != 0xFFFF)
                    {
                        firstEmptyIndex++;
                    }
                }
            }

            var sum = 0L;
            for (var i = 0; i < firstEmptyIndex; i++)
            {
                var block = blocks[i];
                Debug.Assert(block != 0xFFFF);
                sum += block * i;                    
            }
            return sum.ToString();
        }

        private static string DiskToString(Span<ushort> blocks)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < blocks.Length; i++)
            {
                if (blocks[i] == 0xFFFF)
                {
                    sb.Append('.');
                }
                else
                {
                    sb.Append((char)((blocks[i] % 10) + '0'));
                }
            }
            return sb.ToString();
        }
    }
}
