using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Schuelerausweis.Models;
using Schuelerausweis.Services;

namespace Schuelerausweis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger,IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    
    
    [HttpPost]
    public async Task<Results<Ok<User>, BadRequest<Error>, ProblemHttpResult>> GetUserData([FromBody]string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return TypedResults.BadRequest(new Error
            {
                Status = HttpStatusCode.BadRequest,
                Description = "Request is missing a Token"
            });
        }
        
        try
        {
            var urlDecodedToken = WebUtility.UrlDecode(token);
            var decodedToken = WebEncoders.Base64UrlDecode(urlDecodedToken);
            var userOrError = await _userService.GetUserByTokenAsync(decodedToken, cancellationToken);

            if (userOrError is { IsLeft: true, Case: InvalidTokenError error})
            {
                _logger.LogWarning("Token {} had the following issue: {}", token, error.Description);
                return TypedResults.BadRequest(new Error
                {
                    Status = HttpStatusCode.BadRequest,
                    Description = "Invalid Token"
                });
            }

            if (userOrError is { IsRight: true, Case: User user })
            {
                _logger.LogInformation("Token {} successfully resolved", token);
                return TypedResults.Ok(user);
            }
            
            _logger.LogError("Token {} caused an invalid state", token);
            return TypedResults.Problem("This State should not be reached");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Resolving the User for token {} failed due to an exception", token);
            return TypedResults.Problem("Server could not resolve User");
        }
    }
}