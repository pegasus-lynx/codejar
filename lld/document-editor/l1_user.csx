
public record class User(string Name);

public class DocumentUser
{
	public DocumentUser(User user, UserRole role)
	{
		User = user;
		Role = role;
	}

	public User User { get; init; }
	public UserRole Role { get; private set; }
}