using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CreditCardGenerator.Api.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            Id = Guid.NewGuid();
        }
        
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public bool Active { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        public bool isValid()
        {
            var result = new CreditCardValidator(this);
            return result.isValid();
        }
    }

    public class CreditCardValidator
    {
        CreditCard _creditCard;
        public CreditCardValidator(CreditCard creditCard)
        {
            _creditCard = creditCard;
        }

        public bool isValid()
        {
            //MasterCard
            var sequence = new string(_creditCard.CardNumber).Reverse().ToArray();
            var length = sequence.Length;

            // criar regex cartao
            bool regex = !true;
            if (regex)
                return false;

            int soma = 0;
            for (int i = 0; i < length; i++)
            {
                int digito;
                Int32.TryParse(sequence[i].ToString(), out digito);
                if ((i + 1) % 2 == 0)
                {
                    if ((digito *= 2) > 9)
                    {
                        digito -= 9;
                    }
                }

                soma = soma + digito;
            }

            if (soma % 10 == 0 && soma > 0)
                return true;

            return false;
        }
    }
}
