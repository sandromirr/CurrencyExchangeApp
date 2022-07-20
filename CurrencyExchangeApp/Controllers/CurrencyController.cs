using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApp.Models.ViewModels;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.Exceptions;
using CurrencyExchangeApp.Database;

namespace CurrencyExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await _unitOfWork.Currency.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
        }

        [HttpPost("CreateRate")]
        public async Task<IActionResult> CreateRate([FromBody] CreateCurrencyRateviewModel createCurrencyRateViewModel)
        {
            try
            {
                await _unitOfWork.Currency.CreateCurrencyRate(createCurrencyRateViewModel);
                await _unitOfWork.CompleteAsync();
                return NoContent() ;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCurrencyViewModel createCurrencyViewModel)
        {
            try
            {
                await _unitOfWork.Currency.Create(createCurrencyViewModel);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (CurrencyExchangeException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
                var result = await _unitOfWork.Currency.GetCurrencyRate(currencyRateViewModel);
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
                var result = await _unitOfWork.Currency.ExchangeCurrency(currencyExchangeViewModel);
                await _unitOfWork.CompleteAsync();
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
