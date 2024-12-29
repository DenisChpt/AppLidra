namespace AppLidra.Shared.Models;

public class UserResponse(string userName)
{
    public string UserName { get; set; } = userName;
}
