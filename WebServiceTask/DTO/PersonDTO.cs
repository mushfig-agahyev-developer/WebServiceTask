using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebServiceTask.Models;

namespace WebServiceTask.DTO
{
    public class PersonDTO
    {
        [Required(ErrorMessage = "firstName name is required!"),
          StringLength(50, ErrorMessage = "The firstName name can't be more 50 characters!")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "lastName name is required!"),
        StringLength(50, ErrorMessage = "The lastName name can't be more 50 characters!")]
        public string lastName { get; set; }
        public AddressDTO address { get; set; }

        public static implicit operator PersonDTO(Person person)
        {
            if (person == null)
                return null;

            PersonDTO personDTO = new PersonDTO()
            {
                firstName = person.FirstName,
                lastName = person.LastName,
                address = person.Address
            };
            return personDTO;
        }
    }
}
