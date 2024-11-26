using ToDo.Models;

namespace ToDo.Service
{
    public class ToDoService : IToDoService
    {
        private readonly List<ToDoItem> _toDos = new();

        public List<ToDoItem> GetToDos() => _toDos.OrderByDescending(x => x.Status).ThenBy(y => y.Priority).ToList();

        public ToDoItem Add(ToDoItem newToDo)
        {
            ValidateToDo(newToDo, true);
            
            var category = newToDo.Priority > 3 ? Category.Urgent : Category.Normal;
            
            newToDo.Category = category;
            _toDos.Add(newToDo);
            return newToDo;
        }

        public ToDoItem Edit(ToDoItem updatedToDo)
        {
            ValidateToDo(updatedToDo, false);
            var toDo = _toDos.FirstOrDefault(t => t.Name.Equals(updatedToDo.Name, StringComparison.OrdinalIgnoreCase));
            if (toDo == null) throw new KeyNotFoundException("ToDo not found.");

            toDo.Name = updatedToDo.Name;
            toDo.Priority = updatedToDo.Priority;
            toDo.Category = updatedToDo.Priority > 3 ? Category.Urgent : Category.Normal;
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
        
        // Private method for validation
        private void ValidateToDo(ToDoItem toDo, bool isNew)
        {
            if (string.IsNullOrWhiteSpace(toDo.Name))
                throw new ArgumentException("ToDo name is required.");

            if (toDo.Priority <= 0)
                throw new ArgumentException("Priority must be greater than 0.");

            if (isNew)
            {
                // Ensure unique name for new ToDos
                if (_toDos.Any(t => t.Name.Equals(toDo.Name, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException("ToDo name must be unique.");
            }
            else
            {
                // Ensure unique name for editing ToDos
                if (_toDos.Count(t => t.Name.Equals(toDo.Name, StringComparison.OrdinalIgnoreCase)) > 1)
                    throw new ArgumentException("ToDo name must be unique.");
            }
        }
    }
}
