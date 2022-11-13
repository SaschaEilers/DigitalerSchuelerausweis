using System.Collections;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Schuelerausweis.Models;
using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace Schuelerausweis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    [HttpGet]
    public Results<Ok<User>, ProblemHttpResult> Index()
    {
        try
        {
            var directoryIdentifier = new LdapDirectoryIdentifier("localhost", 389);
            const string Base = "dc=yourOrganisation,dc=loc";
            const string User = $"cn=user-read-only,{Base}";
            const string SearchUser = $"cn=obed,{Base}";
            var credential = new NetworkCredential(User, "user-read-only");
            using var a = new LdapConnection(directoryIdentifier);
            a.AuthType = AuthType.Basic;
            a.SessionOptions.ProtocolVersion = 3;
            a.Bind(credential);

            Dictionary<string, string> loadedValues = new();

            var b = a.SendRequest(new SearchRequest(SearchUser, "(&(objectClass=inetOrgPerson))", SearchScope.Subtree));
            if (b is SearchResponse response)
            {
                foreach (SearchResultEntry responseEntry in response.Entries)
                {
                    foreach (DictionaryEntry responseEntryAttribute in responseEntry.Attributes)
                    {
                        var bytes = responseEntryAttribute.Value as DirectoryAttribute;
                        var bb = bytes.Cast<byte[]>().SelectMany(x => x).ToArray();
                        loadedValues[bytes.Name] = Encoding.Default.GetString(bb);
                    }
                }
            }

            User user = new()
            {
                FirstName = loadedValues["cn"],
                LastName = loadedValues["sn"],
                DateOfBirth = DateOnly.Parse(loadedValues["title"]),
                Class = "WIT3C"
            };
        
            return TypedResults.Ok(user);
        }
        catch (Exception e)
        {
            return TypedResults.Problem(e.Message);
        }
    }
}