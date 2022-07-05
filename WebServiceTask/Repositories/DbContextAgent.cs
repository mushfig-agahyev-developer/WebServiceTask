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
        public List<PersonDTO> GetAllPersonal(GetAllRequest request)
        {
            List<PersonDTO> _personal = _db.Personal.AsNoTracking().Include(y => y.Address)
                .Where(r =>
               (string.IsNullOrEmpty(request.FirstName) || r.FirstName.Contains(request.FirstName)) &&
               (string.IsNullOrEmpty(request.LastName) || r.FirstName.Contains(request.LastName)) &&
               (string.IsNullOrEmpty(request.City) || ((r.Address != null) && r.Address.City.Contains(request.City))))
               .OrderByDescending(r => r.Id).Skip(request.PageCount * (request.Page - 1))
               .Take(request.PageCount).Select(y => (PersonDTO)y).ToList();

            return _personal;
        }

        public async Task<List<PersonDTO>> GetAllPersonalAsync(GetAllRequest request)
        {
            List<PersonDTO> _personal = await _db.Personal.AsNoTracking().Include(y => y.Address)
                   .Where(r =>
               (string.IsNullOrEmpty(request.FirstName) || r.FirstName.Contains(request.FirstName)) &&
               (string.IsNullOrEmpty(request.LastName) || r.FirstName.Contains(request.LastName)) &&
               (string.IsNullOrEmpty(request.City) || ((r.Address != null) && r.Address.City.Contains(request.City))))
               .OrderByDescending(r => r.Id).Skip(request.PageCount * (request.Page - 1))
               .Take(request.PageCount).Select(y => (PersonDTO)y).ToListAsync();

            return _personal;
        }

        public int PersonalCount(GetAllRequest request)
        {
            int _personalCount = _db.Personal.AsNoTracking().Count(r =>
            (string.IsNullOrEmpty(request.FirstName) || r.FirstName.Contains(request.FirstName)) &&
            (string.IsNullOrEmpty(request.LastName) || r.FirstName.Contains(request.LastName)) &&
            (string.IsNullOrEmpty(request.City) || ((r.Address != null) && r.Address.City.Contains(request.City))));

            return _personalCount;
        }

        public async Task<int> PersonalCountAsync(GetAllRequest request)
        {
            int _personalCount = await _db.Personal.CountAsync(r =>
            (string.IsNullOrEmpty(request.FirstName) || r.FirstName.Contains(request.FirstName)) &&
            (string.IsNullOrEmpty(request.LastName) || r.FirstName.Contains(request.LastName)) &&
            (string.IsNullOrEmpty(request.City) || ((r.Address != null) && r.Address.City.Contains(request.City))));
            return _personalCount;
        }

        public long Save(PersonDTO personDTO)
        {
            try
            {
                Person person = new Person();
                person.FirstName = personDTO.firstName;
                person.LastName = personDTO.lastName;
                person.Address = new Address()
                { City = personDTO.address.City, AddressLine = personDTO.address.AddressLine };

                var _duplicate = _db.Personal.Include(y => y.Address)
                .Where(r => (r.FirstName == person.FirstName && r.LastName == person.LastName) ||
                (r.Address.City == person.Address.City && r.Address.AddressLine == person.Address.AddressLine))
                .FirstOrDefault();

                _db.Add(person);

                if (_db.SaveChanges() > 0)
                    return person.Id;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                var log = ex.ToString();
                return -1;
            }
        }

        public async Task<long> SaveAsync(PersonDTO personDTO)
        {
            try
            {
                Person person = new Person();
                person.FirstName = personDTO.firstName;
                person.LastName = personDTO.lastName;
                person.Address = new Address()
                { City = personDTO.address.City, AddressLine = personDTO.address.AddressLine };

                await _db.SaveChangesAsync();
                _db.Add(person);

                if (await _db.SaveChangesAsync() > 0)
                    return person.Id;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                var log = ex.ToString();
                return -1;
            }
        }
    }
}
