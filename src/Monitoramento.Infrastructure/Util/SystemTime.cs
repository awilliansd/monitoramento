using System;

namespace Monitoramento.Infrastructure.Util
{
    public static class SystemTime
    {
        public static Func<DateTime> Now { get; } = () => DateTime.Now;
        public static Func<DateTime> UtcNow { get; } = () => DateTime.UtcNow;
    }
}