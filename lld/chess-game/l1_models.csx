#load "l1_enums.csx"

public readonly record struct User(string Name);

public record class Player
{
	public Player(User user, Color color)
	{
		User = user;
		Color = color;
	}

	public User User { get; init; }
	public Color Color { get; init; }
	public string Name => User.Name;

	public override string ToString() => $"{Name} : {Color}";
}