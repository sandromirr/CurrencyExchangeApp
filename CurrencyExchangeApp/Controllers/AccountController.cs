using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApp.Models.ViewModels;
using CurrencyExchangeApp.Database;

namespace CurrencyExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public AccountController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _unitOfWork.Account.GetAccounts());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountViewModel createAccountViewModel)
        {
            try
            {
                await _unitOfWork.Account.Create(createAccountViewModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Get", Name = nameof(Get))]
        public async Task<IActionResult> Get([FromQuery] string personalNumber)
        {
            try
            {
                var account = await _unitOfWork.Account.GetAccountByPersonalNumber(personalNumber);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
