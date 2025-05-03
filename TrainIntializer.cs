using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    internal class TrainIntializer
    {
        public static Train ParseTrainRoute(string input)
        {
            var splitedInput =input.Split(' ');

            int trainNumber = int.Parse(splitedInput[0]);

            var route=new Route(splitedInput[1].Split("-")[0],splitedInput[2].Split("-")[0]);



        }
    }
}
