using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using spy_detection.Data;

namespace spy_detection.Api
{
    public class SpyRepository: ISpyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SpyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Spy>> ToListAsync()
        {
            return _dbContext.Spies.ToListAsync();
        }

        public Task<Spy> GetByIdAsync(int id)
        {
            return _dbContext.FindAsync<Spy>(id);
        }

        public async Task<Spy> CreateAsync(Spy spy)
        {
            var entry = await _dbContext.Spies.AddAsync(spy);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Spy> UpdateAsync(Spy spy)
        {
            var updatedEntry = _dbContext.Update(spy);
            await _dbContext.SaveChangesAsync();

            return updatedEntry.Entity;
        }

        public Task DeleteAsync(Spy spy)
        {
            _dbContext.Remove(spy);
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<Spy> GetAllAsQueryable()
        {
            return _dbContext.Spies.AsQueryable();
        }
    }
}
