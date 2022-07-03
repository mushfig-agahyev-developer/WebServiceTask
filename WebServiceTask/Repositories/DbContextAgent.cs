using Microsoft.EntityFrameworkCore;
using WebServiceTask.DAL;
using WebServiceTask.DTO;
using WebServiceTask.FilterModels;
using WebServiceTask.Interfaces;
using WebServiceTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Repositories
{
    public class DbContextAgent : IDbContextAgent
    {
        private readonly AppDbContext _db;

        public DbContextAgent(AppDbContext appDataBase) => _db = appDataBase;

        //All Requests DbContext will supported from this repository.
        public List<AddressDTO> GetAll(BaseFilter filter)
        {
            List<AddressDTO> adresses = _db.Addresses.AsNoTracking()
                .Where(r => string.IsNullOrEmpty(filter.Search) || r.AddressLine.Contains(filter.Search)).OrderByDescending(r => r.Id)
                .Skip(filter.PageCount * (filter.Page - 1))
                .Take(filter.PageCount).Select(y => (AddressDTO)y).ToList();

            return adresses;
        }

        public async Task<List<AddressDTO>> GetAllAsync(BaseFilter filter)
        {
            List<AddressDTO> adresses = await _db.Addresses.AsNoTracking()
                .Where(r => string.IsNullOrEmpty(filter.Search) || r.AddressLine.Contains(filter.Search)).OrderByDescending(r => r.Id)
              .Skip(filter.PageCount * (filter.Page - 1))
              .Take(filter.PageCount).Select(y => (AddressDTO)y).ToListAsync();

            return adresses;
        }

    }
}
