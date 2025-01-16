namespace BLL.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(): base("Entity is already exists"){}
}