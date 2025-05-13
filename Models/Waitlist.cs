using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI.Models
{
    public class Waitlist
    {
        public List<Ticket> SLWaitlist = new List<Ticket>();
        public List<Ticket> A3Waitlist = new List<Ticket>();
        public List<Ticket> A2Waitlist = new List<Ticket>();
        public List<Ticket> A1Waitlist = new List<Ticket>();

    }
}
