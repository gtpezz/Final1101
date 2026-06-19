namespace TradeTerminal.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string FullName { get; set; }
    public string Login { get; set; }
}
