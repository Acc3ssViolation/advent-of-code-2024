﻿using System.Collections.Generic;

namespace Advent.Shared
{
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> values)
        {
            foreach (var item in values)
                queue.Enqueue(item);
        }
    }
}
