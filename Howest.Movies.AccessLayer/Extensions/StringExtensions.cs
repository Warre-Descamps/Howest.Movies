using System.Globalization;
using System.Text.RegularExpressions;

namespace Howest.Movies.AccessLayer.Extensions;

public static partial class StringExtensions
{
    [GeneratedRegex(@"[^a-zA-Z.\- ]+", RegexOptions.Compiled)]
    private static partial Regex SpecialCharactersRegex();
    
    public static string RemoveSpecialCharacters(this string name)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SpecialCharactersRegex()
            .Replace(name, ""));
    }
}