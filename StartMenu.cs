using Train_Reservation_System_CLI.Exceptions;
using Train_Reservation_System_CLI.IOHandlers;
using Train_Reservation_System_CLI.Services;

namespace Train_Reservation_System_CLI;

public class StartMenu
{
    private readonly BookingManager _bookingManager;
    private readonly CancellationManager _cancellationManager;
    private readonly TicketManager _ticketManager;
    private readonly TrainManager _trainManager;


    public StartMenu()
    {
        _trainManager = new TrainManager();
        _ticketManager = new TicketManager();
        _bookingManager = new BookingManager(_trainManager, _ticketManager);
        _cancellationManager = new CancellationManager(_ticketManager);
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
            catch (InvalidInputException ex)
            {
                OutputHandler.PrintError(ex.Message);
                OutputHandler.PrintBanner("Please Enter Choice Again");
            }                                           
            catch (Exception ex)                        
            {                                           
                OutputHandler.PrintError(ex.Message);   
                OutputHandler.PrintBanner("Please Enter Choice Again ");
            }
    }

    private void ExecuteMenuOption(int choice)
    {
        switch (choice)
        {
            case 1:
                _trainManager.AddTrains();
                break;
            case 2:
                _bookingManager.HandleBookingFlow();
                break;
            case 3:
                _trainManager.GetAllTrains();
                break;
            case 4:
                _ticketManager.GetBookingDetailsByPNR();
                break;
            case 5:
                _ticketManager.GenerateBookingReport();
                break;
            case 6:
                _cancellationManager.HandleCancellation();
                break;
            default:
                OutputHandler.PrintError("Invalid Choice");
                break;
        }
    }
}