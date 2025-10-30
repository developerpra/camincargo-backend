namespace ProductManagement.Application.RequestModels
{
    public class CategoryAddUpdateRequest
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
