namespace NoFramework;

public class Database
{
    private static readonly List<User> Users = new();

    public User? this[int i] => Users.FirstOrDefault(x => x.Id == i);

    public User? this[string username] => Users.FirstOrDefault(x => x.Username == username);

    public User Add(User user)
    {
        Users.Add(user);
        return Users.Last();
    }
}

public record User
{
    private static int _index;

    public User(string Username, string Password)
    {
        this.Username = Username;
        this.Password = Password;
        Id = ++_index;
    }

    public int Id { get; init; }
    public string Username { get; }
    public string Password { get; }
}
