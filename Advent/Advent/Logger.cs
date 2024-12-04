﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    internal enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }

    internal static class Logger
    {
        public const string White = "\u001b[37m";
        public const string Red = "\u001b[31m";
        public const string Yellow = "\u001b[33m";
        public const string Green = "\u001b[32m";

        private static LogLevel _level = LogLevel.Debug;

        private static object _lock = new ();

        public static void SetLevel(LogLevel level) => _level = level;

        public static void Line()
        {
            lock (_lock)
                Console.WriteLine();
        }

        public static void Line(string message, string color = White)
        {
            if (_level > LogLevel.Info) return;

            lock (_lock)
            {
                Console.Write(color);
                Console.WriteLine(message);
                Console.Write(White);
            }
        }

        public static void ErrorLine(string message)
        {
            if (_level > LogLevel.Error) return;

            lock (_lock)
            {
                Console.Write(Red);
                Console.WriteLine(message);
                Console.Write(White);
            }
        }

        public static void WarningLine(string message)
        {
            if (_level > LogLevel.Warning) return;

            lock (_lock)
            {
                Console.Write(Yellow);
                Console.WriteLine(message);
                Console.Write(White);
            }
        }

        public static void DebugLine(string message)
        {
            if (_level > LogLevel.Debug) return;

            lock (_lock)
            {
                Console.Write(Green);
                Console.WriteLine(message);
                Console.Write(White);
            }
        }

        public static void Append(string message, string color = White)
        {
            if (_level > LogLevel.Info) return;

            lock (_lock)
            {
                Console.Write(color);
                Console.Write(message);
                Console.Write(White);
            }
        }
    }
}
