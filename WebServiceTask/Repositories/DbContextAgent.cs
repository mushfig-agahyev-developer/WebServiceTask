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
        public List<PersonDTO> GetAllPersonal(BaseFilter filter)
        {
            List<PersonDTO> _personal = _db.Personal.AsNoTracking().Include(y => y.Address)
                .Where(r => string.IsNullOrEmpty(filter.Search) ||r.FirstName.Contains(filter.Search) ||
                r.LastName.Contains(filter.Search) || r.Address.City.Contains(filter.Search) ||
                r.Address.AddressLine.Contains(filter.Search)).OrderByDescending(r => r.Id)
                .Skip(filter.PageCount * (filter.Page - 1))
                .Take(filter.PageCount).Select(y => (PersonDTO)y).ToList();

            return _personal;
        }

        public async Task<List<PersonDTO>> GetAllPersonalAsync(BaseFilter filter)
        {
            List<PersonDTO> _personal = await _db.Personal.AsNoTracking().Include(y => y.Address)
                .Where(r => string.IsNullOrEmpty(filter.Search) || r.FirstName.Contains(filter.Search) ||
                r.LastName.Contains(filter.Search) || r.Address.City.Contains(filter.Search) ||
                r.Address.AddressLine.Contains(filter.Search)).OrderByDescending(r => r.Id)
                .Skip(filter.PageCount * (filter.Page - 1))
                .Take(filter.PageCount).Select(y => (PersonDTO)y).ToListAsync();

            return _personal;
        }

        public int PersonalCount(BaseFilter filter)
        {
            int _personalCount = _db.Personal.AsNoTracking()
                 .Where(r => string.IsNullOrEmpty(filter.Search) || r.FirstName.Contains(filter.Search) ||
                 r.LastName.Contains(filter.Search) || r.Address.City.Contains(filter.Search) ||
                 r.Address.AddressLine.Contains(filter.Search)).Count();
            return _personalCount;
        }

        public async Task<int> PersonalCountAsync(BaseFilter filter)
        {
            int _personalCount = await _db.Personal.AsNoTracking()
                 .Where(r => string.IsNullOrEmpty(filter.Search) || r.FirstName.Contains(filter.Search) ||
                 r.LastName.Contains(filter.Search) || r.Address.City.Contains(filter.Search) ||
                 r.Address.AddressLine.Contains(filter.Search)).CountAsync();
            return _personalCount;
        }
    }
}
