﻿#define AUTO_LOAD_ASSIGNMENTS
using Advent;
using Advent.Assignments;
using System;
using System.IO;
using System.Linq;

var runner = new Runner();

const int Iterations = 1;
//Logger.SetLevel(LogLevel.Info);

#if AUTO_LOAD_ASSIGNMENTS
var interfaceType = typeof(IAssignment);
var all = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(x => x.GetTypes())
    .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
    .Select(x => Activator.CreateInstance(x));

foreach (var item in all)
{
    if (item is IAssignment assignment)
        runner.Add(assignment);
}

// Slow-ish days, skip during normal runs
runner.SkipDays.Add(14);
runner.SkipDays.Add(12);
//runner.MaxDay = 12;
#else
//runner.Add(new Day01_1());
//runner.Add(new Day01_2());
//runner.Add(new Day02_1());
//runner.Add(new Day02_2());
//runner.Add(new Day03_1());
//runner.Add(new Day03_2());
//runner.Add(new Day04_1());
//runner.Add(new Day04_2());
//runner.Add(new Day05_1());
//runner.Add(new Day05_2());
//runner.Add(new Day06_1());
//runner.Add(new Day06_2());
//runner.Add(new Day07_1());
//runner.Add(new Day07_2());
//runner.Add(new Day08_1());
//runner.Add(new Day08_2());
//runner.Add(new Day09_1());
//runner.Add(new Day09_2());
//runner.Add(new Day10_1());
//runner.Add(new Day10_2());
//runner.Add(new Day11_1());
//runner.Add(new Day11_2());
//runner.Add(new Day12_1());
//runner.Add(new Day12_2());
//runner.Add(new Day13_1());
//runner.Add(new Day13_2());
//runner.Add(new Day14_1());
//runner.Add(new Day14_2());
//runner.Add(new Day15_1());
//runner.Add(new Day15_2());
//runner.Add(new Day16_1());
//runner.Add(new Day16_2());
//runner.Add(new Day17_1());
//runner.Add(new Day17_2());
//runner.Add(new Day18_1());
//runner.Add(new Day18_2());
//runner.Add(new Day19_1());
//runner.Add(new Day19_2());
//runner.Add(new Day20_1());
//runner.Add(new Day20_2());
//runner.Add(new Day21_1());
//runner.Add(new Day21_2());
//runner.Add(new Day22_1());
//runner.Add(new Day22_2());
//runner.Add(new Day23_1());
//runner.Add(new Day23_2());
//runner.Add(new Day24_1());
//runner.Add(new Day24_2());
//runner.Add(new Day25_1());
//runner.Add(new Day25_2());
#endif

var cookie = await File.ReadAllTextAsync(Path.Combine(PathManager.DataDirectory, "cookie.txt"), default);
using var downloader = new DataDownloader(cookie, 2024);
runner.LogTimingToFile = true;
await runner.PrepareAsync(downloader, default);
await runner.RunTestsAsync(default);

Console.WriteLine("Press any key to continue...");
try { Console.Read(); } catch { return; }

for (var i = 0; i < Iterations; i++)
    await runner.RunAsync(default);