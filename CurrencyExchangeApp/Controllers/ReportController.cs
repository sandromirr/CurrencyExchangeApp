using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.Exceptions;
using CurrencyExchangeApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public ReportController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetReports([FromBody] AccountReportFilter accountReportFilter)
        {
            try
            {
                var reports = await _accountRepository.GetAccountReports(accountReportFilter);
                return Ok(reports);
            }
            catch (CurrencyExchangeException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex) // todo exception to -> currenct exchange exception
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
