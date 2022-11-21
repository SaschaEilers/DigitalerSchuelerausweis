using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using Schuelerausweis.Constants;
using Schuelerausweis.Models;

namespace Schuelerausweis.Services;

public class UserService : IUserService
{
    private readonly AttributesConfiguration attributes;
    private readonly ITokenService _tokenService;
    private readonly IEncryptionService _encryptionService;
    private readonly ILdapService _ldapService;

    public UserService(IConfiguration configuration, ITokenService tokenService, IEncryptionService encryptionService, ILdapService ldapService)
    {
        attributes = configuration
            .GetRequiredSection(ConfigurationSections.Attributes)
            .Get<AttributesConfiguration>()!;
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
            FirstName = attributesForUser[attributes.FirstName],
            LastName = attributesForUser[attributes.LastName],
            Class = attributesForUser[attributes.Class],
            DateOfBirth = DateOnly.Parse(attributesForUser[attributes.DateOfBirth]),
            Image = attributesForUser[attributes.Image],
            EnrollmentYear = int.Parse(attributesForUser[attributes.EnrollmentYear])
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