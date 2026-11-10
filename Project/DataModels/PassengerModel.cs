public class PassengerModel
{
    public Int64 Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string DateOfBirth { get; set; } = "";
    public string PassportNumber { get; set; } = "";

    public string Dob
    {
        get => DateOfBirth;
        set => DateOfBirth = value;
    }

    public PassengerModel()
    {
    }

    public PassengerModel(
        Int64 id,
        string firstName,
        string lastName,
        string dateOfBirth,
        string passportNumber = "")
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PassportNumber = passportNumber;
    }
}
