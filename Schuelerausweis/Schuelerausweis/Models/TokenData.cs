using Microsoft.AspNetCore.Mvc;

namespace Schuelerausweis.Models;

public class TokenData : ITokenData
{
    public required string Id { get; set; }
    public required DateTime CreationDate { get; set; }
}

public interface ITokenData
{
    public string Id { get; }
    public DateTime CreationDate { get; }
}