using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.RequestModels;
using ProductManagement.Application.Helper;
using ProductManagement.Shared.Common;

namespace ProductManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly ILogger<CategoryController> _logger;
        private readonly ResponseHelper _responseHelper;

        public CategoryController(ICategoryAppService categoryAppService, ILogger<CategoryController> logger, ResponseHelper responseHelper)
        {
            _categoryAppService = categoryAppService;
            _logger = logger;
            _responseHelper = responseHelper;
        }

        [HttpGet("list")]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _categoryAppService.GetCategories();

                if (categories == null || !categories.Any())
                    return _responseHelper.CreateResponse(false, string.Format(Constant.NOTFOUND, "categories"));

                return _responseHelper.CreateResponse(true, string.Format(Constant.SUCCESS, "categories"), categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(Constant.ERROR, "categories"));
                return _responseHelper.CreateResponse(false, string.Format(Constant.ERROR, "categories"));
            }
        }

        [HttpPost("manage")]
        public IActionResult ManageCategory([FromBody] CategoryAddUpdateRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.CategoryName))
                    return _responseHelper.CreateResponse(false, Constant.INVALID);

                var response = _categoryAppService.ManageCategory(request);
                var categories = _categoryAppService.GetCategories();

                if (!response.Success)
                    return _responseHelper.CreateResponse(false, response.Message, categories);

                return _responseHelper.CreateResponse(true, string.Format(Constant.MANAGE_SUCCESS, "Category"), categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(Constant.ERROR, "category"));
                return _responseHelper.CreateResponse(false, string.Format(Constant.ERROR, "category"));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var response = _categoryAppService.DeleteCategory(id);
                var categories = _categoryAppService.GetCategories();

                if (!response.Success)
                    return _responseHelper.CreateResponse(false, response.Message, categories);

                return _responseHelper.CreateResponse(true, string.Format(Constant.DELETE_SUCCESS, "Category"), categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(Constant.ERROR, "category"));
                return _responseHelper.CreateResponse(false, string.Format(Constant.ERROR, "category"));
            }
        }
    }
}
