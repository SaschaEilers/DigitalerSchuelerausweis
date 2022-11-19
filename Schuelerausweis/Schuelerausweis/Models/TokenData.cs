using Microsoft.AspNetCore.Mvc;

namespace Schuelerausweis.Models;

public class TokenData
{
    [FromQuery(Name = "id")]
    public string Id { get; set; }
    [FromQuery(Name = "token")]
    public Guid Token { get; set; }
}