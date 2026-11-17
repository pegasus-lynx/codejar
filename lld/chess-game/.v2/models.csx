
public readonly record struct User(string Name);

public record class Player(User User, Color Color);