namespace Magyx.Common.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan ParseTimeString(this string timeString)
    {
        var parts = timeString.Split(' ');

        var hours = 0;
        var minutes = 0;
        double seconds = 0;
        var miliseconds = 0;

        foreach (var part in parts)
        {
            if (part.EndsWith("h"))
            {
                hours = int.Parse(part[..^1]);
            }
            else if (part.EndsWith("m"))
            {
                minutes = int.Parse(part[..^1]);
            }
            else if (part.EndsWith("s"))
            {
                seconds = double.Parse(part[..^1]);
            }
            else if (part.EndsWith("ms"))
            {
                miliseconds = int.Parse(part[..^2]);
            }
        }

        return new TimeSpan(hours, minutes, 0).Add(TimeSpan.FromSeconds(seconds)).Add(TimeSpan.FromMilliseconds(miliseconds));
    }
}