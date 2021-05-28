using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreditCardGenerator.Api.Models;

namespace CreditCardGenerator.Api.Data.Repositories
{
    public interface ICreditCardRepository
    {
        Task<CreditCard> Select(Guid id);
        Task<IEnumerable<CreditCard>> SelectAll(string email);
        Task Insert(CreditCard creditCard);
        Task Delete(Guid id);
        
    }
}
