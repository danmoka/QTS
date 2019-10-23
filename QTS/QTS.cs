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
        internal static List<int> unserved; // необслужанные заявки
        internal static int WorkingTime { get; set; } = 0;// время работы системы
        internal static List<Statistics> Statistics { get; set; } = new List<Statistics>();
        internal static Statistics CommonStatistics { get; set; }
        private static Random _random = new Random();

        public static void Start(int requestsCount, int channelsCount, int channelCapacity, decimal? busyTime)
        {
            for (int j = 0; j < WorkingTime; j++)
            {
                unserved = new List<int>();
                CreateRequests(requestsCount); // генерация заявок
                if (busyTime == null)
                    CreateChannels(channelsCount, channelCapacity); // генерация каналов
                else
                    CreateChannels(channelsCount, channelCapacity, (decimal)busyTime);

                // моделирование работы
                for (int i = 0; i < Requests.Length; i++)
                {
                    int numberOfGoodChannel = FindChannel(Requests[i]); // поиск подходящего канала

                    if (numberOfGoodChannel != -1)
                        Channels[numberOfGoodChannel].Serve(Requests[i]);
                    else
                        unserved.Add(i);
                }

                CreateStatistics();

                QTSPrinter.PrintRequestsArrivalTime();
                QTSPrinter.PrintResult();
                QTSPrinter.PrintStatistics(Statistics[j]);
            }

            CreateCommonStatistics();
            QTSPrinter.PrintCommonStatistic();
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

        private static void CreateChannels(int channelsCount, int maxChannelCapacity)
        {
            Channels = new Channel[channelsCount];

            for(int i = 0; i < channelsCount; i++)
            {
                Channels[i] = new Channel(_random.Next(1, maxChannelCapacity));
                
                for (int j = 0; j < Channels[i].Capacity; j++)
                    Channels[i].ServiceTimes[j] = Math.Round((decimal)_random.NextDouble(), 3);
            }
        }

        private static void CreateChannels(int channelsCount, int channelCapacity, decimal busyTime)
        {
            Channels = new Channel[channelsCount];

            for (int i = 0; i < channelsCount; i++)
            {
                Channels[i] = new Channel(channelCapacity);

                for (int j = 0; j < Channels[i].Capacity; j++)
                    Channels[i].ServiceTimes[j] = busyTime;
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

        private static void CreateStatistics()
        {
            decimal pom = Math.Round((decimal)(Requests.Length - unserved.Count) / Requests.Length, 3);
            int cc = Requests.Length - unserved.Count;
            decimal pof = Math.Round((decimal)unserved.Count / Requests.Length, 3);
            decimal cmt = Math.Round(GetTotalServingTime() / (Requests.Length - unserved.Count), 2);

            Statistics statistics = new Statistics.Builder().
                ProbabilityOfMaintenance(pom).
                ChannelCapacity(cc).
                ProbabilityOfRefusal(pof).
                CommonMaintenanceTime(cmt).Build();

            Statistics.Add(statistics);
        }       

        private static decimal GetTotalServingTime()
        {
            decimal totalServingTime = 0;

            foreach (var channel in Channels)
                totalServingTime += channel.ServiceTime;

            return totalServingTime;
        }

        private static void CreateCommonStatistics()
        {
            decimal pom = 0;
            int cc = 0;
            decimal pof = 0;
            decimal cmt = 0;

            foreach(var stat in Statistics)
            {
                pom += stat.probabilityOfMaintenance;
                cc += stat.channelCapacity;
                pof += stat.probabilityOfRefusal;
                cmt += stat.commonMaintenanceTime;
            }

            pom = pom / WorkingTime;
            cc = cc / WorkingTime;
            pof = pof / WorkingTime;
            cmt = cmt / WorkingTime;

            CommonStatistics = new Statistics.Builder().
                ProbabilityOfMaintenance(pom).
                ChannelCapacity(cc).
                ProbabilityOfRefusal(pof).
                CommonMaintenanceTime(cmt).Build();
        }
    }
}
