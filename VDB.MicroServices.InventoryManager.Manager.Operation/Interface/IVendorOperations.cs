using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Interface
{
    public interface IVendorOperations
    {
        public void Create(Vendor vendor);
        public void Delete(Vendor vendor);
        public void Update(Vendor vendor);
        public Task<Vendor> GetVendorAsync(Guid id, params Expression<Func<Vendor, object>>[] includes);
        public Task<Vendor> GetVendorByNameAsync(string name);
        public Task<List<Vendor>> SearchByName(string name, params Expression<Func<Vendor, object>>[] includes);
        Task<List<Vendor>> GetAllVendors(params Expression<Func<Vendor, object>>[] includes);
    }
}
