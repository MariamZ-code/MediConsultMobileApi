using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Category>> GetAll()
        {
            return await dbContext.Categories.AsNoTracking().ToListAsync();
        }


        public List<CountOfCategoriesDTO> GetCountOfCategories() =>
                    dbContext.Providers.Include(c => c.Category)
                .Where(p => p.provider_status == "Activated")
                .GroupBy(ps => new { ps.Category_ID, ps.Category.Category_Name_En, ps.Category.Category_Name_Ar })
                .Select(g => new CountOfCategoriesDTO
                {
                    Category_Id = g.Key.Category_ID,
                    ProviderCount = g.Count(),
                    Category_Name_En = g.Key.Category_Name_En,
                    Category_Name_Ar = g.Key.Category_Name_Ar
                })
                .OrderBy(x => x.Category_Id)
                    .ToList();

        public List<CountOfCategoriesDTO> GetCategories() =>
                   dbContext.Providers.Include(c => c.Category)
               .Where(p => p.provider_status == "Activated"&& (p.Category_ID ==3 || p.Category_ID==4))
               .GroupBy(ps => new { ps.Category_ID, ps.Category.Category_Name_En, ps.Category.Category_Name_Ar })
               .Select(g => new CountOfCategoriesDTO
               {
                   Category_Id = g.Key.Category_ID,
                   ProviderCount = g.Count(),
                   Category_Name_En = g.Key.Category_Name_En,
                   Category_Name_Ar = g.Key.Category_Name_Ar
               })
               .OrderBy(x => x.Category_Id)
                   .ToList();
    }
}
