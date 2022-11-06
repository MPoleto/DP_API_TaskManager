using dp_api_taskManager.Models;

namespace dp_api_taskManager.Service
{
    public interface ITaskToDoService
    {
        TaskToDo GetById(int id);
        IEnumerable<TaskToDo> GetByTitle(string title);
        IEnumerable<TaskToDo> GetByDate(DateTime date);
        IEnumerable<TaskToDo> GetByStatus(EnumStatusTask status);
        IEnumerable<TaskToDo> GetAll();
        TaskToDo Create(TaskToDo task);
        TaskToDo Update(TaskToDo task);
        void Delete(TaskToDo task);
    }
}