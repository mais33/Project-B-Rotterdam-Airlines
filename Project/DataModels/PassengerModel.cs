public class PassengerModel
{
    public Int64 Id { get; set; }
    public Int64 AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Dob { get; set; }

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
        Dob = dob;
    }
}