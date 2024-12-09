using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day09_2 : IAssignment
    {
        private record struct File(ushort Id, LineRange Blocks);

        public string Run(IReadOnlyList<string> input)
        {
            var diskMap = input[0];
            var files = new File[diskMap.Length / 2 + 1];
            var blockCount = 0;
            ushort fileId = 0;
            var firstEmptyIndex = -1;
            for (var i = 0; i < diskMap.Length; i += 2)
            {
                var fileLength = diskMap[i] - '0';
                files[fileId] = new File(fileId, new LineRange(blockCount, fileLength));
                blockCount += fileLength;
                fileId++;

                if (i + 1 < diskMap.Length)
                {
                    var emptyLength = diskMap[i + 1] - '0';
                    if (firstEmptyIndex < 0)
                        firstEmptyIndex = blockCount;
                    blockCount += emptyLength;
                }
            }

            // Defrag that shit
            for (var i = fileId - 1; i > 0; i--)
            {
                // Try to fit the file somewhere between our leftmost empty block and the file's current position
                for (var k = firstEmptyIndex; k < files[i].Blocks.start; k++)
                {
                    var newFileBlock = new LineRange(k, files[i].Blocks.Length);
                    var overlapsAny = false;
                    for (var n = 0; n < files.Length && !overlapsAny; n++)
                    {
                        if (newFileBlock.Overlaps(files[n].Blocks))
                        {
                            overlapsAny = true;
                            break;
                        }
                    }

                    if (!overlapsAny)
                    {
                        files[i].Blocks = newFileBlock;
                    }
                }
            }

            var sum = 0L;
            for (var i = 0; i < files.Length; i++)
            {
                for (var k = files[i].Blocks.start; k < files[i].Blocks.end; k++)
                    sum += files[i].Id * k;
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
