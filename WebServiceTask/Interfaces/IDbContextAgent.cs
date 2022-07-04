using WebServiceTask.DTO;
using WebServiceTask.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceTask.Models;

namespace WebServiceTask.Interfaces
{
    public interface IDbContextAgent
    {
        List<PersonDTO> GetAllPersonal(GetAllRequest request);
        Task<List<PersonDTO>> GetAllPersonalAsync(GetAllRequest request);

        int PersonalCount(GetAllRequest request);
        Task<int> PersonalCountAsync(GetAllRequest request);

        long Save(PersonDTO person);
        Task<long> SaveAsync(PersonDTO person);
    }
}
