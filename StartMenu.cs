using Train_Reservation_System_CLI.Execptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Services;

namespace Train_Reservation_System_CLI;

public class StartMenu
{
    private readonly BookingManager bookingManager;
    private readonly TicketCancellationManager ticketCancellationManager;
    private readonly TicketManager ticketManager;

    private readonly TrainManager trainManager;


    public StartMenu()
    {
        trainManager = new TrainManager();
        ticketManager = new TicketManager();
        bookingManager = new BookingManager(trainManager, ticketManager);
        ticketCancellationManager = new TicketCancellationManager(ticketManager, trainManager);
    }

    public void RunTrainOperationsMenu()
    {
        while (true)
            try
            {
                var choice = InputHandler.GetChoiceFromMenu();
                if (choice == 7) break;
                ExecuteMenuOption(choice);
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

    private void ExecuteMenuOption(int choice)
    {
        switch (choice)
        {
            case 1:
                trainManager.AddTrains();
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
            default:
                OutputHandler.PrintError("Invalid Choice");
                break;
        }
    }
}