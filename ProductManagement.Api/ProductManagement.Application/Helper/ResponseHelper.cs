using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.ResponseModel;

namespace ProductManagement.Application.Helper
{
    public class ResponseHelper : ControllerBase
    {
        public IActionResult CreateResponse(bool success, string message, object? data = null)
        {
            int statusCode = 200;

            if (!success)
            {
                if (message.Contains("Invalid", StringComparison.OrdinalIgnoreCase))
                    statusCode = 400;
                else if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    statusCode = 404;
                else if (message.Contains("error", StringComparison.OrdinalIgnoreCase))
                    statusCode = 500;
            }

            var response = new ProductResponse
            {
                Success = success,
                Message = message,
                Data = data
            };

            return StatusCode(statusCode, response);
        }
    }
}
