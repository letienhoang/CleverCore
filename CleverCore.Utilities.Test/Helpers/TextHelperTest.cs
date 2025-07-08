using CleverCore.Utilities.Helpers;
using Xunit;

namespace CleverCore.Utilities.Test.Helpers
{
    public class TextHelperTest
    {
        [Theory]
        [InlineData("Ha Noi Viet Nam", "ha-noi-viet-nam")]
        [InlineData("Hà Nội -- Việt Nam?", "ha-noi-viet-nam")]
        [InlineData("Hà Nội Việt Nam 123", "ha-noi-viet-nam-123")]
        public void ToUnsignString_InputContainsDiacritics_ReturnUnsignString(string input, string expected)
        {
            // Arrange
            // Act
            string result = TextHelper.ToUnsignString(input);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1234567890, "một tỷ hai trăm ba mươi bốn triệu năm trăm sáu mươi bảy nghìn tám trăm chín mươi đồng chẵn")]
        [InlineData(-1234567890, "Âm một tỷ hai trăm ba mươi bốn triệu năm trăm sáu mươi bảy nghìn tám trăm chín mươi đồng chẵn")]
        [InlineData(0, "không đồng chẵn")]
        public void ToString_InputIsDecimal_ReturnCorrectStringRepresentation(decimal input, string expected)
        {
            // Arrange
            // Act
            string result = TextHelper.ToString(input);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
