namespace MessangerBackend.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public DateTime LastSeenOnline { get; set; }
}

public class UserLoginRegisterDTO
{
    public string Nickname { get; set; }
    public string Password { get; set; }
}
