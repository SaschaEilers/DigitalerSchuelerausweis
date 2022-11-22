using Microsoft.Extensions.Options;
using Schuelerausweis.Constants;
using Schuelerausweis.Models;

namespace Schuelerausweis.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsAlive(DateTime dateTime)
    {
        var expirationDate = CreateExpirationDate(dateTime);
        return DateTime.Now < expirationDate;
    }

    public DateTime CreateExpirationDate(DateTime dateTime)
    {
        var options = _configuration
            .GetRequiredSection(ConfigurationSections.TokenLifeSpan)
            .Get<TokenLifeSpanConfiguration>()!;
        var now = DateTime.Now;
        var pastTime = now - dateTime;
        var leftOverTime = new TimeSpan(options.Days, options.Hours, options.Minutes, options.Seconds) - pastTime;
        return now + leftOverTime;
    }

    public ITokenData CreateToken(string data)
    {
        var tokenData = data.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return new TokenData
        {
            CreationDate = DateTime.ParseExact(tokenData[0], "s", null),
            Id = tokenData[1]
        };
    }
}

public interface ITokenService
{
    public bool IsAlive(DateTime dateTime);
    public DateTime CreateExpirationDate(DateTime dateTime);
    public ITokenData CreateToken(string token);
}