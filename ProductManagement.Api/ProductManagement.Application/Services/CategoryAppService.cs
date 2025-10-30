using ProductManagement.Application.Interfaces;
using ProductManagement.Application.RequestModels;
using ProductManagement.Application.ResponseModel;
using ProductManagement.Application.ViewModels;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Models;

namespace ProductManagement.Application.Services
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly IRepository<Category> _repositoryCategory;

        public CategoryAppService(IRepository<Category> repositoryCategory)
        {
            _repositoryCategory = repositoryCategory;
        }

        public List<CategoryViewModel> GetCategories()
        {
            return _repositoryCategory.GetAll()
                .OrderByDescending(c => c.CategoryId)
                .Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                }).ToList();
        }

        public ProductResponse ManageCategory(CategoryAddUpdateRequest request)
        {
            try
            {
                if (!request.CategoryId.HasValue || request.CategoryId == 0)
                {
                    var category = new Category
                    {
                        CategoryName = request.CategoryName,
                        Description = request.Description
                    };

                    _repositoryCategory.Add(category);
                    _repositoryCategory.SaveChanges();

                    return new ProductResponse { Success = true, Message = "Category added successfully." };
                }
                else
                {
                    var category = _repositoryCategory.GetAll().FirstOrDefault(c => c.CategoryId == request.CategoryId.Value);
                    if (category == null)
                        return new ProductResponse { Success = false, Message = "Category not found." };

                    category.CategoryName = request.CategoryName;
                    category.Description = request.Description;

                    _repositoryCategory.Update(category);
                    _repositoryCategory.SaveChanges();

                    return new ProductResponse { Success = true, Message = "Category updated successfully." };
                }
            }
            catch (Exception ex)
            {
                return new ProductResponse { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public ProductResponse DeleteCategory(int id)
        {
            try
            {
                var category = _repositoryCategory.GetAll().FirstOrDefault(c => c.CategoryId == id);
                if (category == null)
                    return new ProductResponse { Success = false, Message = "Category not found." };

                _repositoryCategory.Remove(category);
                _repositoryCategory.SaveChanges();

                return new ProductResponse { Success = true, Message = "Category deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ProductResponse { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
    }
}
