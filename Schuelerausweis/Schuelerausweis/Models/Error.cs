using System.Net;

namespace Schuelerausweis.Models;

public class Error
{
    public HttpStatusCode Status { get; set; }
    public string Description { get; set; }
}