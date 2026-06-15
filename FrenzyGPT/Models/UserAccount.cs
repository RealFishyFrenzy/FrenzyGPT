public class UserAccount
{
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Salt { get; set; } = "";
}