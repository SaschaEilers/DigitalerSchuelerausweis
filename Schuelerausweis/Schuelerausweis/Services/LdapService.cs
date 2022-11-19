using System.Collections;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using Schuelerausweis.Models;

namespace Schuelerausweis.Services;

public class LdapService : ILdapService, IDisposable
{
    private readonly string _domain;
    private readonly LdapConnection _ldapConnection;
    public LdapService(IOptions<LdapConfig> configuration)
    {
        var serverConfig = configuration.Value.Server;
        var credentialConfig = configuration.Value.Credentials;
        var identifier = new LdapDirectoryIdentifier(serverConfig.HostName, serverConfig.Port);
        var networkCredential =
            new NetworkCredential(string.Join(',', credentialConfig.Username, _domain = credentialConfig.Domain),
                credentialConfig.Password);
        _ldapConnection = new LdapConnection(identifier, networkCredential, AuthType.Basic);
        _ldapConnection.SessionOptions.ProtocolVersion = 3;
        _ldapConnection.Bind();
    }

    public IDictionary<string, string> GetAttributesForUser(string user)
    {
        Dictionary<string, string> loadedValues = new();

        var searchRequest = new SearchRequest(string.Join(',', user, _domain), "(&(objectClass=inetOrgPerson))",
            SearchScope.Subtree);
        var directoryResponse = _ldapConnection.SendRequest(searchRequest);
        if (directoryResponse is not SearchResponse searchResponse)
        {
            return loadedValues;
        }

        foreach (SearchResultEntry responseEntry in searchResponse.Entries)
        {
            foreach (DirectoryAttribute responseEntryAttribute in responseEntry.Attributes)
            {
                var bytes = responseEntryAttribute.Cast<byte[]>()
                                                  .SelectMany(x => x)
                                                  .ToArray();
                loadedValues[responseEntryAttribute.Name] = Encoding.Default.GetString(bytes);
            }
        }

        return loadedValues;
    }

    public void Dispose()
    {
        _ldapConnection.Dispose();
    }
}

public interface ILdapService
{
    public IDictionary<string, string> GetAttributesForUser(string user);
}