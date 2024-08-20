using System.ComponentModel.DataAnnotations;

namespace MessangerBackend.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string Nickname { get; set; }
    public DateTime LastSeenOnline { get; set; }
}

public class UserLoginRegisterDTO
{
    [MaxLength(20)]
    public string Nickname { get; set; }
    public string Password { get; set; }
}
