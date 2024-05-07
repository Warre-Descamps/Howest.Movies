using System.Text;

namespace Magyx.Common.Extensions;

public static class StringExtensions
{
    public static string FirstToUpper(this string text)
    {
        return char.ToUpper(text[0]) + text[1..];
    }

    public static string IfNullOrWhitespace(this string? s1, string s2)
    {
        return string.IsNullOrWhiteSpace(s1) ? s2 : s1;
    }

    public static string[] SplitArgs(this string input)
    {
        return input
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(new List<string>(), (list, s) =>
            {
                if (list.Count == 0)
                {
                    list.Add(s);
                    return list;
                }
                
                if (list.LastOrDefault(m => m.StartsWith('"') && !m.EndsWith('"')) is { } starter &&
                    list.IndexOf(starter) > -1 &&
                    !list
                        .GetRange(list.IndexOf(starter), list.Count - list.IndexOf(starter))
                        .Any(m => m.EndsWith('"')))
                    list[^1] += ' ' + s;
                else
                    list.Add(s);
                return list;
            })
            .Select(s => s.StartsWith('"') && s.EndsWith('"') ? s[1..^1] : s)
            .ToArray();
    }

    public static string SurroundWith(this string input, char target)
    {
        var sb = new StringBuilder(input);
        sb.Insert(0, target);
        sb.Append(target);
        return sb.ToString();
    }
}