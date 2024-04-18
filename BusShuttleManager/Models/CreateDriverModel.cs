using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;


namespace BusShuttleManager.Models
{
    public class CreateDriverModel
    {
       
        public int Id {get; set;}

        [StringLength(60, MinimumLength = 3)]
        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string Email {get; set;}

        public static CreateDriverModel NewDriver(int amountOfDrivers)
        {
            var newId = amountOfDrivers + 1;

            return new CreateDriverModel
            {
                Id = newId,
                FirstName = "",
                LastName = "",
                Email = ""
            };
        }
    }
}