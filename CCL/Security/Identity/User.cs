namespace CCL.Security.Identity;

public class User
{
    public int UserId { get; set; }
    public List<Role> Roles { get; set; }

    public User(int userId, List<Role> roles)
    {
        UserId = userId;
        Roles = roles;
    }
}