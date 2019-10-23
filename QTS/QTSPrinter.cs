using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTS
{
    static class QTSPrinter
    {
        public static void PrintResult()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"\nИз ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{QTS.Requests.Length} ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("заявок");
            Console.Write($"Не обслужены: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(QTS.unserved.Count);
            Console.ForegroundColor = ConsoleColor.DarkRed;

            foreach (int num in QTS.unserved)
                Console.WriteLine($"заявка № {num}");

            Console.ResetColor();
            string answer = "";

            for (int i = 0; i < QTS.Channels.Length; i++)
            {
                answer += $"канал {i}: обслужено заявок {QTS.Channels[i].NumberOfServed} " +
                    $"из {QTS.Channels[i].Capacity} возможных, " +
                    $"время работы: {60 * QTS.Channels[i].BusyTime} [мин]. (№ заявки, время обслуживания) [ ";

                foreach (var servedRequest in QTS.Channels[i].Served)
                    answer += $"({servedRequest.Item1}, {60 * servedRequest.Item2} [мин])";

                answer += "] \n";
            }

            Console.WriteLine(answer);
        }

        public static void PrintRequestsArrivalTime()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Приходы заявок: ");
            Console.ResetColor();

            foreach (Request request in QTS.Requests)
                Console.Write($"№{request.ID}: {60 * request.ArrivalTime}[мин] ");
        }

        public static void PrintStatistics(Statistics statistics)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Статистика:");
            Console.ResetColor();
            Console.WriteLine($"Вероятность обслуживания: {100 * statistics.probabilityOfMaintenance} %");
            Console.WriteLine($"Пропускная способность: {statistics.channelCapacity} [шт/час]");
            Console.WriteLine($"Вероятность отказа: {100 * statistics.probabilityOfRefusal} %");
            Console.WriteLine($"Среднее время обслуживания заявки: {60 * statistics.commonMaintenanceTime} [мин]");
        }

        public static void PrintCommonStatistic()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Общая статистика:");
            Console.ResetColor();
            Console.WriteLine($"Вероятность обслуживания: {100 * QTS.CommonStatistics.probabilityOfMaintenance} %");
            Console.WriteLine($"Пропускная способность: {QTS.CommonStatistics.channelCapacity} [шт/час]");
            Console.WriteLine($"Вероятность отказа: {100 * QTS.CommonStatistics.probabilityOfRefusal} %");
            Console.WriteLine($"Среднее время обслуживания заявки: {60 * QTS.CommonStatistics.commonMaintenanceTime} [мин]");
        }

    }
}
