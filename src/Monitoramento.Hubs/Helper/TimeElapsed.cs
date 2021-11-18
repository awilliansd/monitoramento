using System.Diagnostics;

namespace Monitoramento.Hubs.Helper
{
    public class TimeElapsed
    {
        private Stopwatch _watch;

        public TimeElapsed()
        {
            _watch = Stopwatch.StartNew();
        }

        public int GetElapsedTimeBySecond()
        {
            _watch.Stop();
            var elapsedMs = _watch.ElapsedMilliseconds;
            return (int)(elapsedMs / 1000);
        }

        public string GetTimeByFormat()
        {
            _watch.Stop();
            var ts = _watch.Elapsed;
            _watch = Stopwatch.StartNew();

            if (ts.Hours > 0)
                return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            if (ts.Minutes > 0)
                return ts.Minutes + "," + ts.Seconds + "m";
            if (ts.Seconds > 0)
                return ts.Seconds + "s";

            return $"{ts.Milliseconds / 10:000}ms";
        }

        public string GetTimeByFormatFinal()
        {
            _watch.Stop();
            var ts = _watch.Elapsed;

            if (ts.Hours > 0)
                return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            if (ts.Minutes > 0)
                return ts.Minutes + "," + ts.Seconds + "m";
            if (ts.Seconds > 0)
                return ts.Seconds + "s";

            return $"{ts.Milliseconds / 10:000}ms";
        }

        public int GetSecond()
        {
            var elapsedMs = _watch.ElapsedMilliseconds;
            return (int)(elapsedMs / 1000);
        }

        public void StopTime()
        {
            _watch.Stop();
        }
    }
}