using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using dp_api_taskManager.Models;
using dp_api_taskManager.Service;
using dp_api_taskManager.Controllers;

namespace dp_api_taskManager.Tests
{
    public class TaskToDoControllerTest
    {
        private readonly TaskToDoController _controller;
        private readonly Mock<ITaskToDoService> _serviceStub;

        public TaskToDoControllerTest()
        {
            _serviceStub = new Mock<ITaskToDoService>();
            _controller = new TaskToDoController(_serviceStub.Object);
        }

        [Fact]
        public void GetById_ExistTaskToDoWithId_ReturnTaskOfThisId()
        {
            var task = CreateTask();
            _serviceStub.Setup(s => s.GetById(task.Id)).Returns(task);

            var result = _controller.GetById(task.Id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetById_DoNotExistTaskToDoWithId_ReturnNotFound()
        {
            var result = _controller.GetById(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAll_ExistTasks_ReturnListTasks()
        {
            var tasks = CreateList();
            _serviceStub.Setup(s => s.GetAll()).Returns(tasks);

            var result = _controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAll_DoNotExistTasks_ReturnOk()
        {
            var result = _controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetByTitle_ExistTaskToDoWithTitle_ReturnTaskWithThisTitle()
        {
            var taskList = CreateList();
            var task = CreateTask();
            _serviceStub.Setup(s => s.GetByTitle(task.Title)).Returns(taskList);

            var result = _controller.GetByTitle(task.Title);

            Assert.IsType<OkObjectResult>(result);            
        }

        [Fact]
        public void GetByTitle_DoNotExistTaskWithTitle_ReturnNotFound()
        {
             _serviceStub.Setup(s => s.GetByTitle("title"))
                .Returns((IEnumerable<TaskToDo>)null);

            var result = _controller.GetByTitle("title");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetByDate_ExistTaskToDoWithDate_ReturnTaskWithThisDate()
        {
            var taskList = CreateList();
            var task = CreateTask();
            _serviceStub.Setup(s => s.GetByDate(task.Date)).Returns(taskList);

            var result = _controller.GetByDate(task.Date);

            Assert.IsType<OkObjectResult>(result);            
        }

        [Fact]
        public void GetByDate_DoNotExistTaskWithisDate_ReturnNotFound()
        {
            
            _serviceStub.Setup(s => s.GetByDate(DateTime.MaxValue)).Returns((IEnumerable<TaskToDo>)null);
            
            var result = _controller.GetByDate(DateTime.MaxValue);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetByStatus_ExistTaskToDoWithStatus_ReturnTaskWithThisStatus()
        {
            var taskList = CreateList();
            var task = CreateTask();
            _serviceStub.Setup(s => s.GetByStatus(task.Status)).Returns(taskList);

            var result = _controller.GetByStatus(task.Status);

            Assert.IsType<OkObjectResult>(result);
        }   

        [Fact]
        public void GetByStatus_DoNotExistTaskWithisStatus_ReturnNotFound()
        {
            _serviceStub.Setup(s => s.GetByStatus(EnumStatusTask.Pendente)).Returns((IEnumerable<TaskToDo>)null);
            
            var result = _controller.GetByStatus(EnumStatusTask.Pendente);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_AddTask_ReturnCreatedAtAction()
        {
            var task = CreateTask();
            _serviceStub.Setup(s => s.Create(task)).Returns(task);

            var result = _controller.Create(task);

            Assert.IsType<CreatedAtActionResult>(result);
        }   

        [Fact]
        public void Create_WhenModelInvalid_ReturnBadRequest()
        {
            var task = new TaskToDo();
            _controller.ModelState.AddModelError("Title", "Campo obrigatório");
            
            var result = _controller.Create(task);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Create_WhenDateMinValue_ReturnBadRequest()
        {
            var task = new TaskToDo()
            {
                Id = 1,
                Title = "To do",
                Description = "Description Test",
                Date = DateTime.MinValue,
                
            };
            
            var result = _controller.Create(task);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Update_UpdateCorrectly_ReturnOk()
        {
            var task = CreateTask();
            _serviceStub.Setup(s => s.GetById(task.Id)).Returns(task);
            _serviceStub.Setup(s => s.Update(task)).Returns(task);

            var result = _controller.Update(task.Id, task);

            Assert.IsType<OkObjectResult>(result);
        }   

        [Fact]
        public void Update_UpdateWithIncorrectID_ReturnNotFoundTask()
        {
            var task = CreateTask();

            var result = _controller.Update(5, task);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_WhenModelInvalid_ReturnBadRequest()
        {
            var task = CreateTask();
            _controller.ModelState.AddModelError("Title", "Campo obrigatório");
            
            var result = _controller.Create(task);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_UpdateCorrectly_ReturnNoContent()
        {
            var task = CreateTask();
            _serviceStub.Setup(s => s.GetById(task.Id)).Returns(task);
            _serviceStub.Setup(s => s.Delete(task));

            var result = _controller.Delete(task.Id);

            Assert.IsType<NoContentResult>(result);
        }   

        [Fact]
        public void Delete_UpdateWithIncorrectID_ReturnNotFoundTask()
        {
            var task = CreateTask();

            var result = _controller.Delete(5);

            Assert.IsType<NotFoundResult>(result);
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
}