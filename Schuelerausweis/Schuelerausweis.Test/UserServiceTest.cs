using FluentAssertions;
using LanguageExt;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Moq;
using Schuelerausweis.Models;
using Schuelerausweis.Services;

namespace Schuelerausweis.Test;

[TestClass]
public class UserServiceTest
{
    private const string FirstName = nameof(FirstName);
    private const string LastName = nameof(LastName);
    private const string DateOfBirth = nameof(DateOfBirth);
    private const string Image = nameof(Image);
    private const string EnrollmentYear = nameof(EnrollmentYear);
    private const string Class = nameof(Class);
    
    private readonly IConfiguration _configuration;
    
    public UserServiceTest()
    {
        var manager = new ConfigurationManager();

        IEnumerable<KeyValuePair<string, string?>> options = new Dictionary<string, string?>
        {
            ["Attributes:FirstName"] = FirstName,
            ["Attributes:LastName"] = LastName,
            ["Attributes:DateOfBirth"] = DateOfBirth,
            ["Attributes:Image"] = Image,
            ["Attributes:EnrollmentYear"] = EnrollmentYear,
            ["Attributes:Class"] = Class
        };
        
        manager.AddInMemoryCollection(options);
        _configuration = manager;
    }

    [TestMethod]
    public async Task GetUserByTokenAsync_ShouldReturnInvalidTokenError_WhenTokenIsExpired()
    {
        var mockTokenService = new Mock<ITokenService>();
        var mockData = new Mock<ITokenData>();
        mockTokenService.Setup(x => x.CreateToken(It.IsAny<string>()))
            .Returns(mockData.Object);
        mockTokenService.Setup(x => x.IsAlive(It.IsAny<DateTime>()))
            .Returns(false);
        var mockEncryptionService = new Mock<IEncryptionService>();
        var mockLdapService = new Mock<ILdapService>();
        
        var userService = new UserService(_configuration, mockTokenService.Object, mockEncryptionService.Object, mockLdapService.Object);

        var either = await userService.GetUserByTokenAsync(Array.Empty<byte>(), CancellationToken.None);

        either.State
            .Should()
            .Be(EitherStatus.IsLeft);
        either.Case
            .Should()
            .BeOfType<InvalidTokenError>()
            .Which
            .Description
            .Should()
            .Be("Lifespan expired");
    }

    [TestMethod]
    public async Task GetUserByTokenAsync_ShouldReturnUser_WhenTokenIsValidAndAttributesContainValidData()
    {
        var mockTokenService = new Mock<ITokenService>();
        var mockData = new Mock<ITokenData>();
        mockTokenService.Setup(x => x.CreateToken(It.IsAny<string>()))
            .Returns(mockData.Object);
        mockTokenService.Setup(x => x.IsAlive(It.IsAny<DateTime>()))
            .Returns(true);
        var mockEncryptionService = new Mock<IEncryptionService>();
        var mockLdapService = new Mock<ILdapService>();

        Dictionary<string, string> values = new()
        {
            [FirstName] = "firstName",
            [LastName] = "lastName",
            [DateOfBirth] = "2000-01-01",
            [Image] = "urlHere",
            [EnrollmentYear] = "2000",
            [Class] = "wit3c"
        };
        mockLdapService.Setup(x => x.GetAttributesForUser(It.IsAny<string>()))
            .Returns(values);
        
        var userService = new UserService(_configuration, mockTokenService.Object, mockEncryptionService.Object, mockLdapService.Object);

        var either = await userService.GetUserByTokenAsync(Array.Empty<byte>(), CancellationToken.None);

        either.State
            .Should()
            .Be(EitherStatus.IsRight);
        var user = either.Case
            .Should()
            .BeOfType<User>()
            .Which;
        user.FirstName.Should().Be(values[FirstName]);
        user.LastName.Should().Be(values[LastName]);
        user.DateOfBirth.Should().Be(DateOnly.Parse(values[DateOfBirth]));
        user.Image.Should().Be(values[Image]);
        user.Class.Should().Be(values[Class]);
        user.EnrollmentYear.Should().Be(int.Parse(values[EnrollmentYear]));
    }
}