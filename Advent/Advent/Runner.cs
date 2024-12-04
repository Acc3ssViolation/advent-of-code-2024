﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advent
{
    internal class Runner
    {
        private List<IAssignment> _assignments = new();

        public bool LogTimingToFile { get; set; }
        public int MaxDay { get; set; } = 100;
        public List<int> SkipDays { get; set; } = new();

        public void Add(IAssignment assignment) => _assignments.Add(assignment);

        public async Task PrepareAsync(DataDownloader downloader, CancellationToken cancellationToken)
        {
            // Set us up as a higher priority process 
            try
            {
                var process = Process.GetCurrentProcess();
                process.PriorityClass = ProcessPriorityClass.High;
                process.PriorityBoostEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.WarningLine($"Failed to setup high prio process: {ex}");
            }
            

            foreach (var assignment in _assignments)
            {
                var method = assignment.GetType().GetRuntimeMethods().First(m => m.Name == nameof(IAssignment.Run));
                RuntimeHelpers.PrepareMethod(method.MethodHandle);
                await downloader.DownloadDay(assignment.Day, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task RunTestsAsync(CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();

            var successful = 0;
            var failed = 0;
            var unknown = 0;
            // Do a run on the test data
            Logger.Line("=================================================", Logger.Green);
            Logger.Line("Running test data", Logger.Green);
            Logger.Line("=================================================", Logger.Green);
            foreach (var assingment in _assignments)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (assingment.Day > MaxDay)
                    break;

                if (SkipDays.Contains(assingment.Day))
                    continue;

                try
                {
                    var inputName = Path.Combine(PathManager.DataDirectory, assingment.TestFile.ToLowerInvariant());

                    if (!File.Exists(inputName))
                        continue;

                    Logger.DebugLine($"Loading test data from {inputName} for assignment {assingment.Name}");
                    var testLines = await File.ReadAllLinesAsync(inputName, cancellationToken).ConfigureAwait(false);

                    // Extract the result data from the file first
                    var lastHeaderLine = 0;
                    for (var i = 0; i < testLines.Length; i++)
                    {
                        var line = testLines[i];
                        if (line.Trim() == "data start")
                        {
                            lastHeaderLine = i;
                            break;
                        }
                    }
                    var resultLine = assingment.Part - 1;
                    var expectedResult = resultLine < lastHeaderLine ? testLines[resultLine].Trim() : string.Empty;

                    var lines = testLines.Skip(lastHeaderLine + 1).ToList();

                    GC.Collect();

                    Logger.Line($"Running assignment test {assingment.Name}");
                    stopwatch.Restart();
                    var result = assingment.Run(lines);
                    stopwatch.Stop();
                    Logger.Append($"Result of test {assingment.Name}: ");
                    if (expectedResult == string.Empty)
                    {
                        Logger.Line($"{result}", Logger.Yellow);
                        unknown++;
                    }
                    else if (result == expectedResult)
                    {
                        Logger.Line($"{result}", Logger.Green);
                        successful++;
                    }
                    else
                    {
                        Logger.Line($"{result} does not match expected result {expectedResult}", Logger.Red);
                        failed++;
                    }
                    Logger.Line($"Took {stopwatch.GetMilliseconds():F4} ms ({stopwatch.ElapsedTicks} ticks)");

                    if (LogTimingToFile)
                        await File.AppendAllTextAsync("timing.log", $"{assingment.Name} (test): {stopwatch.GetMilliseconds():F4}\n", cancellationToken).ConfigureAwait(false);
                }
#if !DEBUG
                catch (Exception ex)
                {
                    failed++;
                    stopwatch.Stop();
                    Logger.ErrorLine($"Exception when running assignment {assingment.Name}: {ex.Message}\n{ex.StackTrace}");
                    Logger.ErrorLine($"Took {stopwatch.GetMilliseconds():F4} ms ({stopwatch.ElapsedTicks} ticks)");
                }
#endif
                finally { }

                Logger.Line();
            }

            if (successful > 0)
                Logger.Line($"{successful} successful", Logger.Green);
            if (unknown > 0)
                Logger.Line($"{unknown} unknown", Logger.Yellow);
            if (failed > 0)
                Logger.Line($"{failed} failed", Logger.Red);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();

            // Do a run on the real data
            Logger.Line("=================================================", Logger.Yellow);
            Logger.Line("Running real data", Logger.Yellow);
            Logger.Line("=================================================", Logger.Yellow);
            foreach (var assingment in _assignments)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (assingment.Day > MaxDay)
                    break;

                if (SkipDays.Contains(assingment.Day))
                    continue;

                try
                {
                    var inputName = Path.Combine(PathManager.DataDirectory, assingment.InputFile.ToLowerInvariant());

                    Logger.DebugLine($"Loading data from {inputName} for assignment {assingment.Name}");
                    var lines = await File.ReadAllLinesAsync(inputName, cancellationToken).ConfigureAwait(false);

                    GC.Collect();

                    Logger.Line($"Running assignment {assingment.Name}");
                    stopwatch.Restart();
                    var result = assingment.Run(lines);
                    stopwatch.Stop();
                    Logger.Line($"Result of {assingment.Name}: {result}");
                    Logger.Line($"Took {stopwatch.GetMilliseconds():F4} ms ({stopwatch.ElapsedTicks} ticks)");
                    // Log runtime
                    await File.AppendAllTextAsync("timing.log", $"{assingment.Name}: {stopwatch.GetMilliseconds():F4}\n", cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    Logger.ErrorLine($"Exception when running assignment {assingment.Name}: {ex.Message}\n{ex.StackTrace}");
                    Logger.ErrorLine($"Took {stopwatch.GetMilliseconds():F4} ms ({stopwatch.ElapsedTicks} ticks)");
#if DEBUG
                    throw;
#endif
                }

                Logger.Line();
            }
        }
    }
}
