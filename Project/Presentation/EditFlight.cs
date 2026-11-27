using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

public class EditFlight
{
    private FlightLogic flightLogic = new();
    private AirportLogic airportLogic  = new();
    private PlaneLogic planeLogic = new();
    public void Start()
    {
        EditSelect();
    }

    public void FlightsOverview(List<FlightModel> flights)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                EDIT FLIGHT OVERVIEW              ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        if (flights.Count == 0)
        {
            Console.WriteLine("  > No flights available at the moment.");
            Console.WriteLine();
        }
        else
        {
            for (int i = 0; i < flights.Count; i++)
            {
                FlightModel flight = flights[i];

                string departure = airportLogic.GetDepartureCity(flight);
                string destination = airportLogic.GetDestinationCity(flight);

                Console.WriteLine($"  [{i + 1}] {departure} -> {destination}");
                Console.WriteLine($"      Departure:  {flight.DepartureDatetime}");
                Console.WriteLine($"      Price:      EUR {flight.Price:0.00}");
                Console.WriteLine();
            }
        }

        Console.WriteLine("--------------------------------------------------");
    }

    public void EditSelect()
    {
        bool selecting = true;

        while (selecting)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                  EDIT FLIGHTS                  ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine("  [1] Edit a Flight");
            Console.WriteLine("  [2] Return to Admin menu");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-2): ");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                List<FlightModel> flights = SearchFlightsByStartAirport();
                FlightsOverview(flights);

                if (flights.Count > 0) AskEditFlight(flights);

                Console.WriteLine("  Press any key to return to the menu...");
                Console.ReadKey();
            }

            if (choice == "2")
            {
                Console.WriteLine("  > Returning to Admin menu");
                Console.WriteLine();
                Console.WriteLine("  > Press any key to return...");
                Console.ReadKey();
                selecting = false;
            }
        }
    }

    public List<FlightModel> SearchFlightsByStartAirport()
    {
        Console.Clear();
        Console.WriteLine("Please enter starting airport you want to edit the flight from");
        Console.WriteLine($"\nAvailable airports: {AvailableAirports()}");

        string startAirport = Console.ReadLine().Trim().ToUpper() ?? "";

        List<FlightModel> allFlights = flightLogic.GetFlights();

        List<FlightModel> flights = allFlights
        .Where(f => airportLogic.GetDepartureCity(f).ToUpper() == startAirport)
        .ToList();

        return flights;
    }

    

    public string AvailableAirports()
    {
        List<AirportModel> airports = airportLogic.GetAirports();
        string availableAirports = "";

        foreach(var airport in airports)
        {
            availableAirports += airport.City;
            availableAirports += " ";
        }

        return availableAirports;
    }

    public void AskEditFlight(List<FlightModel> flights)
    {
        bool entering = true;

        while(entering)
        {
            Console.WriteLine(" Enter the corresponding id to edit a flight");
            string idInput = Console.ReadLine() ?? "";

            if (int.TryParse(idInput, out int choice))
            {
                if(choice > 0)
                { 
                    FlightModel flight = flights[choice - 1];
                    PlaneModel plane = planeLogic.GetPlaneById(flight.PlaneId);
                    EditMenu(flight, plane);
                    entering = false;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("  > Invalid choice. Please choose a number from the list.");
                    Console.WriteLine();
                    Console.WriteLine("  Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("  > Please enter a valid number.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
            }

        }
    }

    public void EditMenu(FlightModel flight, PlaneModel plane)
    {
        bool selecting = true;
        if (flight is null || plane is null)
        {
            Console.WriteLine("  > Error: Flight data could not be loaded.");
            Console.ReadKey();
            return;
        }
        List<ActionMenu> menuOptions = new List<ActionMenu>
        {
            new ActionMenu("Plane", () => EditPlaneMenu(plane, flight)),
            new ActionMenu("Departure city", () => Console.WriteLine("Change departure logic here...")),
            new ActionMenu("Destination city", () => Console.WriteLine("Change destination logic here...")),
            new ActionMenu("Price", () => Console.WriteLine("Change price logic here...")),
            new ActionMenu("Arrival date/time", () => Console.WriteLine("Change arrival logic here...")),
            new ActionMenu("Departure date/time", () => Console.WriteLine("Change departure time logic here...")),
            new ActionMenu("Return", () => selecting = false)
        };



        while (selecting)
        {
            PlaneModel currentPlane = planeLogic.GetPlaneById(flight.PlaneId);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("                  EDIT FLIGHT DATA                ");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine($"  [1] Plane: {currentPlane.Name}, {currentPlane.Model}");
            Console.WriteLine($"  [2] Departure city: {airportLogic.GetAirportById(flight.StartAirportId)?.City}");
            Console.WriteLine($"  [3] Destination city: {airportLogic.GetAirportById(flight.DestinationId)?.City}");
            Console.WriteLine($"  [4] Price: €{flight.Price:0.00}");
            Console.WriteLine($"  [5] Arrival date/time: {flight.ArrivalDatetime}");
            Console.WriteLine($"  [6] Departure date/time: {flight.DepartureDatetime}");
            Console.WriteLine($"  [7] Return");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-7): ");

            string choice = Console.ReadLine() ?? "";

            if (int.TryParse(choice, out int index) && index > 0 && index <= menuOptions.Count)
            {
                menuOptions[index - 1].actionMenu();
            }
        }

    }
    
    public void EditPlaneMenu(PlaneModel plane, FlightModel flight)
    {
        Console.WriteLine($"  [1] Plane: {planeLogic.GetPlaneById(plane.Id).Name}, {planeLogic.GetPlaneById(plane.Id)?.Model}\n");

        List<ActionMenu> menuOptions = new List<ActionMenu>
        {
            new ActionMenu("Edit Plane", () => EditPlane(flight)),
            new ActionMenu("Return", () => Console.WriteLine("Returning"))
        };

        bool selecting = true;
        while(selecting)
        {  
            Console.Clear();
            PlaneModel currentPlane = planeLogic.GetPlaneById(flight.PlaneId);
            Console.WriteLine($"  Plane: {currentPlane.Name}, {currentPlane.Model}\n");

            Console.WriteLine("  [1] Edit Plane");
            Console.WriteLine("  [2] Return to Edit menu");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("  Choose an option (1-2): ");
        

            string choice = Console.ReadLine() ?? "";

            if (int.TryParse(choice, out int index) && index > 0 && index <= menuOptions.Count)
            {
                if (index == 2)
                {
                    selecting = false;
                }

                else
                {
                    menuOptions[index - 1].actionMenu();
                }
            }
            else
            {
                Console.WriteLine("\n  > Please enter a valid number (1-2).");
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    public string AllPlanesOverview()
    {
        string planesOverview = "";

        List<PlaneModel> planes = planeLogic.GetAllPlanes();
        foreach(var plane in planes)
        {
           planesOverview += $"  [{plane.Id}] {plane.Name} ({plane.Model})\n";
        }

        return planesOverview;
    }

    public void EditPlane(FlightModel flight)
    {
        string allPlanes = AllPlanesOverview();
        Console.WriteLine(allPlanes);

        Console.WriteLine("--------------------------------------------------");
        Console.Write("  Enter the ID of the new plane: ");

        string input = Console.ReadLine() ?? "";

        if (int.TryParse(input, out int Id))
        {
            PlaneModel foundPlane = planeLogic.GetPlaneById(Id);

            if (foundPlane != null)
            {
                flight.PlaneId = foundPlane.Id;

                flightLogic.UpdateFlight(flight);

                Console.WriteLine($"\n  > Successfully changed plane to {foundPlane.Name}!");
            }
            else
            {
                Console.WriteLine("\n  > Plane ID not found. No changes made.");
            }
        }
        else
        {
            Console.WriteLine("\n  > Invalid input. Please enter a valid number.");
        }

        Console.WriteLine("\n  Press any key to continue...");
        Console.ReadKey();
    }
}