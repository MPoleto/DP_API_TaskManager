using System;
using Xunit;
using Moq;
using System.Linq;
using System.Collections.Generic;
using dp_api_taskManager.Persistence.Repository;
using dp_api_taskManager.Service;
using dp_api_taskManager.Models;

namespace dp_api_taskManager.Tests;

public class TaskToDoServiceTest
{
    private readonly Mock<ITaskToDoRepository> _repositoryStub;
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly TaskToDoService _taskToDoService;

    public TaskToDoServiceTest()
    {
        _repositoryStub = new Mock<ITaskToDoRepository>();
        _unitOfWorkStub = new Mock<IUnitOfWork>();
        _taskToDoService = new TaskToDoService(_repositoryStub.Object, _unitOfWorkStub.Object);
    }
    
    [Fact]
    public void GetAll_TasksExist_ReturnList()
    {
        var list = CreateList();

        _repositoryStub.Setup(r => r.ListAll()).Returns(list);

        var result = _taskToDoService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<TaskToDo>>(result);
    }

    [Fact]
    public void GetAll_IfTasksDoNotExist_ReturnNull()
    {
        _repositoryStub.Setup(r => r.ListAll()).Returns((List<TaskToDo>)null);

        var result = _taskToDoService.GetAll();

        Assert.Null(result);
    }

    [Fact]
    public void GetById_TaskExist_ReturnTask()
    {
        var task = CreateTask();

        _repositoryStub.Setup(r => r.FindById(1)).Returns(task);

        var result = _taskToDoService.GetById(1);

        Assert.NotNull(result);
        Assert.IsType<TaskToDo>(result);
    }

    [Fact]
    public void GetById_TaskDoesNotExist_ReturnNull()
    {
        _repositoryStub.Setup(r => r.FindById(1)).Returns((TaskToDo)null);

        var result = _taskToDoService.GetById(1);

        Assert.Null(result);
    }

    [Fact]
    public void GetByTitle_TasksExist_ReturnAListWithSameTitle()
    {
        var list = CreateList();
        var searchedTask = CreateTask();
        var taskTitle = searchedTask.Title;

        _repositoryStub.Setup(r => r.FindByTitle(taskTitle)).Returns(list);

        var result = _taskToDoService.GetByTitle(taskTitle);

        Assert.NotNull(result);
        Assert.IsType<List<TaskToDo>>(result);
    }

    [Fact]
    public void GetByTitle_IfTitleDoNotExist_ReturnNull()
    {
        _repositoryStub.Setup(r => r.FindByTitle("A")).Returns((List<TaskToDo>)null);

        var result = _taskToDoService.GetByTitle("A");

        Assert.Null(result);
    }

    [Fact]
    public void GetByDate_TasksWithDateExist_ReturnAListWithSameDate()
    {
        var list = CreateList();
        var searchedTask = CreateTask();

        var repo = _repositoryStub.Setup(r => r.FindByDate(searchedTask.Date))
            .Returns(list);

        var result = _taskToDoService.GetByDate(searchedTask.Date);

        Assert.NotNull(result);
        _repositoryStub.Verify(r => r.FindByDate(searchedTask.Date), Times.Once);
    }

    [Fact]
    public void GetByDate_IfDateDoNotExist_ReturnNull()
    {
        var searchedTask = CreateTask();
        var taskDate = searchedTask.Date;

        _repositoryStub.Setup(r => r.FindByDate(taskDate))
            .Returns((IEnumerable<TaskToDo>)(null));

        var result = _taskToDoService.GetByDate(taskDate);

        Assert.Null(result);
    }

    [Fact]
    public void GetByStatus_TasksWithStatus_ReturnAListWithSameStatus()
    {
        var list = CreateList();
        var searchedTask = CreateTask();
        var taskStatus = searchedTask.Status;

        _repositoryStub.Setup(r => r.FindByStatus(taskStatus))
            .Returns(list);

        var result = _taskToDoService.GetByStatus(taskStatus);

        Assert.NotNull(result);
        Assert.IsType<List<TaskToDo>>(result);
        _repositoryStub.Verify(r => r.FindByStatus(taskStatus), Times.Once);
    }

    [Fact]
    public void GetByStatus_IfTaskWithStatusDoNotExist_ReturnNull()
    {
        var list = CreateList();

        _repositoryStub.Setup(r => r.FindByStatus(EnumStatusTask.Finalizado))
            .Returns((IEnumerable<TaskToDo>)(null));

        var result = _taskToDoService.GetByStatus(EnumStatusTask.Finalizado);

        Assert.Null(result);
    }

    [Fact]
    public void Create_AddTask_ReturnNewTask()
    {
        var task = CreateTask();

        _repositoryStub.Setup(r => r.FindByTitle(task.Title))
            .Returns(new List<TaskToDo>());
        
        _repositoryStub.Setup(r => r.Add(task));
        _unitOfWorkStub.Setup(r => r.Complete());

        var result = _taskToDoService.Create(task);

        Assert.NotNull(result);
        Assert.IsType<TaskToDo>(result);
    }
    
    [Fact]
    public void Update_UpdateTask_ReturnTask()
    {
        var task = CreateTask();

        _repositoryStub.Setup(r => r.Update(task));
        _unitOfWorkStub.Setup(r => r.Complete());

        var result = _taskToDoService.Update(task);

        Assert.NotNull(result);
        Assert.IsType<TaskToDo>(result);
    }

    [Fact]
    public void Delete_DeleteTask()
    {
        var task = CreateTask();

        _taskToDoService.Delete(task);

        _repositoryStub.Verify(r => r.Remove(task), Times.Once);
        _unitOfWorkStub.Verify(r => r.Complete(), Times.Once);
    }

    private TaskToDo CreateTask()
    {
        return new TaskToDo()
        {
            Id = 1,
            Title = "To do",
            Description = "Description Test",
            Date = DateTime.Today,
            Status = EnumStatusTask.Pendente
        };
    }

    private List<TaskToDo> CreateList()
    {
        return new List<TaskToDo>()
        {
            new TaskToDo()
            {
                Id = 1,
                Title = "To do 1",
                Description = "Description Test 1",
                Date = DateTime.Now.AddDays(3),
                Status = EnumStatusTask.Iniciado
            },
            new TaskToDo()
            {
                Id = 2,
                Title = "To do 2",
                Description = "Description Test 2",
                Date = DateTime.Now.AddDays(12),
                Status = EnumStatusTask.Pendente
            },
            new TaskToDo()
            {
                Id = 3,
                Title = "To do 3",
                Description = "Description Test 3",
                Date = DateTime.Now.AddDays(5),
                Status = EnumStatusTask.Pendente
            }
        };
    }
}