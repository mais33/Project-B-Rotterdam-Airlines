public class PassengerModel
{
    public Int64 Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Dob { get; set; } = "";

    public PassengerModel()
    {
    }

    public PassengerModel(
        Int64 id,
        string firstName,
        string lastName,
        string dob)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Dob = dob;
    }
}