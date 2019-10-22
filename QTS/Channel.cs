using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTS
{
    internal class Channel
    {
        internal int Capacity { get; set; } // емкость
        internal decimal BusyTime { get; set; } // общее время работы
        internal int NumberOfServed { get; set; } // количество обслужанных
        internal decimal[] ServiceTimes { get; set; } // время обслуживания заявок
        internal List<Tuple<int, decimal>> Served { get; set; } // список обслуженных
        internal decimal ServiceTime { get; set; } // общее время обслуживания

        internal Channel(int capacity)
        {
            Capacity = capacity;
            BusyTime = 0;
            ServiceTime = 0;
            NumberOfServed = 0;
            ServiceTimes = IntervalGenerator.GenerateIntervals(capacity);
            Served = new List<Tuple<int, decimal>>();
        }

        internal void Serve(Request request)
        {
            BusyTime = request.ArrivalTime + ServiceTimes[NumberOfServed];
            ServiceTime += ServiceTimes[NumberOfServed];
            Served.Add(new Tuple<int, decimal>(request.ID, ServiceTimes[NumberOfServed]));
            NumberOfServed++;
        }

        internal bool IsFree(Request request)
        {
            if (NumberOfServed < Capacity && request.ArrivalTime >= BusyTime)
                return true;

            return false;
        }
    }
}
