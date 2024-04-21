using DomainModel;

namespace BusShuttleManager.Services
{
    public interface IDriverService
    {
        public List<Driver> getAllDrivers();

        public List<Driver> getActiveDrivers();

        public Driver findDriverById(int id);

        public void UpdateDriverById(int id, string fName, string lName);

        public int GetAmountOfDrivers();

        public void CreateNewDriver(int id, string fName, string lName, string email);

        public string FindDriverByEmail(string email);

        public void deactivateDriver(int id);
    }

}