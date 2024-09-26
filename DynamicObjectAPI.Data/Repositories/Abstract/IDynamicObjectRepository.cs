using DynamicObjectAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Data.Repositories.Abstract
{
    public interface IDynamicObjectRepository
    {
        Task<IEnumerable<DynamicObject>> GetAllAsync();
        Task<DynamicObject> GetByIdAsync(int id);
        Task AddAsync(DynamicObject dynamicObject);
        Task UpdateAsync(DynamicObject dynamicObject);
        Task DeleteAsync(int id);
    }
}
