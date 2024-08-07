using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;
using WebGame.Domain.ToDo;
using WebGame.Domain.User.Helpers;

namespace WebGame.Domain.User;

public class User : IEntity<int>
{
    public int Id { get; set; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string Password { get; private set; }
    private List<ToDoItem> _toDoItems = new List<ToDoItem>();
    public IReadOnlyCollection<ToDoItem> ToDoItems => _toDoItems.AsReadOnly();



    private const int MIN_PASSWORD_LENGTH = 8;

    private User(int id, string name, string email, string password)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
    }

    public Result<User> Create(int id, string name, string email, string password)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<User>("имя не может быть пустым");
        }

        if (!IsEmailValid(email))
        {
            return Result.Failure<User>("некорректный email");
        }

        if (!IsPasswordValid(password))
        {
            return Result.Failure<User>("невалидный пароль");
        }

        var hashedPass = PasswordHalper.GetHash(password);

        var user = new User(id, name, email, hashedPass);

        return Result.Success<User>(user);
    }

    public Result CheckPasswordCorrectness(string password)
    {
        var hashed = PasswordHalper.GetHash(Password);

        if (hashed != Password)
        {
            return Result.Failure("неверный пароль");
        }

        return Result.Success();
    }

    public Result AddToDoItem(ToDoItem toDoItem)
    {
        if (toDoItem is null)
        {
            return Result.Failure("тудушка не должна быть пустой");
        }

        _toDoItems.Add(toDoItem);
        return Result.Success("тудушка добавлена");
    }

    public Result RemoveToDoItem(ToDoItem toDoItem)
    {
        if (toDoItem == null)
            return Result.Failure("тудушка не должна быть пустой");

        _toDoItems.Remove(toDoItem);

        return Result.Success();
    }

    private bool IsEmailValid(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        return emailRegex.IsMatch(email);
    }

    private bool IsPasswordValid(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        if (password.Length < MIN_PASSWORD_LENGTH)
        {
            return false;
        }

        if (!password.Any(x => char.IsDigit(x)))
        {
            return false;
        }

        return true;
    }
}
