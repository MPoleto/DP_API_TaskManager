using dp_api_taskManager.Models;

namespace dp_api_taskManager.Persistence.Repository
{
    public interface ITaskToDoRepository
    {
        TaskToDo FindById(int id);
        IEnumerable<TaskToDo> FindByTitle(string title);
        IEnumerable<TaskToDo> FindByDate(DateTime date);
        IEnumerable<TaskToDo> FindByStatus(EnumStatusTask status);
        IEnumerable<TaskToDo> ListAll();
        void Add(TaskToDo task);
        void Update(TaskToDo task);
        void Remove(TaskToDo task);
    }
}