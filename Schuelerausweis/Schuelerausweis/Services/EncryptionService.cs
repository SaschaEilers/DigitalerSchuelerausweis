using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Schuelerausweis.Constants;
using Schuelerausweis.Models;

namespace Schuelerausweis.Services;

public sealed class EncryptionService : IEncryptionService, IDisposable
{
    private readonly Aes _encryption;
    private readonly Encoding _encoding;
    public EncryptionService(IConfiguration configuration)
    {
        _encryption = Aes.Create();
        _encoding = Encoding.UTF8;
        var options = configuration
            .GetRequiredSection(ConfigurationSections.Encryption)
            .Get<EncryptionConfiguration>()!;
        _encryption.Mode = CipherMode.CBC;
        _encryption.Padding = PaddingMode.Zeros;
        _encryption.KeySize = 128;
        _encryption.BlockSize = 128;
        _encryption.IV = _encoding.GetBytes(options.IV);
        _encryption.Key = _encoding.GetBytes(options.Key, 0,15);
    }
    
    public async Task<string> DecryptAsync(byte[] cypherText, CancellationToken cancellationToken)
    {
        using ICryptoTransform decryptor = _encryption.CreateDecryptor();
        await using MemoryStream stream = new(cypherText);
        await using CryptoStream cryptoStream = new(stream, decryptor, CryptoStreamMode.Read);
        using StreamReader reader = new(cryptoStream, _encoding);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    public void Dispose()
    {
        _encryption.Dispose();
    }
}

public interface IEncryptionService
{
    public Task<string> DecryptAsync(byte[] cypherText, CancellationToken cancellationToken);
}