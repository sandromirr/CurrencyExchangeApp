using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApp.Repositories;
using CurrencyExchangeApp.Models.ViewModels;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.Exceptions;

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
        public async Task<ActionResult> CurrencyRate(CurrencyRateViewModel currencyRateViewModel)
        {
            try
            {
                var result = await _currencyRepository.GetCurrencyRate(currencyRateViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Exchange",Name = nameof(Exchange))]
        public async Task<IActionResult> Exchange([FromBody] CurrencyExchangeViewModel currencyExchangeViewModel)
        {
            try
            {
                var result = await _currencyRepository.ExchangeCurrency(currencyExchangeViewModel);
                return Ok(result);
            }
            catch (CurrencyExchangeException ex) 
            {
                var currencyExchangeError = new CurrencyExchangeError()
                {
                    Message = ex.Message,
                    VisibleAccount = (ex.currencyExhangeExceptionEnum == CurrencyExhangeExceptionEnum.AnnonymousExchangeAmountExceeded || ex.currencyExhangeExceptionEnum == CurrencyExhangeExceptionEnum.AnnonymousExchangeAmountExceeded)
                };
                
                return StatusCode(StatusCodes.Status500InternalServerError, currencyExchangeError);
            }
            catch (Exception ex) // todo exception to -> currenct exchange exception
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
