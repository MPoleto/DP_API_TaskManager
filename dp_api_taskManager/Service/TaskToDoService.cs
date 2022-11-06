using dp_api_taskManager.Models;
using dp_api_taskManager.Persistence.Repository;

namespace dp_api_taskManager.Service
{
    public class TaskToDoService : ITaskToDoService
    {
        private readonly ITaskToDoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

    public TaskToDoService(ITaskToDoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<TaskToDo> GetAll()
    {
      return _repository.ListAll();
    }

    public TaskToDo GetById(int id)
    {
      return _repository.FindById(id);
    }

    public IEnumerable<TaskToDo> GetByTitle(string title)
    {
      return _repository.FindByTitle(title);
    }

    public IEnumerable<TaskToDo> GetByDate(DateTime date)
    {
      return _repository.FindByDate(date);
    }

    public IEnumerable<TaskToDo> GetByStatus(EnumStatusTask status)
    {
      return _repository.FindByStatus(status);
    }

    public TaskToDo Create(TaskToDo task)
    {
      try
      {
        _repository.Add(task);
        _unitOfWork.Complete();
      }
      catch (Exception e)
      {
        throw new Exception("Erro ao salvar", e);
      }
      return task;
    }

    public TaskToDo Update(TaskToDo task)
    {
      _repository.Update(task);
      _unitOfWork.Complete();

      return task;
    }

    public void Delete(TaskToDo task)
    {
      _repository.Remove(task);
      _unitOfWork.Complete();
    }

  }
}