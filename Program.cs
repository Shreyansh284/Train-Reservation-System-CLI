
Console.WriteLine("Welcome to Train Reservation System CLI");
Console.WriteLine("Please enter the number of trains you want to add:");
int NoOfTrains=Int32.Parse(Console.ReadLine());

Train_Reservation_System_CLI.Train[] trains = new Train_Reservation_System_CLI.Train[NoOfTrains];

for (int i = 0; i < NoOfTrains; i++)
{
    Console.WriteLine("Please enter the name of the train:");
    string trainName = Console.ReadLine();
    Console.WriteLine("Please enter the number of coaches in the train:");
    string trainCoaches = Console.ReadLine();
    trains[i] = new Train_Reservation_System_CLI.Train(trainName.Split(" "), trainCoaches.Split(" "));
    trains[i].setTrainDetails();
    trains[i].getTrainDetails();
}

Console.WriteLine("Please enter Detail To Book Tickets");
string bookingDetails=Console.ReadLine();
Console.WriteLine(trains[0].bookTicket(bookingDetails));


