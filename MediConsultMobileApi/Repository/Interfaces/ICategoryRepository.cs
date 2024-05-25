using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        List<CountOfCategoriesDTO> GetCountOfCategories();
        List<CountOfCategoriesDTO> GetCategories();
    }
}
