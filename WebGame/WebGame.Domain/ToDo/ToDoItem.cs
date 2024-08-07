using CSharpFunctionalExtensions;
using WebGame.Domain.Enums;

namespace WebGame.Domain.ToDo;

public partial class Domain
{
    public class ToDoItem : IEntity<int>
    {
        public int Id { get; }
        public string? Name;
        public string? Description { get; }
        public TodoStatus? Status { get; private set; }
        public DateTime? CreatedAt { get; }
        public DateTime? DueDate { get; private set; }

        private ToDoItem(int id, string name, string description, DateTime dueDate)
        {
            Id = id;
            Description = description;
            Name = name;
            Status = TodoStatus.None;
            CreatedAt = DateTime.UtcNow;
            DueDate = dueDate;
        }

        public static Result<ToDoItem> Create(int id, string name, string description, DateTime dueDate)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result.Failure<ToDoItem>("Название должно быть не пустым");
            }

            if (dueDate < DateTime.UtcNow)
            {
                return Result.Failure<ToDoItem>("Дата окончания должна быть больше текщей");
            }

            var toDo = new ToDoItem(id, name, description, dueDate);

            return Result.Success(toDo);
        }

        public Result UpdateName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return Result.Failure("новое имя не должно быть пустым");
            }

            Name = newName;
            return Result.Success();
        }

        public Result Complete()
        {
            Status = TodoStatus.Done;
            return Result.Success();
        }

        public Result ChangeDueDate(DateTime newDueDateTime)
        {
            if (newDueDateTime < CreatedAt)
            {
                return Result.Failure("дата окончания не может быть меньше даты начала");
            }

            DueDate = newDueDateTime;
            return Result.Success();
        }
    }
}
