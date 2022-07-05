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
using WebServiceTask.Helpers;

namespace WebServiceTask.Repositories
{
    public class DbContextAgent : IDbContextAgent
    {
        private readonly AppDbContext _db;

        public DbContextAgent(AppDbContext appDataBase) => _db = appDataBase;

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
               (string.IsNullOrEmpty(request.LastName) || r.LastName.Contains(request.LastName)) &&
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

                if (personDTO.address != null)
                    person.Address = new Address()
                    { City = personDTO.address.City, AddressLine = personDTO.address.AddressLine };

                var _current = _db.Personal.FirstOrDefault(y => y.FirstName.Equals(person.FirstName) ||
                     y.LastName.Equals(person.LastName));

                if (_current != null)
                {
                    _current.FirstName = person.FirstName;
                    _current.LastName = person.LastName;

                    _db.Entry(_current).Reference(r => r.Address).Load();

                    if (person.Address != null)
                    {
                        if (_current.Address == null)
                            _current.Address = person.Address;
                        else
                        {
                            _current.Address.City = person.Address.City;
                            _current.Address.AddressLine = person.Address.AddressLine;
                        }

                    }
                    else
                        if (_current.Address != null)
                        _db.Addresses.Remove(_current.Address);

                    if (!_db.ChangeTracker.HasChanges())
                        goto Error;

                    if (_db.SaveChanges() > 0)
                        return _current.Id;
                    else
                        goto Error;
                }

                /*
                 I configure that 
                firstname, lastname, City, addressLine must unique.
                And in Fluent api was wroted Contraints.
                About update I don't see any Id for update that's why I configured that the code always was updated
                if all datas equals ChangeTracker.HasChanges() was returned -1;

                For the case update with ID
                 if (await _db.Personal.AnyAsync(y => y.Id == person.Id && y.FirstName.Equals(person.FirstName) || y.LastName.Equals(person.LastName)))
                    return -1;

                if (person.Address != null && await _db.Addresses.AnyAsync(y => y.Id == person.Address.Id && y.City.Equals(person.Address.City) ||
                    y.AddressLine.Equals(person.Address.AddressLine)))
                    return -1;

                So that a invalid request does not go to the database.
                And if will come then will fall catch. Database will not accept.

                As a unique date, you could be choose not a column, but a row.
                 */

                _db.Add(person);

                if (_db.SaveChanges() > 0)
                    return person.Id;
                else
                    goto Error;
            }
            catch (Exception ex)
            {
                var log = ex.ToString();
                goto Error;
            }
        Error: return -1;
        }

        public async Task<long> SaveAsync(PersonDTO personDTO)
        {
            try
            {
                Person person = new Person();
                person.FirstName = personDTO.firstName;
                person.LastName = personDTO.lastName;

                if (personDTO.address != null)
                    person.Address = new Address()
                    { City = personDTO.address.City, AddressLine = personDTO.address.AddressLine };

                var _current = await _db.Personal.FirstOrDefaultAsync(y => y.FirstName.Equals(person.FirstName) ||
                     y.LastName.Equals(person.LastName));

                if (_current != null)
                {
                    _current.FirstName = person.FirstName;
                    _current.LastName = person.LastName;

                    _db.Entry(_current).Reference(r => r.Address).Load();

                    if (person.Address != null)
                    {
                        if (_current.Address == null)
                            _current.Address = person.Address;
                        else
                        {
                            _current.Address.City = person.Address.City;
                            _current.Address.AddressLine = person.Address.AddressLine;
                        }

                    }
                    else
                        if (_current.Address != null)
                        _db.Addresses.Remove(_current.Address);

                    if (!_db.ChangeTracker.HasChanges())
                        goto Error;

                    if (await _db.SaveChangesAsync() > 0)
                        return _current.Id;
                    else
                        goto Error;
                }

                /*
                 I configure that 
                firstname, lastname, City, addressLine must unique.
                And in Fluent api was wroted Contraints.
                About update I don't see any Id for update that's why I configured that the code always was updated
                if all datas equals ChangeTracker.HasChanges() was returned -1;

                For the case update with ID
                 if (await _db.Personal.AnyAsync(y => y.Id == person.Id && y.FirstName.Equals(person.FirstName) || y.LastName.Equals(person.LastName)))
                    return -1;

                if (person.Address != null && await _db.Addresses.AnyAsync(y => y.Id == person.Address.Id && y.City.Equals(person.Address.City) ||
                    y.AddressLine.Equals(person.Address.AddressLine)))
                    return -1;

                So that a invalid request does not go to the database.
                And if will come then will fall catch. Database will not accept.

                As a unique date, you could be choose not a column, but a row.
                 */

                _db.Add(person);

                if (await _db.SaveChangesAsync() > 0)
                    return person.Id;
                else
                    goto Error;
            }
            catch (Exception ex)
            {
                var log = ex.ToString();
                goto Error;
            }
        Error: return -1;
        }
    }
}
