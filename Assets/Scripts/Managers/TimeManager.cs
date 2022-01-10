using System;

public class TimeManager
{
    public static long Now
    {
        get
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
