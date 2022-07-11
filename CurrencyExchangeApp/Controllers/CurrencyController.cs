using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApp.Repositories;

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
    }
}
