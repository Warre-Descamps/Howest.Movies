using System.Runtime.CompilerServices;

namespace Magyx.Common.Extensions;

public static class TaskExtensions
{
    public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1>, Task<T2>) taskTuple)
    {
        return CombineTasks().GetAwaiter();

        async Task<(T1, T2)> CombineTasks()
        {
            var (task1, task2) = taskTuple;
            await Task.WhenAll(task1, task2);
            return (task1.Result, task2.Result);
        }
    }

    public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
    {
        return Task.WhenAll(tasks).GetAwaiter();
    }

    public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> tasks)
    {
        return Task.WhenAll(tasks).GetAwaiter();
    }

    public static TaskAwaiter<List<(T1, T2)>> GetAwaiter<T1, T2>(this IEnumerable<(T1, Task<T2>)> tasks)
    {
        return CombineItems().GetAwaiter();
        
        async Task<List<(T1, T2)>> CombineItems()
        {
            var valueTuples = tasks as (T1, Task<T2>)[] ?? tasks.ToArray();
            var t1S = valueTuples.Select(t => t.Item1).ToArray();
            var t2S = valueTuples.Select(t => t.Item2).ToArray();
            await Task.WhenAll(t2S);
            var results = new List<(T1, T2)>();
            for (var i = 0; i < valueTuples.Length; i++)
            {
                results.Add((t1S[i], t2S[i].Result));
            }

            return results;
        }
    }
}