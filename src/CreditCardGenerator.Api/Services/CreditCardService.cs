using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditCardGenerator.Api.Data.Repositories;
using CreditCardGenerator.Api.Models;

namespace CreditCardGenerator.Api.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _repository;

        public CreditCardService(ICreditCardRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreditCard> GetCreditCard(Guid id)
        {
            return await _repository.Select(id);
        }

        public async Task<IEnumerable<CreditCard>> GetCreditCardsByEmail(string email)
        {
            return await _repository.SelectAll(email);
        }

        public async Task<CreditCard> Generate(string email)
        {
            var creditCard = new CreditCard
            {
                Email = email
            };

            Random random = new Random();

            //MasterCard
            var iin = $"{random.Next(510000, 559999):000000}";
            var account = $"{random.Next(0, 999999999):000000000}";

            string sequence = new string(($"{iin}{account}").Reverse().ToArray());
            var length = sequence.Length;

            int soma = 0;
            for (int i = 0; i < length; i++)
            {
                var digito = Int32.Parse(sequence[i].ToString());
                if ((i + 1) % 2 > 0)
                {
                    if ((digito *= 2) > 9)
                    {
                        digito -= 9;
                    }
                }

                soma = soma + digito;
            }

            var verfication = (10 - (soma % 10 != 0 ? soma % 10 : 10)).ToString();

            string number = $"{iin}{account}{verfication}";

            creditCard.CardNumber = number;
            creditCard.CreatedAt = DateTime.Now;
            creditCard.Active = true;

            await _repository.Insert(creditCard);
            return creditCard;
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

    }
}
