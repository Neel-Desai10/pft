using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Utility
{
    public class BaseApiController : ControllerBase
    {
        protected new IActionResult Ok(object successMessage)
        {
            return base.Ok(Envelope.Ok(successMessage));
        }
         protected new IActionResult Ok(string errorMessage)
        {
            return base.Ok(Envelope.Ok(errorMessage));
        }
        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }
        protected IActionResult Ok<T>(T result,string successMessage)
        {
            return base.Ok(Envelope.Ok(result,successMessage));
        }

            protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.ErrorMessage(errorMessage));
        }
    }
}