namespace DotNet.TestProject.IdentityDomainModels.Models;

public class User : IdentityUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public int? GenderId { get; set; }
    public Gender Gender { get; set; }

    public Address Address { get; set; }
}
