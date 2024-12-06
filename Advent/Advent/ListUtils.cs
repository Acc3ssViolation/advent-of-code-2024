﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent
{
    internal static class ListUtils
    {
        public static string AggregateString<T>(this IEnumerable<T> list)
        {
            return list.Aggregate("", (a, b) => a.Length > 0 ? $"{a}, {b}" : (b?.ToString() ?? ""));
        }

        public static string AggregateString<T>(this IEnumerable<T> list, Func<T, string> func)
        {
            return list.Aggregate("", (a, b) => a.Length > 0 ? $"{a}, {func(b)}" : func(b));
        }

        public static List<T> CopyAndRemoveAt<T>(this List<T> list, int index)
        {
            var copy = new List<T>(list);
            copy.RemoveAt(index);
            return copy;
        }
    }
}
