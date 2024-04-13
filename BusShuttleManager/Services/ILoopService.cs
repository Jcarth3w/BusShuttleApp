using DomainModel;

namespace BusShuttleManager.Services
{
    public interface ILoopService
    {
        public List<Loop> getAllLoops();

        public Loop getLoopById(int id);

        public void UpdateLoopById(int id, string name);

        public int GetAmountOfLoops();

        public void CreateNewLoop(int id, string name);
    }

}