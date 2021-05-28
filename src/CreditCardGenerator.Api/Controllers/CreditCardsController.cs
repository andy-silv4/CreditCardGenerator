using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CreditCardGenerator.Api.Models;
using CreditCardGenerator.Api.Services;

namespace CreditCardGenerator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private readonly ICreditCardService _service;

        public CreditCardsController(ICreditCardService service)
        {
            _service = service;
        }
        
        [HttpGet("{email}")]
        public async Task<ActionResult<IEnumerable<CreditCard>>> GetCreditCards(string email)
        {
            var creditCards = await _service.GetCreditCardsByEmail(email);
            return Ok(creditCards);
        }
        
        // [HttpGet("{id}")]
        // public async Task<ActionResult<CreditCard>> GetCreditCard(Guid id)
        // {
        //     var creditCard = await _service.GetCreditCard(id);

        //     if (creditCard == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(creditCard);
        // }

        [HttpPost]
        public async Task<ActionResult<CreditCard>> PostCreditCard(CreditCard creditCard)
        {
            creditCard = await _service.Generate(creditCard.Email);
            // return CreatedAtAction(nameof(GetCreditCards), new { id = creditCard.Id }, creditCard);
            return CreatedAtAction(nameof(GetCreditCards), new { email = creditCard.Email }, creditCard);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<CreditCard>> DeleteCreditCard(Guid id)
        {
            var creditCard = await _service.GetCreditCard(id);
            if (creditCard == null)
            {
                return NotFound();
            }

            await _service.Delete(id);

            return NoContent();
        }
    }
}
