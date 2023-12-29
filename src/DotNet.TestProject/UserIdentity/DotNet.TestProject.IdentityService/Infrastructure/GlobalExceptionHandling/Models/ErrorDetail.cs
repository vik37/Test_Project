using System.Text.Json;

namespace DotNet.TestProject.IdentityService.Infrastructure.GlobalExceptionHandling.Models;

#pragma warning disable
public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public List<string> Messages { get; set; } = new List<string>();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}