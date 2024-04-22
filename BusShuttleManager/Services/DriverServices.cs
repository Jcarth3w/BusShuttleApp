using DomainModel;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BusShuttleManager.Data;
namespace BusShuttleManager.Services
{
    public class DriverServices : IDriverService
    {
        private readonly ILogger<DriverServices> logger;
        private DataContext db;
        List<Driver> drivers;

        private readonly ApplicationDbContext _applicationDb;

        public DriverServices(ApplicationDbContext applicationDb, ILogger<DriverServices> logger)
        {
            _applicationDb = applicationDb;
            this.logger = logger;
        }


        public List<Driver> getAllDrivers()
        {
            logger.LogInformation("Getting all drivers...");
            db = new DataContext();
            drivers = db.Driver
                .Select(d => new Driver(d.Id, d.FirstName, d.LastName)).ToList();

            return drivers;
        }

        public List<Driver> getActiveDrivers()
        {
            logger.LogInformation("Getting all active drivers...");
            db = new DataContext();
            return db.Driver.Where(driver => driver.IsActive).ToList();
        }

        public Driver findDriverById(int id)
        {
            logger.LogInformation("Finding driver by ID: {Id}", id);
            db = new DataContext();
            var driver = db.Driver
                .SingleOrDefault(driver => driver.Id == id);
            
   
            return new Driver(driver);
 
        }

        public void UpdateDriverById(int id, string fName, string lName)
        {
            logger.LogInformation("Updating driver with ID: {Id}", id);
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
            logger.LogInformation("Getting total amount of drivers...");
            db = new DataContext();
            return db.Driver.Count();
        }


        public void CreateNewDriver(int id, string fName, string lName, string email)
        {
            logger.LogInformation("Creating new driver with ID: {Id}", id);
            db = new DataContext();
            db.Add(new Driver{Id=id, FirstName=fName, LastName=lName, Email=email});
            db.SaveChanges();
        }
        
        public string FindDriverByEmail(string email)
        {
            logger.LogInformation("Finding user with email: {Email}", email);
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

        public void deactivateDriver(int id)
        {
            logger.LogInformation("Deactivating driver with ID: {Id}", id);
            db = new DataContext();
            var existingDriver = db.Driver.FirstOrDefault(d => d.Id == id);
        
            if (existingDriver != null)
            {
                existingDriver.IsActive = false;
                db.SaveChanges();
            }
        }
    }
}