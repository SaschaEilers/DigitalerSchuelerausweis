using System.DirectoryServices.Protocols;

namespace Schuelerausweis.Services;

public class LdapService : ILdapService
{
    public IDictionary<string, string> Attributes { get; set; }

    public LdapService(IConfiguration configuration)
    {
        var section = configuration.GetSection("ldap");
        var credentials = section.GetSection("credentials");
        var serverSection = section.GetSection("server");
        var server = serverSection.GetValue<string>("hostname");
        var port = serverSection.GetValue<int>("port");
        var directoryIdentifier = new LdapDirectoryIdentifier(server, port);
        var user = configuration.GetValue<string>("domain");
    }
}

public interface ILdapService
{
    public IDictionary<string, string> Attributes { get; } 
}