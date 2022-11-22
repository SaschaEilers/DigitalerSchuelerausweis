namespace Schuelerausweis.Models;

public class User
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Class { get; set; }
    public required string Image { get; set; }
    public required int EnrollmentYear { get; set; }
    public required DateTime ExpirationDate { get; set; }
}