using System.Text;

public class TicketTemplate
{
    public string Create(List<TicketModel> tickets)
    {
        FlightLogic flights = new FlightLogic();
        AirportAccess airports = new AirportAccess();
        string template = File.ReadAllText("DataSources/ticket-template.txt");
        StringBuilder result = new StringBuilder();

        foreach (TicketModel ticket in tickets)
        {
            FlightModel? flight = flights.GetFlightById(ticket.FlightId);
            if (flight == null)
            {
                throw new InvalidOperationException($"Flight {ticket.FlightId} could not be found.");
            }

            string origin = airports.GetById(flight.StartAirportId)?.City ?? "Unknown";
            string text = template
                .Replace("{reference}", ticket.BookingReference)
                .Replace("{ticket}", ticket.Id.ToString())
                .Replace("{flight}", flight.Id.ToString())
                .Replace("{origin}", origin)
                .Replace("{destination}", flights.GetDestinationCity(flight))
                .Replace("{departure}", flight.DepartureDatetime)
                .Replace("{arrival}", flight.ArrivalDatetime)
                .Replace("{passenger}", ticket.FirstName + " " + ticket.LastName)
                .Replace("{seat}", ticket.Seat);
            result.AppendLine(text);
        }

        return result.ToString();
    }
}
