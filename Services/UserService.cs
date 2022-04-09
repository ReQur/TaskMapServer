using System;


namespace dotnetserver
{
    public interface ITimeService
    {
        DateTime GetCurrentTime();
    }

    public class TimeService: ITimeService
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}