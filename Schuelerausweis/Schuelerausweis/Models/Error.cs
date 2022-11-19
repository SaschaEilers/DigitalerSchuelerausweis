using System.Net;

namespace Schuelerausweis.Models;

public class Error
{
    public required HttpStatusCode Status { get; set; }
    public required string Description { get; set; }
}