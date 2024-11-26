using Microsoft.AspNetCore.Mvc;
using ToDo.Controllers;
using ToDo.Models;
using ToDo.Service;

namespace ToDo.Tests
{
    [TestFixture]
    public class ToDoControllerTests
    {
        private IToDoService toDoService;
        private ToDoController _controller;

        [SetUp]
        public void SetUp()
        {
            toDoService = new ToDoService(); // Use the actual service
            _controller = new ToDoController(toDoService);
        }

        [Test]
        public void GetToDos_ReturnsOkWithToDoList()
        {
            // Arrange
            var toDo1 = new ToDoItem { Name = "Test ToDo 1", Priority = 1, Status = Status.NotStarted };
            var toDo2 = new ToDoItem { Name = "Test ToDo 2", Priority = 2, Status = Status.InProgress };
            var mockList = new List<ToDoItem>
            {
                toDo1,
                toDo2
            }.OrderByDescending(x => x.Status).ThenBy(x => x.Priority);

            _controller.AddToDo(toDo1);
            _controller.AddToDo(toDo2);

            // Act
            var result = _controller.GetToDos() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(result.Value, Is.EqualTo(mockList));
        }

        [Test]
        public void AddToDo_WithValidToDo_ReturnsOk()
        {
            // Arrange
            var newToDo = new ToDoItem { Name = "New ToDo", Priority = 1, Status = Status.NotStarted };

            // Act
            var result = _controller.AddToDo(newToDo) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(result.Value, Is.EqualTo(newToDo));
        }
        [Test]
        public void AddToDo_CategoryBasedOnPriority_ReturnsOk()
        {
            // Arrange
            var newToDo = new ToDoItem { Name = "New ToDo", Priority = 4, Status = Status.NotStarted };

            // Act
            var result = _controller.AddToDo(newToDo) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result); 
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(((ToDoItem)result.Value).Category == Category.Urgent);
            Assert.That(result.Value, Is.EqualTo(newToDo));
        }
        [Test]
        public void AddToDo_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var duplicateToDo = new ToDoItem { Name = "Duplicate ToDo", Priority = 1, Status = Status.NotStarted };

            // Act
            _controller.AddToDo(duplicateToDo);
            var result = _controller.AddToDo(duplicateToDo) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("ToDo name must be unique."));
        }

        [Test]
        public void EditToDo_WithValidData_ReturnsOk()
        {
            // Arrange
            var updatedToDo = new ToDoItem { Name = "Updated ToDo", Priority = 2, Status = Status.InProgress };
            _controller.AddToDo(updatedToDo);
            
            // Act
            var result = _controller.EditToDo(updatedToDo) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(result.Value, Is.EqualTo(updatedToDo));
        }

        [Test]
        public void EditToDo_WithNonExistentToDo_ReturnsNotFound()
        {
            // Arrange
            var updatedToDo = new ToDoItem { Name = "Non-existent ToDo", Priority = 1, Status = Status.NotStarted };

            // Act
            var result = _controller.EditToDo(updatedToDo) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("ToDo not found."));
        }

        [Test]
        public void DeleteToDo_WithCompletedStatus_ReturnsNoContent()
        {
            // Arrange
            var toDoName = "Completed";
            var newToDo = new ToDoItem { Name = "Completed", Priority = 4, Status = Status.Completed };
            _controller.AddToDo(newToDo);

            // Act
            var result = _controller.DeleteToDo(toDoName) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        [Test]
        public void DeleteToDoByName_WithNonExistentName_ReturnsNotFound()
        {
            // Arrange
            var toDoName = "NonExistent";
            
            // Act
            var result = _controller.DeleteToDo(toDoName) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("ToDo not found."));
        }
        [Test]
        public void DeleteToDo_WithNonCompletedStatus_ReturnsBadRequest()
        {
            // Arrange
            var toDoName = "Not Completed";
            var newToDo = new ToDoItem { Name = "Not Completed", Priority = 4, Status = Status.InProgress };
            _controller.AddToDo(newToDo);

            // Act
            var result = _controller.DeleteToDo(toDoName) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("Only completed to-dos can be deleted."));
        }
    }
}
