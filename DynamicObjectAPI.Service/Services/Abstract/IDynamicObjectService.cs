using DynamicObjectAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Service.Services.Abstract
{
    public interface IDynamicObjectService
    {
        Task<IEnumerable<DynamicObject>> GetAllObjectsAsync();
        Task<DynamicObject> GetObjectByIdAsync(int id);
        Task CreateObjectAsync(DynamicObject dynamicObject);
        Task UpdateObjectAsync(DynamicObject dynamicObject);
        Task DeleteObjectAsync(int id);
    }
}
