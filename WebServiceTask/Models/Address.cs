using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Models
{
    public class Address
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "City name is required!"),
           StringLength(100, ErrorMessage = "City name can't be more 100 characters!")]
        public string City { get; set; }
        [Required(ErrorMessage = "City name is required!"),
           MinLength(100, ErrorMessage = "Address must be minimum 10 characters!")]
        public string AddressLine { get; set; }
        public virtual Person Person { get; set; }
    }
}
