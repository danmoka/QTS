using System;
using System.Collections.Generic;

namespace QTS
{
    /// <summary>
    /// Система массового обслуживания
    /// </summary>
    public static class QTS
    {
        internal static Request[] Requests { get; set; } // заявки
        internal static Channel[] Channels { get; set; } // каналы
        internal static List<int> unserved = new List<int>(); // необслужанные заявки
        private static Random random = new Random();

        public static void Start(int requestsCount, int channelsCount)
        {
            CreateRequests(requestsCount); // генерация заявок
            CreateChannels(channelsCount); // генерация каналов

            // моделирование работы
            for(int i = 0; i < Requests.Length; i++)
            {
                int numberOfGoodChannel = FindChannel(Requests[i]); // поиск подходящего канала

                if (numberOfGoodChannel != -1)
                    Channels[numberOfGoodChannel].Serve(Requests[i]);
                else
                    unserved.Add(i);
            }

            PrintRequestsArrivalTime();
            PrintResult();
            PrintStatistics();
        }

        private static void CreateRequests(int requestsCount)
        {
            Requests = new Request[requestsCount];
            decimal[] intervals = IntervalGenerator.GenerateIntervals(requestsCount);
            decimal totalTime = 0;

            for(int i = 0; i < requestsCount; i++)
            {
                totalTime += intervals[i];
                Requests[i] = new Request(i, totalTime);
            }
        }

        private static void CreateChannels(int channelsCount)
        {
            Channels = new Channel[channelsCount];

            for(int i = 0; i < channelsCount; i++)
            {
                Channels[i] = new Channel(random.Next(1, 10));
                decimal[] intervals = IntervalGenerator.GenerateIntervals(Channels[i].Capacity);

                for (int j = 0; j < Channels[i].Capacity; j++)
                    Channels[i].ServiceTimes[j] = intervals[j];
            }
        }

        private static int FindChannel(Request request)
        {
            int minServed = int.MaxValue;
            int minChannelNumber = -1;

            for(int i = 0; i < Channels.Length; i++)
            {
                if(Channels[i].IsFree(request))
                    if(Channels[i].NumberOfServed < minServed)
                    {
                        minServed = Channels[i].NumberOfServed;
                        minChannelNumber = i;
                    }
            }

            return minChannelNumber;
        }

        public static void PrintResult()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"\nИз ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{Requests.Length} ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("заявок");
            Console.Write($"Не обслужены: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(unserved.Count);
            Console.ForegroundColor = ConsoleColor.DarkRed;

            foreach (int num in unserved)
                Console.WriteLine($"заявка № {num}");

            Console.ResetColor();
            string answer = "";

            for(int i = 0; i < Channels.Length; i++)
            {
                answer += $"канал {i}: обслужено заявок {Channels[i].NumberOfServed} " +
                    $"из {Channels[i].Capacity} возможных, " +
                    $"время работы: {Channels[i].BusyTime}. (№ заявки, время обслуживания) [ ";

                foreach (var servedRequest in Channels[i].Served)
                    answer += $"{servedRequest} ";

                answer += "] \n";
            }

            Console.WriteLine(answer);
        } 

        private static void PrintRequestsArrivalTime()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Приходы заявок: ");
            Console.ResetColor();

            foreach (Request request in Requests)
                Console.Write($"№{request.ID}: {request.ArrivalTime} ");
        }

        private static void PrintStatistics()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Статистика:");
            Console.ResetColor();
            Console.WriteLine($"Вероятность обслуживания: {100 * Math.Round((decimal)(Requests.Length - unserved.Count) / Requests.Length, 3)} %");
            Console.WriteLine($"Пропускная способность: {Requests.Length - unserved.Count} [шт/час]");
            Console.WriteLine($"Вероятность отказа: {100 * Math.Round((decimal)unserved.Count / Requests.Length, 3)} %");
            Console.WriteLine($"Среднее время обслуживания заявки: {60 * Math.Round(GetTotalServingTime() / (Requests.Length - unserved.Count), 2)} [мин]");
        }

        private static decimal GetTotalServingTime()
        {
            decimal totalServingTime = 0;

            foreach (var channel in Channels)
                totalServingTime += channel.ServiceTime;

            return totalServingTime;
        }
    }
}
