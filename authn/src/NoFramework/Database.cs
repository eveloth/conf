namespace NoFramework;

public class Database
{
    private static readonly List<User> Users = new();
    private static int _index;

    public User? this[int i] => Users.FirstOrDefault(x => x.Id == i);

    public User? this[string username] => Users.FirstOrDefault(x => x.Username == username);

    public User Add(User user)
    {
        _index++;
        user.Id = _index;
        Users.Add(user);
        return Users.Last();
    }
}

public record User(string Username, string Password)
{
    public int Id { get; set; }
}
