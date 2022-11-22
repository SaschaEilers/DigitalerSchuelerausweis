using System.Net;
using FluentAssertions;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using Schuelerausweis.Controllers;
using Schuelerausweis.Models;
using Schuelerausweis.Services;

namespace Schuelerausweis.Test;

[TestClass]
public class UserControllerTest
{
    [TestMethod]
    public async Task UserController_ReturnsBadRequest_WhenUserServiceReturnsInvalidTokenError()
    {
        var mockLogger = new Mock<ILogger<UserController>>();
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(x => 
                x.GetUserByTokenAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InvalidTokenError
            {
                Description = string.Empty
            });
        
        UserController controller = new(mockLogger.Object, mockUserService.Object);
        var result = await controller.GetUserData("oi4r8cBcG4iLPc%2Bico3FIA%3D%3D", CancellationToken.None);
        var responseValue = result
            .Result
            .Should()
            .BeOfType<BadRequest<Error>>()
            .Which
            .Value;
        
        responseValue.Should().NotBeNull();
        responseValue!.Status.Should().Be(HttpStatusCode.BadRequest);
        responseValue.Description.Should().Be("Invalid Token");
    }
    
    [TestMethod]
    public async Task UserController_ReturnsProblem_WhenExceptionIsThrown()
    {
        var mockLogger = new Mock<ILogger<UserController>>();
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(x =>
                x.GetUserByTokenAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some Exception"));
        
        UserController controller = new(mockLogger.Object, mockUserService.Object);
        var responseResult= await controller.Invoking(x => x.GetUserData("oi4r8cBcG4iLPc%2Bico3FIA%3D%3D", It.IsAny<CancellationToken>()))
            .Should()
            .NotThrowAsync();

        var result = responseResult
            .Which
            .Result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .Which;
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.Should().Be("Server could not resolve User");
    }
    
    [TestMethod]
    public async Task UserController_ReturnsProblem_UserServiceReturnsInvalidState()
    {
        var mockLogger = new Mock<ILogger<UserController>>();
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(x =>
                x.GetUserByTokenAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Either<InvalidTokenError, User>.Bottom);
        
        UserController controller = new(mockLogger.Object, mockUserService.Object);
        var responseResult= await controller.Invoking(x => x.GetUserData("oi4r8cBcG4iLPc%2Bico3FIA%3D%3D", It.IsAny<CancellationToken>()))
            .Should()
            .NotThrowAsync();

        var result = responseResult
            .Which
            .Result
            .Should()
            .BeOfType<ProblemHttpResult>()
            .Which;
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.Should().Be("This State should not be reached");
    }
    
    [DataTestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("  ")]
    public async Task UserController_ReturnsBadRequest_WhenTokenIsMissing(string token)
    {
        var mockLogger = new Mock<ILogger<UserController>>();
        var mockUserService = new Mock<IUserService>();
        
        UserController controller = new(mockLogger.Object, mockUserService.Object);
        var result = await controller.GetUserData(token, CancellationToken.None);
        var responseValue = result
            .Result
            .Should()
            .BeOfType<BadRequest<Error>>()
            .Which
            .Value;
        
        responseValue.Should().NotBeNull();
        responseValue!.Status.Should().Be(HttpStatusCode.BadRequest);
        responseValue.Description.Should().Be("Request is missing a Token");
    }
    
    [TestMethod]
    public async Task UserController_ReturnsOkWithSameUserInstanceAsUserServiceProvided_WhenUserServiceReturnsAUser()
    {
        var mockLogger = new Mock<ILogger<UserController>>();
        var mockUserService = new Mock<IUserService>();
        var expectedResult = new User
        {
            FirstName = "firstName",
            LastName = "lastName",
            DateOfBirth = new DateOnly(2000, 01, 01),
            Class = "wit3c",
            EnrollmentYear = 2000,
            Image = "imageName"
        };
        
        mockUserService
            .Setup(x => 
                x.GetUserByTokenAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);
        
        UserController controller = new(mockLogger.Object, mockUserService.Object);
        var result = await controller.GetUserData("oi4r8cBcG4iLPc%2Bico3FIA%3D%3D", CancellationToken.None);
        var responseValue = result
            .Result
            .Should()
            .BeOfType<Ok<User>>()
            .Which
            .Value;

        responseValue.Should().NotBeNull()
            .And.BeSameAs(expectedResult);
    }
}