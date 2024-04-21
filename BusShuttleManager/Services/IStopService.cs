using DomainModel;

namespace BusShuttleManager.Services
{
    public interface IStopService
    {
        public List<Stop> getAllStops();

        public List<Stop> getActiveStops();

        public Stop findStopById(int id);

        public void UpdateStopById(int id, string name);

        public int GetAmountOfStops();

        public void CreateNewStop(int id, string name, double lat, double lon);

        public void deactivateStop(int id);

    }

}