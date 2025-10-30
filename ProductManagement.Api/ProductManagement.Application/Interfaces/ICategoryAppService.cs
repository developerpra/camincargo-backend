using ProductManagement.Application.RequestModels;
using ProductManagement.Application.ResponseModel;
using ProductManagement.Application.ViewModels;

namespace ProductManagement.Application.Interfaces
{
    public interface ICategoryAppService
    {
        List<CategoryViewModel> GetCategories();
        ProductResponse ManageCategory(CategoryAddUpdateRequest request);
        ProductResponse DeleteCategory(int id);
    }
}
