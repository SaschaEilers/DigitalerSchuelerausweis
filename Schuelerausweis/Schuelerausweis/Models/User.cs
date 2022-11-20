namespace Schuelerausweis.Models;

public class User
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Class { get; set; }
    //public required Uri ImageUri { get; set; }
}