using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApp.Repositories;
using CurrencyExchangeApp.Models.ViewModels;
using CurrencyExchangeApp.Models.Exceptions;

namespace CurrencyExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository _accountRepository)
        {
            this._accountRepository = _accountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _accountRepository.GetAccounts());
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
                await _accountRepository.Create(createAccountViewModel);
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
                var account = await _accountRepository.GetAccountByPersonalNumber(personalNumber);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
