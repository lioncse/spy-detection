using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using spy_detection.Data;

namespace spy_detection.Api
{
    public interface ISpyRepository
    {
        IQueryable<Spy> GetAllAsQueryable();
        Task<List<Spy>> ToListAsync();
        Task<Spy> GetByIdAsync(int id);
        Task<Spy> CreateAsync(Spy spy);
        Task<Spy> UpdateAsync(Spy spy);
        Task DeleteAsync(Spy spy);
    }
}
