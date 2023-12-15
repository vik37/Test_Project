namespace DotNet.TestProject.IdentityDomainModels.Models;

public class Gender
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<User> Users { get; set; }
}