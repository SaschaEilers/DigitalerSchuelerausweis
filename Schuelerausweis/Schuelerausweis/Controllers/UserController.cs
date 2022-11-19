using System.Collections;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Schuelerausweis.Models;
using Schuelerausweis.Services;
using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace Schuelerausweis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly ILdapService _ldapService;
    private readonly IValidator<TokenData> _validator;

    public UserController(ILdapService ldapService, IValidator<TokenData> validator)
    {
        _ldapService = ldapService;
        _validator = validator;
    }
    
    [HttpGet]
    public Results<Ok<User>, BadRequest<Error>, ProblemHttpResult> GetUserData([FromQuery]TokenData data)
    {
        var validationresult = _validator.Validate(data);
        if (!validationresult.IsValid)
        {
            return TypedResults.BadRequest(new Error
            {
                Status = HttpStatusCode.BadRequest,
                Description = "Invalid request parameter"
            });
        }
        
        
        
        _ldapService.GetAttributesForUser(data.Id);
        
        try
        {
            var now = DateTime.Now.ToBinary();
            Span<byte> nowBytes = stackalloc byte[16];
            for (int i = 0; i < nowBytes.Length; i++)
            {
                nowBytes[i] = now >>
            }
            new Guid(nowBytes);
            Response.Cookies.Append("Ptoken", );

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