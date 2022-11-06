using dp_api_taskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace dp_api_taskManager.Persistence.Repository
{
    public class TaskToDoRepository : ITaskToDoRepository
    {
        private OrganizerContext _context;

        public TaskToDoRepository(OrganizerContext context)
        {
            _context = context;
        }

        public TaskToDo FindById(int id)
        {
        return _context.TaskToDos.Find(id);
        }

        public IEnumerable<TaskToDo> ListAll()
        {
        return _context.TaskToDos.AsNoTracking().ToList();
        }

        public IEnumerable<TaskToDo> FindByDate(DateTime date)
        {
        return _context.TaskToDos.AsNoTracking().Where(p => p.Date == date);
        }

        public IEnumerable<TaskToDo> FindByStatus(EnumStatusTask status)
        {
        return _context.TaskToDos.AsNoTracking().Where(p => p.Status == status);
        }

        public IEnumerable<TaskToDo> FindByTitle(string title)
        {
        return _context.TaskToDos.AsNoTracking().Where(p => p.Title.Contains(title));
        }

        public void Add(TaskToDo task)
        {
            _context.TaskToDos.Add(task);
        }

        public void Update(TaskToDo task)
        {
            _context.Update(task);
        }

        public void Remove(TaskToDo task)
        {
            _context.TaskToDos.Remove(task);
        }
  }
}