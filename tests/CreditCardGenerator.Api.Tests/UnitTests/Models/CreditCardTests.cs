using CreditCardGenerator.Api.Tests.Fixtures;
using Xunit;

namespace CreditCardGenerator.Api.Tests.Models
{
    [Collection(nameof(CreditCardCollection))]
    public class CreditCardTests
    {
        private readonly CreditCardFixture _fixture;

        public CreditCardTests(CreditCardFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreditCard_NewGenerated_ShouldBeValid()
        {
            // Arrange
            var creditCard = _fixture.Valid();
            
            // Act
            var valid = creditCard.isValid();

            // Assert
            Assert.True(valid);
        }

        [Fact]
        public void CreditCard_NewGenerated_ShouldBeInvalid()
        {
            // Arrange
            var creditCard = _fixture.Invalid();
            
            // Act
            var invalid = creditCard.isValid();

            // Assert
            Assert.False(invalid);
        }
    }
}
