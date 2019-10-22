using System;

namespace QTS
{
    /// <summary>
    /// Класс - генератор интервалов между моментами прихода двух случайных событий по формуле -1/lambda * log(r),
    /// lambda - количество заявок в час, r - случайное число в интервале от 0 до 1.
    /// </summary>
    internal static class IntervalGenerator
    {
        private static Random random = new Random();

        internal static decimal[] GenerateIntervals(int quantityPerTimeUnit)
        {
            decimal[] Intervals = new decimal[quantityPerTimeUnit];

            for(int i = 0; i < quantityPerTimeUnit; i++)
            {
                var a = -Math.Log(random.NextDouble()) / quantityPerTimeUnit;
                var b = (decimal)a;
                Intervals[i] = Math.Round(b, 3);
            }

            return Intervals;
        }

    }
}
