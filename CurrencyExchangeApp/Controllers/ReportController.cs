using CurrencyExchangeApp.Database;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> GetReports([FromBody] AccountReportFilter accountReportFilter)
        {
            try
            {
                var reports = await _unitOfWork.Account.GetAccountReports(accountReportFilter);
                return Ok(reports);
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
    }
}
