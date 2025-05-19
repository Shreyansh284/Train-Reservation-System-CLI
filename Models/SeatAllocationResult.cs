using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI.Models
{
    public class SeatAllocationResult
    {
        public List<Seat> BookedSeats { get; set; } = new();
        public int WaitingSeats { get; set; }
    }
}
