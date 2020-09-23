using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FhatFinder.Scraper
{
    public static class EnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int degreeOfParalellism, Func<T, Task> action)
        {
            return Task.WhenAll(Partitioner
                    .Create(source)
                    .GetPartitions(degreeOfParalellism)
                    .Select(partition => Task.Run(async () =>
                    {
                        using (partition)
                            while (partition.MoveNext())
                                await action(partition.Current);
                    })));
        }
    }
}
