namespace Schuelerausweis.Models;

public class LdapConfiguration
{
    public LdapCredentialsConfiguration Credentials { get; set; }
    public LdapServerConfiguration Server { get; set; }
}