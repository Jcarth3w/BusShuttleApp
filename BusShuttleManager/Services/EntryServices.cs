using DomainModel;
namespace BusShuttleManager.Services
{
    public class EntryServices : IEntryService
    {
        private DataContext db;
        List<Entry> entries;

        public List<Entry> getAllEntries()
        {
            db = new DataContext();
            entries = db.Entry
                .Select(e => new Entry(e.TimeStamp, e.Boarded, e.LeftBehind, e.LoopId, e.DriverId, e.StopId, e.BusId)).ToList();
            return entries;
        }

        public Entry findEntryById(int id)
        {
            db = new DataContext();
            var entry = db.Entry
                .SingleOrDefault(e =>e.Id == id);

            return new Entry(entry);
        }


        public void CreateNewEntry(DateTime timeStamp, int boarded, int leftBehind)
        {
            db = new DataContext();
            var totalEntries = db.Entry.Count();
            db.Add(new Entry{Id = totalEntries+1, TimeStamp=timeStamp, Boarded=boarded, LeftBehind=leftBehind});
            db.SaveChanges();
        }

        public List<Entry> getEntriesByDate(DateTime selectedDate)
        {
            db = new DataContext();
            return db.Entry
                     .Where(entry => entry.TimeStamp.Date == selectedDate.Date)
                     .ToList();
        }
    }
}