using System.ComponentModel.DataAnnotations;

namespace DomainModel
{
     public class Bus
     {
          [Key]
          public int Id { get; set;}

          public string BusName {get; set;}

          public bool IsActive { get; set; }

          public Bus()
          {
               IsActive = true;
          }

          public Bus(int id, string name)
          {
               Id = id;
               BusName = name;
          }

          public Bus(string name)
          {
               BusName = name;
          }

          public Bus(Bus bus)
          {
               Id = bus.Id;
               BusName = bus.BusName;
          }

          public void Update(string name)
          {
               BusName = name;
          }
     }
}

