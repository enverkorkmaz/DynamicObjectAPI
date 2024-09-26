using Microsoft.EntityFrameworkCore;
using DynamicObjectAPI.Core.Models;
using DynamicObjectAPI.Data.Repositories;
using DynamicObjectAPI.Service.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicObjectAPI.Data.Repositories.Abstract;
using DynamicObjectAPI.Data;
using DynamicObjectAPI.Service.Helpers;

namespace DynamicObjectAPI.Service.Services.Concrete
{
    public class DynamicObjectService : IDynamicObjectService
    {
        private readonly AppDbContext _context;
        private readonly IDynamicObjectRepository _repository;

        public DynamicObjectService(AppDbContext context, IDynamicObjectRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<DynamicObject>> GetAllObjectsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DynamicObject> GetObjectByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateObjectAsync(DynamicObject dynamicObject)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); // Transaction başlatıldı
            try
            {
                
                DynamicObjectHelper.ConvertFields(dynamicObject.Fields);

                await _repository.AddAsync(dynamicObject);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); 
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateObjectAsync(DynamicObject dynamicObject)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingObject = await _context.DynamicObjects.FirstOrDefaultAsync(x => x.Id == dynamicObject.Id);

                if (existingObject != null)
                {
                    DynamicObjectHelper.ConvertFields(dynamicObject.Fields);

                    _context.Entry(existingObject).State = EntityState.Detached;

                    _context.Attach(dynamicObject);
                    _context.Entry(dynamicObject).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }





        public async Task DeleteObjectAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _repository.DeleteAsync(id);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
