using System.Linq;
using CreditCardGenerator.Api.Data.Repositories;
using CreditCardGenerator.Api.Services;
using CreditCardGenerator.Api.Tests.Fixtures;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace CreditCardGenerator.Api.Tests.Services
{
    [Collection(nameof(CreditCardCollection))]
    public class CreditCardRepositoryTests
    {
        private readonly CreditCardFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly ICreditCardService _service;

        public CreditCardRepositoryTests(CreditCardFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<CreditCardService>();
        }

        [Fact]
        public void CreditCardService_GetCreditCardsByEmail_ShouldReturnCreditCards()
        {
            // Arrange
            _mocker.GetMock<ICreditCardRepository>()
                .Setup(r => r.SelectAll("teste@teste.com").Result)
                .Returns(_fixture.ValidList(50));

            // Act
            var creditCards = _service.GetCreditCardsByEmail("teste@teste.com");
            
            // Assert
            _mocker.GetMock<ICreditCardRepository>().Verify(r => r.SelectAll("teste@teste.com"), Times.Once);
            Assert.Equal(50, creditCards.Result.Count(c => c.Active));
        }

        [Fact]
        public void CreditCardService_Generate_ShouldBeExecutedWithSuccess()
        {
            // Arrange
            string email = _fixture.Valid().Email;

            // Act
            var inserted = _service.Generate(email).Result;

            // Assert
            _mocker.GetMock<ICreditCardRepository>().Verify(r => r.Insert(inserted), Times.Once);
            Assert.True(inserted.isValid());
        }

        [Fact]
        public void CreditCardService_Generate_ShouldFail()
        {
            // Arrange
            var creditCard = _fixture.Invalid();

            // Act
            var inserted = _service.Generate(creditCard.Email).Result;
            inserted = creditCard;

            // Assert
            _mocker.GetMock<ICreditCardRepository>().Verify(r => r.Insert(inserted), Times.Never);
            Assert.False(inserted.isValid());
        }
    }
}
