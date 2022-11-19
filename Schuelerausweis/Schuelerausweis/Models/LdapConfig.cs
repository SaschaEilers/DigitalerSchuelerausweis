namespace Schuelerausweis.Models;

public class LdapConfig
{
    public LdapCredetialsConfig Credentials { get; set; }
    public LdapServerConfig Server { get; set; }
}

public class LdapCredetialsConfig
{
    public string Domain { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LdapServerConfig
{
    public string HostName { get; set; }
    public int  Port { get; set; }
}