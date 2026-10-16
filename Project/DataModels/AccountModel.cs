public class AccountModel
{

    public Int64 Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public bool IsBlocked {get; set;}
    public string Password { get; set; }


    public AccountModel(){}
    public AccountModel(Int64 id, string firstName, string lastName, string email, string phoneNumber, UserRole role , bool status, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        IsBlocked = status;
        Password = password;
    }

    public override string ToString()
    {
        return $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}, Email: {Email}, PhoneNumber{PhoneNumber}, Role: {Role}, Password: {Password} ";
    }

    public string ToAdminString() //info for admin to manage accounts
    {
        string blockStatus = IsBlocked ? "Yes" : "No";
        return $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}, Email: {Email}, PhoneNumber: {PhoneNumber}, Role: {Role}, IsBlocked: {blockStatus}";
    }


}

    public enum UserRole
    {
        Customer = 1,
        Admin = 2
    }


