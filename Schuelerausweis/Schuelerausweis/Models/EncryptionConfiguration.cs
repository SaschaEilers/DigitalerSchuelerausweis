namespace Schuelerausweis.Models;

public class EncryptionConfiguration
{
    public required string Key { get; set; }
    public required string IV { get; set; }
}