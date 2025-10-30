using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.RequestModels;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.ResponseModel;
using ProductManagement.Application.Helper;
using ProductManagement.Shared.Common;

namespace ProductManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly ILogger<ProductController> _logger;
        private readonly ResponseHelper _responseHelper;
        public ProductController(IProductAppService productAppService, ILogger<ProductController> logger,
            ResponseHelper responseHelper)
        {
            _productAppService = productAppService;
            _logger = logger;
            _responseHelper = responseHelper;
        }

        [HttpGet("list")]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _productAppService.GetProducts();

                if (products == null || !products.Any())
                    return _responseHelper.CreateResponse(false, string.Format(Constant.NOTFOUND, "products"));

                return _responseHelper.CreateResponse(true, string.Format(Constant.SUCCESS, "products"), products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(Constant.ERROR, "products"));
                return _responseHelper.CreateResponse(false, string.Format(Constant.ERROR, "products"));
            }
        }
        [HttpPost("manage")]
        public IActionResult ManageProduct([FromBody] ProductAddUpdateRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.ProductName))
                    return _responseHelper.CreateResponse(false, Constant.INVALID);

                var response = _productAppService.ManageProduct(request);
                var products = _productAppService.GetProducts();

                if (!response.Success)
                    return _responseHelper.CreateResponse(false, response.Message, products);

                return _responseHelper.CreateResponse(true, string.Format(Constant.MANAGE_SUCCESS, "Product"), products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(Constant.ERROR, "product"));
                return _responseHelper.CreateResponse(false, string.Format(Constant.ERROR, "product"));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var response = _productAppService.DeleteProduct(id);
                var products = _productAppService.GetProducts();

                if (!response.Success)
                    return _responseHelper.CreateResponse(false, response.Message, products);

                return _responseHelper.CreateResponse(true, string.Format(Constant.DELETE_SUCCESS, "Product"), products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(Constant.ERROR, "product"));
                return _responseHelper.CreateResponse(false, string.Format(Constant.ERROR, "product"));
            }
        }
    }
}
