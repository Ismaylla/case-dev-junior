using System.ComponentModel;
using System.Reflection;

namespace toDo_CaseDev.UnitTests.Model {
    public static class EnumExtensions {
        public static string GetDescription(this Enum value) {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo != null) {
                var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null) {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }
    }

    public class EnumExtensionsTests {
        private enum TestEnum {
            [Description("Test Description")]
            TestValue,

            NoDescription
        }

        [Fact]
        public void GetDescription_WithDescription_ReturnsDescription() {
            // Arrange
            var enumValue = TestEnum.TestValue;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.Equal("Test Description", description);
        }

        [Fact]
        public void GetDescription_WithoutDescription_ReturnsEnumName() {
            // Arrange
            var enumValue = TestEnum.NoDescription;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.Equal("NoDescription", description);
        }
    }
}
