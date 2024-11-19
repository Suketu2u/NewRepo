using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Controllers;
using ToDo.Models;
using ToDo.Service;

namespace ToDo.Tests
{
    [TestFixture]
    public class ToDoControllerTests
    {
        private Mock<IToDoService> _mockToDoService;
        private ToDoController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockToDoService = new Mock<IToDoService>();
            _controller = new ToDoController(_mockToDoService.Object);
        }

        [Test]
        public void GetToDos_ReturnsOkWithToDoList()
        {
            // Arrange
            var mockToDoList = new List<ToDoItem>
            {
                new ToDoItem { Name = "Test ToDo 1", Priority = 1, Status = Status.NotStarted },
                new ToDoItem { Name = "Test ToDo 2", Priority = 2, Status = Status.InProgress}
            };
            _mockToDoService.Setup(service => service.GetToDos()).Returns(mockToDoList);

            // Act
            var result = _controller.GetToDos() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(result.Value, Is.EqualTo(mockToDoList));
        }

        [Test]
        public void AddToDo_WithValidToDo_ReturnsOk()
        {
            // Arrange
            var newToDo = new ToDoItem { Name = "New ToDo", Priority = 1, Status = Status.NotStarted };
            _mockToDoService.Setup(service => service.Add(newToDo)).Returns(newToDo);

            // Act
            var result = _controller.AddToDo(newToDo) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(result.Value, Is.EqualTo(newToDo));
        }

        [Test]
        public void AddToDo_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var duplicateToDo = new ToDoItem { Name = "Duplicate ToDo", Priority = 1, Status = Status.NotStarted };
            _mockToDoService.Setup(service => service.Add(duplicateToDo)).Throws(new ArgumentException("ToDo name must be unique."));

            // Act
            var result = _controller.AddToDo(duplicateToDo) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("ToDo name must be unique."));
        }

        [Test]
        public void EditToDo_WithValidData_ReturnsOk()
        {
            // Arrange
            var toDoId = Guid.NewGuid();
            var updatedToDo = new ToDoItem { Name = "Updated ToDo", Priority = 2, Status = Status.InProgress };
            _mockToDoService.Setup(service => service.Edit(updatedToDo)).Returns(updatedToDo);

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
            //var toDoId = Guid.NewGuid();
            var updatedToDo = new ToDoItem { Name = "Non-existent ToDo", Priority = 1, Status = Status.NotStarted };
            _mockToDoService.Setup(service => service.Edit(updatedToDo)).Throws(new KeyNotFoundException("ToDo not found."));

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
            _mockToDoService.Setup(service => service.Delete(toDoName));

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
            _mockToDoService.Setup(service => service.Delete(toDoName))
                .Throws(new KeyNotFoundException("ToDo with the specified name was not found."));

            // Act
            var result = _controller.DeleteToDo(toDoName) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("ToDo with the specified name was not found."));
        }
        [Test]
        public void DeleteToDo_WithNonCompletedStatus_ReturnsBadRequest()
        {
            // Arrange
            var toDoName = "Not Completed";
            _mockToDoService.Setup(service => service.Delete(toDoName)).Throws(new InvalidOperationException("Only completed to-dos can be deleted."));

            // Act
            var result = _controller.DeleteToDo(toDoName) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Value, Is.EqualTo("Only completed to-dos can be deleted."));
        }
    }
}
