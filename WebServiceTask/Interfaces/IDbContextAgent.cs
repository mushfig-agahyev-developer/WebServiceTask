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
        List<AddressDTO> GetAll(BaseFilter queryParameters);
        Task<List<AddressDTO>> GetAllAsync(BaseFilter queryParameters);
    }
}
