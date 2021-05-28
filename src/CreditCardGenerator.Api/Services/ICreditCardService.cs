using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreditCardGenerator.Api.Models;

namespace CreditCardGenerator.Api.Services
{
    public interface ICreditCardService
    {
        Task<CreditCard> GetCreditCard(Guid id);
        Task<IEnumerable<CreditCard>> GetCreditCardsByEmail(string email);
        Task<CreditCard> Generate(string email);
        Task Delete(Guid id);
    }
}
