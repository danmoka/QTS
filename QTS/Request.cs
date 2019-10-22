using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTS
{
    internal class Request
    {
        internal int ID { get; set; }
        internal decimal ArrivalTime { get; set; } // время прихода заявки
        //public double WaitTime { get; set; }

        internal Request(int id, decimal arrivalTime)
        {
            ID = id;
            ArrivalTime = arrivalTime;
        }
    }
}
