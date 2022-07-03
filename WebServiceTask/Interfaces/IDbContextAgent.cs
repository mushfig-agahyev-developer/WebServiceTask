using WebServiceTask.DTO;
using WebServiceTask.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebServiceTask.Interfaces
{
    public interface IDbContextAgent
    {
        List<PersonDTO> GetAllPersonal(BaseFilter queryParameters);
        Task<List<PersonDTO>> GetAllPersonalAsync(BaseFilter queryParameters);

        int PersonalCount(BaseFilter queryParameters);
        Task<int> PersonalCountAsync(BaseFilter queryParameters);
    }
}
