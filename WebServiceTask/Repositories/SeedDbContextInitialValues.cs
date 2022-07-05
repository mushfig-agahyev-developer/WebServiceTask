using WebServiceTask.DAL;
using WebServiceTask.Interfaces;
using WebServiceTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Repositories
{
    public class SeedDbContextInitialValues : ISeedDbContextInitialValues
    {
        public async Task Initialize(AppDbContext context)
        {
            context.Personal.AddRange(new List<Person>() {
                new Person(){
                 FirstName = "Danilo",
                 LastName = "Fedorko",
                 Address =  new Address() { City = "Андрушівка", AddressLine = "Андрушівка,26, Полтавська область, 37142, Україна" }
                }});

            bool flag = await context.SaveChangesAsync() > 0;
        }
    }
}
