using WebServiceTask.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Interfaces
{
    public interface ISeedDbContextInitialValues
    {
        Task Initialize(AppDbContext context);
    }
}
