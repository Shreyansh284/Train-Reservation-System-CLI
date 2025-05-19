using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train_Reservation_System_CLI.Services.IOHandlers;
using Train_Reservation_System_CLI.Services.Parsers;
using Train_Reservation_System_CLI.Services;
using Train_Reservation_System_CLI.Execptions;

namespace Train_Reservation_System_CLI
{
    public class StartMenu
    {

        private TrainManager trainManager;
        private BookingManager bookingManager;
        private TicketManager ticketManager;
        private TicketCancellationManager ticketCancellationManager;


        public StartMenu()
        {
            trainManager = new TrainManager();
            ticketManager = new TicketManager();
            bookingManager = new BookingManager(trainManager,ticketManager);
            ticketCancellationManager = new TicketCancellationManager(ticketManager, trainManager);
        }

        public void RunTrainOperationsMenu()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    int choice = InputHandler.GetChoiceFromMenu();
                    HandleMenuChoice(choice, ref exit);
                }
                catch (InvalidInputExecption ex)
                {
                    OutputHandler.PrintError(ex.Message);
                    OutputHandler.PrintBanner("Please Enter Details Again ");
                }
                catch (Exception ex)
                {
                    OutputHandler.PrintError(ex.Message);
                    OutputHandler.PrintBanner("Please Enter Details Again ");
                }
            }
        }

        private void HandleMenuChoice(int choice, ref bool exit)
        {
            switch (choice)
            {
                case 1:
                    trainManager.AddTrain();
                    break;
                case 2:
                    bookingManager.GetBookingDetails();
                    break;
                case 3:
                    trainManager.GetAllTrains();
                    break;
                case 4:
                    ticketManager.GetBookingDetailsByPNR();
                    break;
                case 5:
                    ticketManager.GenerateBookingReport();
                    break;
                case 6:
                    ticketCancellationManager.GetCancellationDetails();
                    break;
                case 7:
                    exit = true;
                    break;
                default:
                    OutputHandler.PrintError("Invalid Choice");
                    break;
            }
        }


    }
}
