namespace toDo_CaseDev.UnitTests.Model
{
    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            [System.ComponentModel.Description("Test Description")]
            TestValue,
            
            NoDescription
        }

        [Fact]
        public void GetDescription_WithDescription_ReturnsDescription()
        {
            // Arrange
            var enumValue = TestEnum.TestValue;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.Equal("Test Description", description);
        }

        [Fact]
        public void GetDescription_WithoutDescription_ReturnsEnumName()
        {
            // Arrange
            var enumValue = TestEnum.NoDescription;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.Equal("NoDescription", description);
        }
    }
}
