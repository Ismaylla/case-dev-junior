using TaskApi.Exceptions;

namespace toDo_CaseDev.UnitTests.Exception
{
    public class ApiErrorResponseTests
    {
        [Fact]
        public void Constructor_WithSingleMessage_SetsStatusAndMessage()
        {
            // Arrange
            var status = "Error";
            var message = "Test error message";

            // Act
            var response = new ApiErrorResponse(status, message);

            // Assert
            Assert.Equal(status, response.Status);
            Assert.Single(response.Mensagens);
            Assert.Equal(message, response.Mensagens.First());
        }

        [Fact]
        public void Constructor_WithMultipleMessages_SetsStatusAndMessages()
        {
            // Arrange
            var status = "Error";
            var messages = new[] { "Error 1", "Error 2", "Error 3" };

            // Act
            var response = new ApiErrorResponse(status, messages);

            // Assert
            Assert.Equal(status, response.Status);
            Assert.Equal(3, response.Mensagens.Count());
            Assert.Equal(messages, response.Mensagens);
        }
    }
}
