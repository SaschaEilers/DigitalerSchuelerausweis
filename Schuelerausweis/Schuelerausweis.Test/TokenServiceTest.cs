using FluentAssertions;
using Schuelerausweis.Services;
using Microsoft.Extensions.Configuration;
using Schuelerausweis.Models;

namespace Schuelerausweis.Test;

[TestClass]
public class TokenServiceTest
{
    private readonly IConfiguration _configuration;

    public TokenServiceTest()
    {
        var manager = new ConfigurationManager();

        IEnumerable<KeyValuePair<string, string?>> bla = new Dictionary<string, string?>
        {
            ["TokenLifeSpan:Days"] = "0",
            ["TokenLifeSpan:Hours"] = "0",
            ["TokenLifeSpan:Minutes"] = "3",
            ["TokenLifeSpan:Seconds"] = "0"
        };
        
        manager.AddInMemoryCollection(bla);
        _configuration = manager;
    }

    [TestMethod]
    public void IsAlive_ShouldReturnFalse_WhenDateTimeOutOfRange()
    {
        var tokenService = new TokenService(_configuration);

        var result = tokenService.IsAlive(DateTime.Now.AddMinutes(-3));
        result.Should().BeFalse();
    }
    
    [TestMethod]
    public void IsAlive_ShouldReturnTrue_WhenDateTimeInRange()
    {
        var tokenService = new TokenService(_configuration);

        var result = tokenService.IsAlive(DateTime.Now.AddMinutes(-2).AddSeconds(59));
        result.Should().BeTrue();
    }

    [TestMethod]
    public void CreateToken_ShouldReturnTokenDataWithDateTimeAndId_WhenInputIsValidFormat()
    {
        var tokenService = new TokenService(_configuration);
        var dateTime = new DateTime(2000, 01, 01);
        const string name = "user";
        var tokenData = tokenService.CreateToken($"{dateTime:s}|{name}");
        tokenData
            .Should()
            .NotBeNull()
            .And
            .BeOfType<TokenData>();
        tokenData.Id
            .Should()
            .Be(name);
        tokenData.CreationDate
            .Should()
            .Be(dateTime);
    }

    [TestMethod]
    public void CreateToken_ShouldThrowFormatException_WhenInputIsInvalid()
    {
        var tokenService = new TokenService(_configuration);
        tokenService.Invoking(x => x.CreateToken("Invalid"))
            .Should()
            .Throw<FormatException>();
    }
    
    [TestMethod]
    public void CreateToken_ShouldThrowOutOfBoundsException_WhenInputDoesNotContainTwoValues()
    {
        var tokenService = new TokenService(_configuration);
        var dateTime = new DateTime(2000, 01, 01);
        tokenService.Invoking(x => x.CreateToken(dateTime.ToString("s")))
            .Should()
            .Throw<IndexOutOfRangeException>();
    }
}