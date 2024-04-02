using DomainModel;

namespace BusShuttleManager.Services
{
    public interface IBusService
    {
        public List<Bus> getAllBusses();

        public Bus findBusById(int id);

        public void UpdateBusById(int id, string name);

        public void CreateNewBus(string name);
    }

}