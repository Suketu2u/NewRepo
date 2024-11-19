using System.Xml.Linq;
using ToDo.Models;

namespace ToDo.Service
{
    public class ToDoService : IToDoService
    {
        private readonly List<ToDoItem> _toDos = new();

        public List<ToDoItem> GetToDos() => _toDos.OrderByDescending(x => x.Status).ThenBy(y => y.Priority).ToList();

        public ToDoItem Add(ToDoItem newToDo)
        {
            if (_toDos.Any(t => t.Name.Equals(newToDo.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("ToDo name must be unique.");

            if (string.IsNullOrWhiteSpace(newToDo.Name))
                throw new ArgumentException("ToDo name is required.");

            _toDos.Add(new ToDoItem()
            {
                Name = newToDo.Name,
                Priority = newToDo.Priority,
                Status = newToDo.Status,
            });
            return newToDo;
        }

        public ToDoItem Edit(ToDoItem updatedToDo)
        {
            var toDo = _toDos.FirstOrDefault(t => t.Name.Equals(updatedToDo.Name, StringComparison.OrdinalIgnoreCase));
            if (toDo == null) throw new KeyNotFoundException("ToDo not found.");

            if (_toDos.Where(t => t.Name.Equals(updatedToDo.Name, StringComparison.OrdinalIgnoreCase)).Count() > 1)
                throw new ArgumentException("ToDo name must be unique.");

            toDo.Name = updatedToDo.Name;
            toDo.Priority = updatedToDo.Priority;
            toDo.Status = updatedToDo.Status;
            return toDo;
        }

        public void Delete(string name)
        {
            var toDo = _toDos.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (toDo == null) throw new KeyNotFoundException("ToDo not found.");

            if (toDo.Status != Status.Completed)
                throw new InvalidOperationException("Only completed to-dos can be deleted.");

            _toDos.Remove(toDo);
        }
    }
}
