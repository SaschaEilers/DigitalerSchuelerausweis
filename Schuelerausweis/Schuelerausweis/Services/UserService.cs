using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using Schuelerausweis.Models;

namespace Schuelerausweis.Services;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly IEncryptionService _encryptionService;
    private readonly ILdapService _ldapService;

    public UserService(ITokenService tokenService, IEncryptionService encryptionService, ILdapService ldapService)
    {
        _tokenService = tokenService;
        _encryptionService = encryptionService;
        _ldapService = ldapService;
    }
    
    public async Task<Either<InvalidTokenError, User>> GetUserByTokenAsync(byte[] encryptedToken, CancellationToken cancellationToken)
    {
        var decryptedToken = await _encryptionService.DecryptAsync(encryptedToken, cancellationToken);
        var tokenData = _tokenService.CreateToken(decryptedToken);
        if (!_tokenService.IsAlive(tokenData.CreationDate))
        {
            return new InvalidTokenError
            {
                Description = $"Lifespan expired"
            };
        }
        
        var attributesForUser = _ldapService.GetAttributesForUser(tokenData.Id);
        return new User
        {
            FirstName = attributesForUser["cn"],
            LastName = attributesForUser["sn"],
            Class = "WIT3C",
            DateOfBirth = DateOnly.Parse(attributesForUser["title"])
        };
    }
}

public struct InvalidTokenError
{
    public string Description { get; set; }
}

public interface IUserService
{
    public Task<Either<InvalidTokenError, User>> GetUserByTokenAsync(byte[] token, CancellationToken cancellationToken);
}