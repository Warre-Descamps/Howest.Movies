namespace Magyx.Common.Extensions;

public static class RandomExtensions
{
    private const string ErrorMessage = "Max must be greater than min.";

    public static bool NextBoolean(this Random random)
    {
        return random.Next(0, 2) != 0;
    }

    public static short Next(this Random random, short min, short max)
    {
        if (max <= min)
        {
            throw new ArgumentException(ErrorMessage);
        }

        var rn = (max * 1.0 - min * 1.0) * random.NextDouble() + min * 1.0;
        return Convert.ToInt16(rn);
    }

    public static long Next(this Random random, long min, long max)
    {
        if (max <= min)
        {
            throw new ArgumentException(ErrorMessage);
        }

        var rn = (max * 1.0 - min * 1.0) * random.NextDouble() + min * 1.0;
        return Convert.ToInt64(rn);
    }

    public static float Next(this Random random, float min, float max)
    {
        if (max <= min)
        {
            throw new ArgumentException(ErrorMessage);
        }

        var rn = (max * 1.0 - min * 1.0) * random.NextDouble() + min * 1.0;
        return Convert.ToSingle(rn);
    }

    public static double Next(this Random random, double min, double max)
    {
        if (max <= min)
        {
            throw new ArgumentException(ErrorMessage);
        }

        var rn = (max - min) * random.NextDouble() + min;
        return rn;
    }

    public static DateTime Next(this Random random, DateTime min, DateTime max)
    {
        if (max <= min)
        {
            throw new ArgumentException(ErrorMessage);
        }

        var minTicks = min.Ticks;
        var maxTicks = max.Ticks;
        var rn = (Convert.ToDouble(maxTicks)
                  - Convert.ToDouble(minTicks)) * random.NextDouble()
                 + Convert.ToDouble(minTicks);
        return new DateTime(Convert.ToInt64(rn));
    }

    public static TimeSpan Next(this Random random, TimeSpan min, TimeSpan max)
    {
        if (max <= min)
        {
            throw new ArgumentException(ErrorMessage);
        }

        var minTicks = min.Ticks;
        var maxTicks = max.Ticks;
        var rn = (Convert.ToDouble(maxTicks)
                  - Convert.ToDouble(minTicks)) * random.NextDouble()
                 + Convert.ToDouble(minTicks);
        return new TimeSpan(Convert.ToInt64(rn));
    }
}