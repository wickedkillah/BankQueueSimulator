using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankQueueSimulator
{
    public class Teller
    {
        public int? currentClient;
        public TimeSpan currentTimeToWork;


        public Teller(TimeSpan currentTimeToWork)
        {
            this.currentTimeToWork = currentTimeToWork;
        }


    }
}
