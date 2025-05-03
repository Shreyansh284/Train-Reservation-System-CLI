using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    public class Train
    {
        public int TrainNumber;
        public Route Route;
        public List<Coach> Coach;

        public Train(int TrainNumber, Route Route)
        {
            this.TrainNumber = TrainNumber;
            this.Route = Route;
        }




    }
}
