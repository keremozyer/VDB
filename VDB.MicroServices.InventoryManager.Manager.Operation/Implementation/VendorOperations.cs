using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Implementation
{
    public class VendorOperations : IVendorOperations
    {
        private readonly IInventoryManagerUnitOfWork DB;

        public VendorOperations(IInventoryManagerUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(Vendor vendor)
        {
            this.DB.VendorRepository.Create(vendor);
        }

        public void Delete(Vendor vendor)
        {
            this.DB.VendorRepository.Delete(vendor);
        }

        public void Update(Vendor vendor)
        {
            this.DB.VendorRepository.Update(vendor);
        }

        public async Task<Vendor> GetVendorAsync(Guid id, params Expression<Func<Vendor, object>>[] includes)
        {
            return await this.DB.VendorRepository.GetFirstAsync(v => v.Id == id, includes: includes);
        }

        public async Task<Vendor> GetVendorByNameAsync(string name)
        {
            return await this.DB.VendorRepository.GetFirstAsync(v => v.Name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Searches db for vendors starting with given name. Makes case invariant comparison.
        /// </summary>
        /// <param name="name">Full or partial vendor name</param>
        /// <param name="includes">Other entities to include in search.</param>
        /// <returns>List of vendors.</returns>
        public async Task<List<Vendor>> SearchByName(string name, params Expression<Func<Vendor, object>>[] includes)
        {
            return await this.DB.VendorRepository.GetAsync(v => v.Name.ToLower().StartsWith(name), includes: includes);
        }

        public async Task<List<Vendor>> GetAllVendors(params Expression<Func<Vendor, object>>[] includes)
        {
            return await this.DB.VendorRepository.GetAsync(filter: null, includes: includes);
        }
    }
}
