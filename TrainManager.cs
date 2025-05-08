using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    class TrainManager
    {
        public List<Train> Trains = new List<Train>();

        public void AddTrain()
        {
            int numberOfTrains = InputHandler.GetNumberOfTrainsToAdd();
            for (int i = 0; i < numberOfTrains; i++)
            {
                var train = TrainConfiguration.TrainDetails();
                Trains.Add(train);
            }
        }

        public void GetAllTrains()
        {
            OutputHandler.PrintAllTrains(Trains);
        }

        public List<Train> GetTrainsByRoute(string from ,string to)
        {
            return Trains.Where(t => t.HasRoute(from, to)).ToList();
        }

    }
}
