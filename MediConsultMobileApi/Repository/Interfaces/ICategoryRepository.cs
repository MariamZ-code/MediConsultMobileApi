using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
    }
}
