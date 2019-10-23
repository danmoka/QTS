using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTS
{
    class Statistics
    {
        internal decimal probabilityOfMaintenance;
        internal int channelCapacity;
        internal decimal probabilityOfRefusal;
        internal decimal commonMaintenanceTime;

        public class Builder
        {
            internal decimal _pom = 0;
            internal int _cc = 0;
            internal decimal _pof = 0;
            internal decimal _cmt = 0;

            public Builder()
            {

            }

            public Builder ProbabilityOfMaintenance(decimal val)
            {
                _pom = val;
                return this;
            }

            public Builder ChannelCapacity(int val)
            {
                _cc = val;
                return this;
            }

            public Builder ProbabilityOfRefusal(decimal val)
            {
                _pof = val;
                return this;
            }

            public Builder CommonMaintenanceTime(decimal val)
            {
                _cmt = val;
                return this;
            }

            public Statistics Build()
            {
                return new Statistics(this);
            }

        }

        public Statistics(Builder builder)
        {
            probabilityOfMaintenance = builder._pom;
            channelCapacity = builder._cc;
            probabilityOfRefusal = builder._pof;
            commonMaintenanceTime = builder._cmt;
        }
    }
}
