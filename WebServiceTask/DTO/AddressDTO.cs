using WebServiceTask.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.DTO
{
    public class AddressDTO
    {
        [Required(ErrorMessage = "city name is required!"),
           StringLength(100, ErrorMessage = "City name can't be more 100 characters!")]
        public string City { get; set; }
        [Required(ErrorMessage = "AddressLine is required!"),
           StringLength(200, ErrorMessage = "AddressLine must be maximum 200 characters!")]
        public string AddressLine { get; set; }

        public static implicit operator AddressDTO(Address address)
        {
            if (address == null)
                return null;

            AddressDTO _model = new AddressDTO()
            {
                AddressLine = address.AddressLine,
                City = address.City
            };

            return _model;
        }
    }
}
