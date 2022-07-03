using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Models
{
    public class Person
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Person name is required!"), 
            StringLength(50, ErrorMessage = "Person name can't be more 50 characters!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Person lastname is required!"),
            StringLength(50, ErrorMessage = "Person lastname can't be more 50 characters!")]
        public string LastName { get; set; }

        public long? AddressId { get; set; }
        public virtual Address Address { get; set; }

    }
}
