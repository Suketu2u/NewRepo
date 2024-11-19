using ToDo.Models;

namespace ToDo.Service
{
    public interface IToDoService
    {
        List<ToDoItem> GetToDos();
        ToDoItem Add(ToDoItem newToDo);
        ToDoItem Edit(ToDoItem updatedToDo);
        void Delete(string name);
    }
}
