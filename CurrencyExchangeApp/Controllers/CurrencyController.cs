using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApp.Repositories;
using CurrencyExchangeApp.Models.ViewModels;

namespace CurrencyExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository _currencyRepository)
        {
            this._currencyRepository = _currencyRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await _currencyRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCurrencyViewModel createCurrencyViewModel)
        {
            try
            {
                await _currencyRepository.Create(createCurrencyViewModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Rate", Name = nameof(CurrencyRate))]
        public ActionResult CurrencyRate(CurrencyRateViewModel currencyRateViewModel)
        {
            try
            {
                var result = _currencyRepository.GetCurrencyRate(currencyRateViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Exchange")]
        public IActionResult Exchange()
        {
            return NoContent();
        }
    }
}
