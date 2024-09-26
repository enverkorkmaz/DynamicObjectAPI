using DynamicObjectAPI.Core.Models;
using DynamicObjectAPI.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Data.Repositories
{
    public class DynamicObjectRepository : IDynamicObjectRepository
    {
        private readonly AppDbContext _context;

        public DynamicObjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DynamicObject>> GetAllAsync()
        {
            return await _context.DynamicObjects.ToListAsync();
        }

        public async Task<DynamicObject> GetByIdAsync(int id)
        {
            return await _context.DynamicObjects.FindAsync(id);
        }

        public async Task AddAsync(DynamicObject dynamicObject)
        {
            await _context.DynamicObjects.AddAsync(dynamicObject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DynamicObject dynamicObject)
        {
            _context.DynamicObjects.Update(dynamicObject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dynamicObject = await GetByIdAsync(id);
            if (dynamicObject != null)
            {
                _context.DynamicObjects.Remove(dynamicObject);
                await _context.SaveChangesAsync();
            }
        }
    }
}
