namespace Schuelerausweis.Models;

public class LdapCredentialsConfiguration
{
    public required string Domain { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
}