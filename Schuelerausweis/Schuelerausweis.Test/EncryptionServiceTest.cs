using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Schuelerausweis.Services;

namespace Schuelerausweis.Test;

[TestClass]
public class EncryptionServiceTest
{
    private readonly IConfiguration _configuration;
    
    public EncryptionServiceTest()
    {
        var manager = new ConfigurationManager();

        IEnumerable<KeyValuePair<string, string?>> bla = new Dictionary<string, string?>
        {
            ["Encryption:Key"] = "1231231231231231",
            ["Encryption:IV"] = "1231231231231231"
        };
        
        manager.AddInMemoryCollection(bla);
        _configuration = manager;
    }

    [TestMethod]
    public async Task DecryptAsync_ShouldReturnValueWhichStartsWithTheExpectedValue()
    {
        using var encryptionService = new EncryptionService(_configuration);
        var decodedBytes = WebEncoders.Base64UrlDecode("oi4r8cBcG4iLPc+ico3FIA==");
        
        var tokenData= await encryptionService.DecryptAsync(decodedBytes, CancellationToken.None);
        
        tokenData.Should().StartWith("test");
    }
}