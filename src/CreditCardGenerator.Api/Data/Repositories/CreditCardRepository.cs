using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditCardGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditCardGenerator.Api.Data.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApiDbContext _context;

        public CreditCardRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<CreditCard> Select(Guid id)
        {
            return await _context.CreditCards.FindAsync(id);
        }

        public async Task<IEnumerable<CreditCard>> SelectAll(string email)
        {
            return await _context.CreditCards
                .Where(x => x.Email == email && x.Active)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task Insert(CreditCard creditCard)
        {
            _context.CreditCards.Add(creditCard);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var creditCard = await _context.CreditCards.FindAsync(id);
            if (creditCard != null)
            {
                _context.CreditCards.Remove(creditCard);
                await _context.SaveChangesAsync();
            }
        }

    }
}
