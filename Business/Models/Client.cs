namespace Business.Models;

public class Client
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ClientName { get; set; } = null!;
}
