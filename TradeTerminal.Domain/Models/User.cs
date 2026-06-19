namespace TradeTerminal.Domain.Models;

public class User
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string FullName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public Role Role { get; set; }
    public ICollection<Order> Orders { get; set; }
}
