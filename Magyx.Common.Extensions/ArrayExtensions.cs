namespace Magyx.Common.Extensions;

public static class ArrayExtensions
{
    public static T? TryGetValue<T>(this T[] array, int index)
    {
        return array.Length <= index ? default : array[index];
    }
    
    public static T TryGetValue<T>(this T[] array, int index, T defaultValue)
    {
        return array.TryGetValue(index) ?? defaultValue;
    }

    public static IEnumerable<string> GetWordContinuations(this IEnumerable<string[]> array, string[] words)
    {
        return array
            .Where(cw =>
            {
                if (cw.Length < words.Length) return false;
                var wordsMatch = true;
                for (var i = 0; i < words.Length; i++)
                {
                    wordsMatch &= string.IsNullOrWhiteSpace(words[i]) || cw[i] == words[i];
                    if (!wordsMatch)
                        break;
                }

                return wordsMatch || (!string.IsNullOrWhiteSpace(words[^1]) && cw[words.Length - 1].StartsWith(words[^1]));
            })
            .Select(cw => cw[words.Length - 1])
            .Distinct();
    }

    public static IEnumerable<TSource> AppendRange<TSource>(this IEnumerable<TSource> array, IEnumerable<TSource> second)
    {
        return second.Aggregate(array, (current, item) => current.Append(item));
    }

    public static IEnumerable<string> SurroundWith(this IEnumerable<string> inputs, char target)
    {
        return inputs.Select(input => input.SurroundWith(target));
    }
}