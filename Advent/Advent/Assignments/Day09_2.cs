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
            var gaps = new LineRange[diskMap.Length / 2 + 1];
            var gapCount = 0;
            var blockCount = 0;
            ushort fileId = 0;
            for (var i = 0; i < diskMap.Length; i += 2)
            {
                var fileLength = diskMap[i] - '0';
                Debug.Assert(fileLength > 0);
                files[fileId] = new File(fileId, new LineRange(blockCount, fileLength));
                blockCount += fileLength;
                fileId++;

                if (i + 1 < diskMap.Length)
                {
                    var emptyLength = diskMap[i + 1] - '0';
                    if (emptyLength == 0)
                        continue;
                    gaps[gapCount] = new LineRange(blockCount, emptyLength);
                    blockCount += emptyLength;
                    gapCount++;
                }
            }

            var blocks = new ushort[blockCount];
            //FilesToBlocks(files, blocks);
            //Logger.DebugLine(DiskToString(blocks));

            // Defrag that shit
            var firstGap = 0;
            for (var i = fileId - 1; i > 0; i--)
            {
                // Try to fit the file somewhere between our leftmost empty block and the file's current position
                var fileLength = files[i].Blocks.Length;
                var g = firstGap;
                var foundGap = false;
                for (; g < gapCount; g++)
                {
                    if (gaps[g].Length < fileLength)
                        continue;

                    if (gaps[g].end > files[i].Blocks.start)
                        break;

                    foundGap = true;
                    break;
                }

                if (!foundGap)
                    continue;

                files[i].Blocks = new LineRange(gaps[g].start, fileLength);
                gaps[g].start += fileLength;

                //FilesToBlocks(files, blocks);
                //Logger.DebugLine(DiskToString(blocks));
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
