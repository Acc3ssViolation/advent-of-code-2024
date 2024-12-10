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
        private struct Gap(ushort Prev, ushort Next, LineRange Blocks)
        {
            public ushort Prev = Prev;
            public ushort Next = Next;
            public LineRange Blocks = Blocks;
        }

        public string Run(IReadOnlyList<string> input)
        {
            var diskMap = input[0].ToCharArray();
            var diskMapLength = diskMap.Length;
            var files = new File[diskMapLength / 2 + 1];
            var gaps = new Gap[diskMapLength / 2 + 1];
            var gapCount = 0;
            var blockCount = 0;
            ushort fileId = 0;
            for (var i = 0; i < diskMapLength; i += 2)
            {
                var fileLength = diskMap[i] - '0';
                Debug.Assert(fileLength > 0);
                files[fileId] = new File(fileId, new LineRange(blockCount, fileLength));
                blockCount += fileLength;
                fileId++;

                if (i + 1 < diskMapLength)
                {
                    var emptyLength = diskMap[i + 1] - '0';
                    if (emptyLength == 0)
                        continue;
                    gaps[gapCount] = new Gap((ushort)(gapCount - 1), (ushort)(gapCount + 1), new LineRange(blockCount, emptyLength));
                    blockCount += emptyLength;
                    gapCount++;
                }
            }

            // Defrag that shit
            var firstGap = 0;
            for (var i = fileId - 1; i > 0; i--)
            {
                // Try to fit the file somewhere between our leftmost empty block and the file's current position
                var fileLength = files[i].Blocks.Length;
                for (var g = firstGap; g < gapCount;)
                {
                    if (gaps[g].Blocks.Length < fileLength)
                    {
                        // File doesn't fit in this gap, try the next gap
                        g = gaps[g].Next;
                    }
                    else if (gaps[g].Blocks.end > files[i].Blocks.start)
                    {
                        // Gap ends after the start of the block, we ran out of gaps to check
                        break;
                    }
                    else
                    {
                        // Gap can contain this file
                        ref var gap = ref gaps[g];

                        files[i].Blocks = new LineRange(gap.Blocks.start, fileLength);
                        if (gap.Blocks.Length > fileLength)
                        {
                            // Still space left in this gap
                            gap.Blocks.start += fileLength;
                        }
                        else
                        {
                            // No more space!
                            // Remove ourselves from the linked list (except for the first gap, which has invalid pointers)
                            if (g != 0)
                            {
                                gaps[gap.Prev].Next = gap.Next;
                                gaps[gap.Next].Prev = gap.Prev;
                            }

                            // If this was the left-most gap then we can start the next searches further in the list
                            // because there are no more empty gaps here or to our left.
                            if (g == firstGap)
                                firstGap = gap.Next;
                        }
                        break;
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

        private static void FilesToBlocks(Span<File> files, Span<ushort> blocks)
        {
            blocks.Fill(0xFFFF);
            for (var i = 0; i < files.Length; i++)
            {
                for (var k = files[i].Blocks.start; k < files[i].Blocks.end; k++)
                    blocks[k] = files[i].Id;
            }
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
