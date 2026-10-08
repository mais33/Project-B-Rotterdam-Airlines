public class AccountModel
{

    public Int64 Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }



    public AccountModel(Int64 id, string firstName, string lastName, string email, string phoneNumber, string role, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        Password = password;
    }


}



