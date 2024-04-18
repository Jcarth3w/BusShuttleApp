using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BusShuttleManager.Data;
namespace BusShuttleManager.Services
{
    public class DriverServices : IDriverService
    {
        private DataContext db;
        List<Driver> drivers;

        private readonly ApplicationDbContext _applicationDb;

        public DriverServices(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }


        public List<Driver> getAllDrivers()
        {
            db = new DataContext();
            drivers = db.Driver
                .Select(d => new Driver(d.Id, d.FirstName, d.LastName)).ToList();

            return drivers;
        }

        public Driver findDriverById(int id)
        {
            db = new DataContext();
            var driver = db.Driver
                .SingleOrDefault(driver => driver.Id == id);
            
   
            return new Driver(driver);
 
        }

        public void UpdateDriverById(int id, string fName, string lName)
        {
            db = new DataContext();
            
            var existingDriver = db.Driver.SingleOrDefault(driver => driver.Id == id);
            existingDriver.Update(fName, lName);

            var driver = db.Driver
                .SingleOrDefault(driver => driver.Id == existingDriver.Id);
            
            if(driver != null)
            {
                driver.FirstName = fName;
                driver.LastName = lName;
                db.SaveChanges();
            }
        }

        public int GetAmountOfDrivers() 
        {
            db = new DataContext();
            return db.Driver.Count();
        }


        public void CreateNewDriver(int id, string fName, string lName, string email)
        {
            db = new DataContext();
            db.Add(new Driver{Id=id, FirstName=fName, LastName=lName, Email=email});
            db.SaveChanges();
        }

        public void DeleteDriver(int id)
        {
            db = new DataContext();
            var driver = db.Driver.FirstOrDefault(d => d.Id == id);
            if (driver != null)
            {
                db.Driver.Remove(driver);
                db.SaveChanges(); 
            }
        }
        
        public string FindDriverByEmail(string email)
        {
            var user = _applicationDb.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return user.Email;
            }
            else
            {
                return "";
            }
        }
    }
}