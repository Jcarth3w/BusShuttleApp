using DomainModel;

namespace BusShuttleManager.Services
{
    public interface IEntryService
    {
        public List<Entry> getAllEntries();

        public Entry findEntryById(int id);

        public void CreateNewEntry(DateTime timeStamp, int boarded, int leftBehind);

        public List<Entry> getEntriesByDate(DateTime dateTime);
    }

}