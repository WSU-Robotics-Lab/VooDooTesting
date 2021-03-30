using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodoo_Testing
{
    public class Queue
    {
        public int ID;
        public string URL;
        public DateTime TimeSent;
        public DateTime TimeRecieved;

        public Queue(int id, string url)
        {
            ID = id;
            URL = url;
        }

    }
}
