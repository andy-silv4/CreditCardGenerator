using System;
using System.Collections.Generic;
using CreditCardGenerator.Api.Models;
using Xunit;

namespace CreditCardGenerator.Api.Tests.Fixtures
{
    [CollectionDefinition(nameof(CreditCardCollection))]
    public class CreditCardCollection : ICollectionFixture<CreditCardFixture>
    {    
    }
    public class CreditCardFixture
    {
        public IList<CreditCard> ValidList(int count){
            IList<CreditCard> creditCards = new List<CreditCard>();

            for (int i = 0; i < count; i++)
            {
                creditCards.Add(this.Valid());
            }

            return creditCards;
        }
        public CreditCard Valid(){

            var creditCard = new CreditCard()
            {
                Id = Guid.NewGuid(),
                Email = this.getEmail(),
                CardNumber = this.getCardNumber(),
                CreatedAt = DateTime.Now,
                Active = true
            };

            return creditCard;
        }
        public CreditCard Invalid(){
            var creditCard = new CreditCard()
            {
                Id = Guid.NewGuid(),
                Email = this.getEmail(false),
                CardNumber = this.getCardNumber(false),
                CreatedAt = DateTime.Now
            };

            return creditCard;
        }

        private string getCardNumber(bool valid = true)
        {
            string[] validNumbers = {"5319039864352227", "5359498197221072", "5281721453156523"};
            string[] invalidNumbers = {"5319 0398 6435 2227", "5359 4981 9722 1075", "1111 2222 3333 4444"};

            Random random = new Random();
            if (valid)
                return validNumbers[random.Next(0, 2)];
            else
                return invalidNumbers[random.Next(0, 2)];
        }

        private string getEmail(bool valid = true)
        {
            string[] emails = {"gmail", "bol", "yahoo"};
            string[] names = {"joao", "maria", "jose"};

            Random random = new Random();
            string firstName = names[random.Next(0, 2)];
            string lastName = names[random.Next(0, 2)];
            
            string email = "";
            if (valid)
                email = $"{firstName}.{lastName}@{emails[random.Next(0, 2)]}.com";
            else
                email = $"{firstName}.{lastName}{emails[random.Next(0, 2)]}";

            return email;
        }
        
    }
}