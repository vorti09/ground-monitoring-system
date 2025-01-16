using CCL.Security.Identity;

namespace CCL.Security;

public class SecurityContext
{
    private static User _user = null!;

    public static User GetUser()
    {
        return _user;
    }

    public static void SetUser(User user)
    {
        _user = user;
    }
}