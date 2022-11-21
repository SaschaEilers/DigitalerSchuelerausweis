namespace Schuelerausweis.Models;

public class LdapConfiguration
{
    public required LdapCredentialsConfiguration Credentials { get; set; }
    public required LdapServerConfiguration Server { get; set; }
}