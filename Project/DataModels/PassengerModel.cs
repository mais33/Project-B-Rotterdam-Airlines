
public class PassengerModel
{
    public Int64 Id { get; set; }

    public Int64 AccountId { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    // Matches the date_of_birth column in the database.
    public string DateOfBirth { get; set; } = "";

    public string? PassportNumber { get; set; }

    // Compatibility with existing code using Dob.
    public string Dob
    {
        get => DateOfBirth;
        set => DateOfBirth = value;
    }

    // Required for Dapper mapping.
    public PassengerModel()
    {
    }

    // Existing constructor, kept for compatibility.
    public PassengerModel(
        Int64 id,
        Int64 accountId,
        string firstName,
        string lastName,
        string dob)
    {
        Id = id;
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
    }

    // Constructor matching the final ERD.
    public PassengerModel(
        Int64 id,
        string firstName,
        string lastName,
        string dateOfBirth,
        string? passportNumber = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PassportNumber = passportNumber;
    }
}
